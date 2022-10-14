using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinput : MonoBehaviour
{
    Vector3 originPos;
    Rigidbody rigid;
    [SerializeField]
    float Speed = 5.0f;
    Vector3 Dir = Vector3.zero;
    [SerializeField]
    float jumpHight = 3.0f;
    [SerializeField]
    bool IsGrounded;
    [SerializeField]
    LayerMask groundedlayer;

    [SerializeField]
    int PlayerIndex;

    float fAccSpeed = 1.0f;

    [SerializeField]
    float MaxSpeed;


    public float fNowSpeed;
    public bool IsCanMove { get; set; } = false;
    public float fCanMoveTime { get; set; } = 0.0f;
    public int GetPlayerInDex()
    {
        return PlayerIndex;
    }
    public float GetAccSpeed()
    {
        return fAccSpeed;
    }
    void Start()
    {
        originPos = transform.position;
        rigid = GetComponent<Rigidbody>();
        IsGrounded = false;
        IsCanMove = false;
        fCanMoveTime = 3.0f;
        fAccSpeed = 1.0f;
        fNowSpeed = 0;
        print("PAD" + PlayerIndex.ToString());
    }
    public void SetAccSpeed(float _value)
    {
        fAccSpeed = _value;
    }
    private void DebuffFunction()
    {
        //if (IsCanMove == false)
        //{
        //    fCanMoveTime -= Time.deltaTime;
        //    if (fCanMoveTime <= 0)
        //    {
        //        fCanMoveTime = 0;
        //        IsCanMove = true;
        //    }
        //}
    }
    private void KeyboardInput()
    {
        CheckGrounded();
        if (IsCanMove == false)
        {
            Dir = Vector3.zero;
            fNowSpeed = 0.0f;
            return;
        }

        Dir.x = Input.GetAxisRaw("Horizontal");
        Dir.z = Input.GetAxisRaw("Vertical");
        if (Dir != Vector3.zero)
        {
            transform.forward = Dir;
            if (IsCanMove == true)
            {
                fNowSpeed = 1.0f;
            }
            else
            {
                fNowSpeed = 0.0f;
            }
        }
        else
        {
            fNowSpeed = 0.0f;
        }
        if (Dir.x != 0)
        {
             rigid.AddForce(Vector3.right * Dir.x * Speed* fAccSpeed * Time.deltaTime);
        }
        if (Dir.z != 0)
        {
              rigid.AddForce(Vector3.forward * Dir.z * Speed * fAccSpeed * Time.deltaTime);
        }
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, MaxSpeed);
        //  rigid.velocity = new Vector3(Dir.x, 0, Dir.z) * Speed * SpeefAcc * Time.deltaTime;

    }
    private void MovementKeyInput()
    {
        CheckGrounded();
        if (IsCanMove == false)
        {
            Dir = Vector3.zero;
            fNowSpeed = 0.0f;
            return;
        }
        string InputPath = "PAD" + PlayerIndex.ToString();
        Dir.x = Input.GetAxis(InputPath+"_DPAD_Horizontal");
        Dir.z = Input.GetAxis(InputPath + "_DPAD_Vertical");
        print(Dir.x);
        if (Dir.x == 0)
            Dir.x = Input.GetAxisRaw(InputPath + "_LSTICK_Horizontal");
        if (Dir.z == 0)
            Dir.z = Input.GetAxisRaw(InputPath + "_LSTICK_Vertical");
        if (Dir != Vector3.zero)
        {
            transform.forward = Dir;
            if (IsCanMove == true)
                fNowSpeed = 1.0f;
            else
                fNowSpeed = 0.0f;
        }
        else
        {
            fNowSpeed = 0.0f;
        }

        if (Dir.x != 0)
        {
            //rigid.AddForce(Vector3.right * Dir.x * Speed* fAccSpeed * Time.deltaTime);
            rigid.velocity = Vector3.right * Dir.x * Speed * fAccSpeed * Time.deltaTime;
        }
        if (Dir.z != 0)
        {
            // rigid.AddForce(Vector3.forward * Dir.z * Speed * fAccSpeed * Time.deltaTime);
            rigid.velocity = Vector3.forward * Dir.z * Speed * fAccSpeed * Time.deltaTime;
        }
      //  rigid.velocity = new Vector3(Dir.x, 0, Dir.z) * Speed * SpeefAcc * Time.deltaTime;

    }
    private void FixedUpdate()
    {
        if (PlayerIndex != 0)
        {
            MovementKeyInput();
        }
        if (PlayerIndex == 0)
        {
            KeyboardInput();
        }
    }
    // Update is called once per frame
    void Update()
    {
        DebuffFunction();
    }
    private void CheckGrounded()
    {
        RaycastHit Hit;
        //Debug.DrawRay(transform.position, Vector3.down * 1.0f, Color.black, 0.3f);
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out Hit, 1.0f, groundedlayer))
        {
            IsGrounded = true;
            return;
        }
        IsGrounded = false;
    }
    public bool GetIsGrounded()
    {
        return IsGrounded;
    }
}
