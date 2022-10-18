using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKALPlayerInfomation : MonoBehaviour
{
    Playerinput pinput;
    string InputPath;

    bool IsDrink;
    int nBottleCount;
    int nintoxicationStack;
    public int PlayerIndex { get; set; }
    public int nScore { get; set; }
    public bool IsNearInBarTable { get; set; }
    public bool IsNearInTable { get; set; }
    SKALDrinkTable myDrinkTable;

    [SerializeField]
    float fStunTime = 2.0f;
    SKALGameManager skalManager;

    [SerializeField]
    SKALUiController UiController;
    [SerializeField]
    GameObject PlayerCanvas;


    [SerializeField]
    Animator Anim;

    Rigidbody rigid;

    private void Start()
    {
        pinput = GetComponent<Playerinput>();
        rigid = GetComponent<Rigidbody>();
        skalManager = GameObject.Find("GameManager").GetComponent<SKALGameManager>();
        UiController.gameObject.SetActive(true);
        PlayerCanvas.gameObject.SetActive(true);
        UiController.Init(this);
        PlayerIndex = pinput.GetPlayerInDex();
        InputPath = "PAD" + PlayerIndex.ToString();
        nScore = 0;
        SetIsDrink(false);
        nBottleCount = 0;
        nintoxicationStack = 0;
        IsNearInBarTable = false;
        IsNearInTable = false;
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
                //Vector3 jumpvelocity = Vector3.up * Mathf.Sqrt(jumpHight * -Physics.gravity.y);
                //rigid.AddForce(jumpvelocity, ForceMode.VelocityChange);
            }
        }
    }
    private void PressAButton()
    {
        if (IsNearInBarTable)
        {
            ReFillDrink();
        }
        if (IsNearInTable)
        {
            DrinkDrunk();
        }
    }
    private void PressBButton()
    {
        if (IsNearInTable)
        {
            pinput.IsCanMove = false;
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
            Anim.SetInteger("SutnStage", 0);
        }
    }
    public void Anim_DrinkEvent()
    {
        nBottleCount--;
        print("Anim_Drink");
        if (nBottleCount <= 0)
        {
            nBottleCount = 0;
            SetIsDrink(false);
            AddintoxicationStack();
        }
    }
    public void Anim_AttackEvent()
    {
        GenseiTable();
    }
    public void Anim_AttackEndEvent()
    {
        print("asd");
          pinput.IsCanMove = true;
    }
    public void Anim_StunedEvent()
    {
        Anim.SetInteger("StunState", 1);
        Invoke("StunDisable", fStunTime);
    }
    private void StunDisable()
    {
        Anim.SetInteger("StunState", 0);

        pinput.IsCanMove = true;
    }
    public void Anim_PickUpEvent()
    {
            nBottleCount++;
            print("Anim_PickUpEvent");
            if (nBottleCount > 3 + nintoxicationStack)
            {
                SetIsDrink(true);
        }
    }
    public void Anim_CanMove()
    {
        pinput.IsCanMove = true;
    }
    private void ReFillDrink()
    {
        if (IsDrink == false)
        {
            Anim.SetTrigger("IsPickUp");
            pinput.IsCanMove = false;
        }
    }
    private void DrinkDrunk()
    {
        if (IsDrink == true)
        {
            Anim.SetTrigger("IsDrinking");
            pinput.IsCanMove = false;
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
