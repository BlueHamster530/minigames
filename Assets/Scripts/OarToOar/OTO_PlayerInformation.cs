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

    GameObject InBoatObject;

    bool IsRaglanokBuffOn;
    [SerializeField]
    float fAttackPower= 1.0f;
    [SerializeField]
    float fjumpHight = 2.0f;
    public bool bIsJumped { get; set; }

    Rigidbody rigid;
    bool bIsHit;
    // Start is called before the first frame update
    void Start()
    {
        pinput = GetComponent<Playerinput>();
        OTOGamemanager = GameObject.Find("GameManager").GetComponent<OTO_GameManger>();
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
    }
    private void Hitdisable()
    {
        bIsHit = false;
    }
    private void Update()
    {
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
        }
    }
    private void ButtonInput()
    {
        if (PlayerIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PressAButton();
            }
            if (Input.GetKeyDown(KeyCode.S) && bIsJumped == true)
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
            if (Input.GetButtonDown(InputPath + "_Button_B")&&bIsJumped == true)
            {
                PressBButton();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallGround"))
        {
            FallDownToWater();
        }
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
                            if (InBoatObject != Hit.transform.parent.gameObject)
                            {
                                if (Hit.collider.CompareTag("OTO_Paddle"))
                                {
                                    AddScore(4);
                                }
                                else if (Hit.collider.CompareTag("OTO_Boat"))
                                {
                                    AddScore(2);
                                }
                                InBoatObject = Hit.transform.parent.gameObject;
                            }
                        }
                        bIsJumped = false;
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
    public void JumpEvent()
    {
            bIsJumped = true;
        if (rigid.velocity.y <= 0)
            rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

            Vector3 jumpvelocity = Vector3.up * Mathf.Sqrt(fjumpHight * -Physics.gravity.y);
            rigid.AddForce(jumpvelocity, ForceMode.VelocityChange);
    }
    public void EventHit(Vector3 ForceDirection, float Power)
    {
        rigid.AddForce(ForceDirection * Power, ForceMode.VelocityChange);
        Invoke("Hitdisable", 1.0f);
        bIsHit = true;
    }
    private void PressBButton()
    {
        RaycastHit Hit;
        print("Attacked");
        Debug.DrawRay(transform.position, transform.forward * 2.0f, Color.red,3.0f);
        if (Physics.Raycast(transform.position, transform.forward, out Hit, 2.0f))
        {
            if (Hit.collider.CompareTag("Player"))
            {
                OTO_PlayerInformation HitPlayer = Hit.collider.GetComponent<OTO_PlayerInformation>();
                if (HitPlayer.bIsHit == false)
                {
                    print("AttackHit");
                    HitPlayer.JumpEvent();
                    HitPlayer.EventHit(transform.forward, fAttackPower);
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
