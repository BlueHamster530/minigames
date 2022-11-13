using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SKALPlayerInfomation : MonoBehaviour
{
    Playerinput pinput;
    bool IsDrink;
    [SerializeField]
    int nBottleCount;
    [SerializeField]
    int nintoxicationStack;
    public int PlayerIndex { get; set; }
    public int nScore { get; set; }
    public bool IsNearInBarTable { get; set; }
    public bool IsNearInTable { get; set; }
    SKALDrinkTable myDrinkTable;

    [SerializeField]
    float fStunTime = 2.0f;
    SKALGameManager skalManager;

    SKALUiController UiController;
    GameObject PlayerCanvas;

    bool bIsIdle;

    [SerializeField]
    Animator Anim;

    Rigidbody rigid;
    private void OnEnable()
    {
        pinput = GetComponent<Playerinput>();
        rigid = GetComponent<Rigidbody>();
        nScore = 0;
        skalManager = GameObject.Find("GameManager").GetComponent<SKALGameManager>();
        PlayerIndex = pinput.GetPlayerInDex();

        skalManager.AddPlayerinput(PlayerIndex-1, pinput);
        UiController = GameObject.Find("PlayerInfos").transform.GetChild(PlayerIndex-1).GetComponent<SKALUiController>();
        UiController.gameObject.SetActive(true);
        PlayerCanvas = GameObject.Find("PlayersCanvas").transform.GetChild(PlayerIndex - 1).gameObject;
        PlayerCanvas.gameObject.SetActive(true);
        PlayerCanvas.GetComponent<SKALPlayerUILock>().target = this.transform;
        UiController.Init(this);
        SetIsDrink(false);
        nBottleCount = 0;
        nintoxicationStack = 0;
        IsNearInBarTable = false;
        IsNearInTable = false;
        bIsIdle = true;
    }
    private void Start()
    {
    }
    private void AnimationFunction()
    {
        //Anim.SetFloat("MoveSpeed", pinput.fNowSpeed);
        if(Mathf.Abs(rigid.velocity.x) <= 0.1f && Mathf.Abs(rigid.velocity.z) <= 0.1f)
        {
            Anim.SetFloat("MoveSpeed", 0.0f);
        }
        else
        Anim.SetFloat("MoveSpeed", 1.0f);
    }
    private void Update()
    {
        ButtonInput();
        AnimationFunction();
        //Anim.SetFloat("MoveSpeed", pinput.fNowSpeed);
        if (skalManager.bIsRaglanok == true)
        {
            nintoxicationStack = 5;
        }
    }
    private void ButtonInput()
    {
        //if (PlayerIndex == 0)
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        PressAButton();
        //    }
        //    if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        PressBButton();
        //    }
        //}
        //else
        //{
        //    if (Input.GetButtonDown(InputPath + "_Button_A"))
        //    {
        //        PressAButton();
        //    }
        //    if (Input.GetButtonDown(InputPath + "_Button_B"))
        //    {
        //        PressBButton();
        //        //Vector3 jumpvelocity = Vector3.up * Mathf.Sqrt(jumpHight * -Physics.gravity.y);
        //        //rigid.AddForce(jumpvelocity, ForceMode.VelocityChange);
        //    }
        //}
    }
    public void PressAButton()
    {
        if (IsNearInBarTable)
        {
            if(bIsIdle == true)
                ReFillDrink();
        }
        if (IsNearInTable)
        {
            if (bIsIdle == true)
                DrinkDrunk();
        }
    }
    public void PressBButton()
    {
        if (IsNearInTable)
        {
            Anim.SetTrigger("IsAttack");
        }
    }
    private void GenseiTable()
    {
        if (IsNearInTable == true && myDrinkTable !=null)
        {
           bool IsSucess = myDrinkTable.GenseiTable(PlayerIndex);
            if (IsSucess)
            {
                if (skalManager.bIsRaglanok == false)
                {
                    nScore += nintoxicationStack;
                    nintoxicationStack = 0;
                }
                else
                {
                    nScore += nintoxicationStack*2;
                }
            }
            else
            {
                if (skalManager.bIsRaglanok == false)
                {
                    nScore -= nintoxicationStack;
                    nintoxicationStack = 0;
                }
                else
                {
                    nScore -= nintoxicationStack * 2;
                }
                nScore -= nintoxicationStack;
                nintoxicationStack = 0;
            }
        }
    }
    private void InitDrinks()
    {
        SetIsDrink(false);
        nBottleCount = 0;
    }
    public void AttckDoingDrink()
    {
        if (IsNearInTable == true && myDrinkTable != null && nBottleCount > 0&& IsDrink == true)
        {
            pinput.IsCanMove = false;
            InitDrinks();
            Anim.SetTrigger("IsStun");

            SKALSoundManager.instance.CallCrash();
            Anim.SetInteger("SutnStage", 1);
        }
    }
    public void Anim_DrinkEvent()
    {
        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            nBottleCount--;
            SKALSoundManager.instance.CallDrinking();
            print("Anim_Drink");
            if (nBottleCount <= 0)
            {
                nBottleCount = 0;
                SetIsDrink(false);
                AddintoxicationStack();
            }
        }
    }
    public void Anim_End()
    {
        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            bIsIdle = true;
        }
    }
    public void Anim_AttackEvent()
    {
        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            GenseiTable();
        }
    }
    public void Anim_StunedEvent()
    {

        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            Anim.SetInteger("StunState", 2);
            Invoke("StunDisable", fStunTime);
        }
    }
    private void StunDisable()
    {
        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            Anim.SetInteger("StunState", 0);
        }
    }
    public void Anim_PickUpEvent()
    {
        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            nBottleCount++;
            print("Anim_PickUpEvent");
            SKALSoundManager.instance.CallRefill();
            if (nBottleCount >= 1 + nintoxicationStack)
            {
                SetIsDrink(true);
            }
        }
    }
    public void Anim_EndEvent()
    {
        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            pinput.IsCanMove = true;
            bIsIdle = true;
        }
    }
    public void Anim_StartEvent()
    {
        if (SceneManager.GetActiveScene().name == "SKAL")
        {
            pinput.IsCanMove = false;
            bIsIdle = false;
        }
    }
    private void ReFillDrink()
    {
        if (IsDrink == false)
        {
            Anim.SetTrigger("IsPickUp");
        }
    }
    private void DrinkDrunk()
    {
        if (IsDrink == true)
        {
            Anim.SetTrigger("IsDrinking");
        }
    }
    private void SetIsDrink(bool _value)
    {
        IsDrink = _value;
       // DrinkImage.SetActive(_value);
    }
    private void AddintoxicationStack()
    {
        nintoxicationStack++;
        if (nintoxicationStack >= 5)
            nintoxicationStack = 5;
        pinput.SetAccSpeed(1 + (0.1f * nintoxicationStack));//스택만큼 속도 증가
        if (skalManager.bIsRaglanok == false)
        {
            nScore += 1;
        }
        else
        {
            nScore += 2;
        }

    }
    public int GetnintoxicationStack()
    {
        return nintoxicationStack;
    }
    public int GetnBottleCount()
    {
        return nBottleCount;
    }
    public void SetDrinkTable(SKALDrinkTable value)
    {
        myDrinkTable = value;
    }

}
