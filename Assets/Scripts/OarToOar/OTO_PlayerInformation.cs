using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTO_PlayerInformation : MonoBehaviour
{
    Playerinput pinput;
    string InputPath;
    public int PlayerIndex { get; set; }
    public int nScore { get; set; }
    OTO_GameManger OTOGamemanager;
    [SerializeField]
    OTO_UiController UiController;
    Vector3 vOriginPos;

    Animator Anim;
    [SerializeField]
    float fStunTime;
    GameObject InBoatObject;

    bool IsRaglanokBuffOn;
    [SerializeField]
    float fAttackPower= 1.0f;
    [SerializeField]
    float fjumpHight = 2.0f;
    public bool bIsJumped { get; set; }

    Rigidbody rigid;
    bool bIsHit;
    [SerializeField]
    bool bIsIdle;

    int nNowboatIndex;
    bool isPaddle;
    // Start is called before the first frame update
    void Start()
    {
        pinput = GetComponent<Playerinput>();
        OTOGamemanager = GameObject.Find("GameManager").GetComponent<OTO_GameManger>();
        Anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        UiController.gameObject.SetActive(true);
        UiController.Init(this);
        PlayerIndex = pinput.GetPlayerInDex();
        InputPath = "PAD" + PlayerIndex.ToString();
        nScore = 0;
        IsRaglanokBuffOn = false;
        vOriginPos = transform.position;
        bIsJumped = false;
        bIsHit = false;
        bIsIdle = true;
        isPaddle = false;
        nNowboatIndex = -1;
    }
    private void Hitdisable()
    {
        bIsHit = false;
    }
    private void Update()
    {
        AnimationFunction();
        ButtonInput();
        JumpMovement();
        if (OTOGamemanager.bIsRaglanok == true&& IsRaglanokBuffOn == false)
        {
            RaglanokBuffEnable();
        }
    }
    private void RaglanokBuffEnable()
    {
        IsRaglanokBuffOn = true;
        pinput.SetAccSpeed(pinput.GetAccSpeed() * 1.5f);
        fAttackPower = fAttackPower * 1.5f;
    }
    private void PressAButton()
    {
        if (bIsJumped == false)
        {
            JumpEvent();
            OTO_SoundManager.instance.CallJump();
        }
    }

    public void Anim_CanMove()
    {
        pinput.IsCanMove = true;
    }
    private void ButtonInput()
    {
        if (PlayerIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PressAButton();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PressBButton();
            }
        }
        else
        {
            if (Input.GetButtonDown(InputPath + "_Button_A"))
            {
                PressAButton();
            }
            if (Input.GetButtonDown(InputPath + "_Button_B"))
            {
                PressBButton();
            }
        }
    }
    private void AnimationFunction()
    {
        //Anim.SetFloat("MoveSpeed", pinput.fNowSpeed);
        if (Mathf.Abs(rigid.velocity.x) <= 0.1f && Mathf.Abs(rigid.velocity.z) <= 0.1f)
        {
            Anim.SetFloat("MoveSpeed", 0.0f);
        }
        else
            Anim.SetFloat("MoveSpeed", 1.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallGround"))
        {
            FallDownToWater();
        }
    }
    public void Anim_AttackEndEvent()
    {
        print("asd");
        pinput.IsCanMove = true;
    }
    public void Anim_StunedEvent()
    {
        Anim.SetInteger("StunState", 2);
        Invoke("StunDisable", fStunTime);
    }
    private void StunDisable()
    {
        Anim.SetInteger("StunState", 0);

        bIsHit = false;
        pinput.IsCanMove = true;
        bIsIdle = true;
    }

    private void JumpMovement()
    {
       // if (pinput.GetIsGrounded() == true)
        {
            if (bIsJumped == true)
            {
                RaycastHit Hit;
                Debug.DrawRay(transform.position, Vector3.down * 1.2f, Color.red, 0.3f);
                if (Physics.Raycast(transform.position, Vector3.down, out Hit, 1.2f))
                {
                    if (rigid.velocity.y < 0)
                    {
                        print("Grounded");
                        print(Hit.transform.name);
                        if (!Hit.collider.CompareTag("FallGround"))
                        {
                                if (Hit.collider.CompareTag("OTO_Paddle"))
                                {
                                    if (Hit.transform.GetComponentInParent<OTO_Boar>().nBoatIndex != nNowboatIndex)
                                    {
                                        if (isPaddle == true)
                                            AddScore(4);
                                        else
                                            AddScore(2);

                                        isPaddle = true;
                                    nNowboatIndex = Hit.transform.GetComponentInParent<OTO_Boar>().nBoatIndex;
                                    }
                                }
                                else if (Hit.collider.CompareTag("OTO_Boat"))
                                  {
                                     if (Hit.transform.GetComponentInParent<OTO_Boar>().nBoatIndex != nNowboatIndex)
                                     {
                                         AddScore(1);
                                         isPaddle = false;
                                         nNowboatIndex = Hit.transform.GetComponentInParent<OTO_Boar>().nBoatIndex;
                                     }
                                 }
                        }
                        bIsJumped = false;
                        Anim.SetBool("IsJump", false);
                    }
                }
            }
        }
    }
    private void AddScore(int _Value)
    {
        if (OTOGamemanager.bIsRaglanok == false)
            nScore += _Value;
        else
            nScore += _Value * 2;

        UiController.SetScoreText(nScore);
    }
    public void JumpEvent(bool IsByHit = false)
    {
            bIsJumped = true;
        if (rigid.velocity.y <= 0)
            rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

            Vector3 jumpvelocity = Vector3.up * Mathf.Sqrt(fjumpHight * -Physics.gravity.y);
            rigid.AddForce(jumpvelocity, ForceMode.VelocityChange);
        if (IsByHit == false)
            Anim.SetBool("IsJump", true);
    }
    public void EventHit(Vector3 ForceDirection, float Power)
    {
        rigid.AddForce(ForceDirection * Power, ForceMode.VelocityChange);
        Anim.SetInteger("SutnStage", 1);
        Anim.SetTrigger("IsStun");
        bIsHit = true;
    }
    private void PressBButton()
    {
        if (pinput.IsCanMove == true&& bIsIdle == true)
        {
            Anim.SetTrigger("IsAttack");
        }
    }

    public void Anim_EndEvent()
    {
        pinput.IsCanMove = true;
        bIsIdle = true;
    }
    public void Anim_StartEvent()
    {
        pinput.IsCanMove = false;
        bIsIdle = false;
    }
    public void Anim_AttackEvent()
    {
        RaycastHit[] Hit = Physics.RaycastAll(transform.position+new Vector3(0,1.5f,0), transform.forward, 2.0f);
        Debug.DrawRay(transform.position + new Vector3(0, 1.5f, 0), transform.forward * 2.0f, Color.red, 3.0f);
        for (int i = 0; i < Hit.Length; i++)
        {
            if (Hit[i].collider.CompareTag("Player"))
            {
                OTO_PlayerInformation HitPlayer = Hit[i].collider.GetComponent<OTO_PlayerInformation>();
                if (HitPlayer.bIsHit == false)
                {
                    print("AttackHit");
                    HitPlayer.JumpEvent();
                    HitPlayer.EventHit(transform.forward, fAttackPower);
                    OTO_SoundManager.instance.CallPunch();
                }
            }
        }
    }
    private void FallDownToWater()
    {
        Effect_Object_Twinkle Clone = transform.gameObject.AddComponent<Effect_Object_Twinkle>();
        Clone.Init(8,true,vOriginPos);
        AddScore(-1);
        StartCoroutine(FreezeMove(1.6f));
        OTO_SoundManager.instance.CallDive();
    }
    private IEnumerator FreezeMove(float _Time)
    {
        pinput.IsCanMove = false;
        yield return new WaitForSeconds(_Time);
        pinput.IsCanMove = true;
        rigid.velocity = new Vector3(0, 0, 0);
        yield return 0 ;
    }
}
