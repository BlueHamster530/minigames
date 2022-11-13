using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Playerinput : MonoBehaviour
{
    Vector3 originPos;
    Rigidbody rigid;
    [SerializeField]
    float Speed = 5.0f;

    public Vector3 Dir = Vector3.zero;

    string NowScneneName;

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

    [SerializeField]
    bool IsSKALGame;
    [SerializeField]
    bool IsMain;
    [SerializeField]
    bool ISOTOGame;
    [SerializeField]
    bool IsGameExplane;
    [SerializeField]
    bool IsEnding;

    [SerializeField]
    LobiInfomation Lobiinfo;
    [SerializeField]
    SKALPlayerInfomation skalinfo;
    [SerializeField]
    OTO_PlayerInformation otoinfo;
    [SerializeField]
    Gameexplaneinfomation explaneinfo;

    Animator Anim;

    [SerializeField]
    GameObject IndexImage;
    [SerializeField]
    Sprite[] indeximages;

    [SerializeField]
    public RuntimeAnimatorController MainAnim;
    [SerializeField]
    public RuntimeAnimatorController SkalAnim;
    [SerializeField]
    public RuntimeAnimatorController OtoAnim;
    [SerializeField]
    public RuntimeAnimatorController EndingAnim;

    public float fNowSpeed;
    public bool IsCanMove { get; set; } = false;
    public float fCanMoveTime { get; set; } = 0.0f;
    public int GetPlayerInDex()
    {
        return PlayerIndex;
    }
    public void SetPlayerInDex(int num)
    {
        PlayerIndex = num;
    }
    public float GetAccSpeed()
    {
        return fAccSpeed;
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        SceneChangeInit();
    }

    public void SceneChangeInit()
    {
        IsSKALGame = false;
        ISOTOGame = false;
        IsMain = false;
        IsGameExplane = false;
        IsEnding = false;
        skalinfo.enabled = false;
        otoinfo.enabled = false;
        Lobiinfo.enabled = false;
        explaneinfo.enabled = false;
        NowScneneName = SceneManager.GetActiveScene().name;
        MaxSpeed = 8.0f;
        switch (NowScneneName)
        {
            case "MainScene":
                {
                    IsMain = true;
                    Lobiinfo.enabled = true;
                    Anim.runtimeAnimatorController = MainAnim;
                    MaxSpeed = 0.0f;
                    transform.position = CharaterManager.instance.MainSpawnPosition[PlayerIndex-1];
                    transform.eulerAngles = CharaterManager.instance.MainSpawnRotate[PlayerIndex - 1];
                }
                break;
            case "GameExplaneScene":
                {
                    IsGameExplane = true;
                    explaneinfo.enabled = true;
                    MaxSpeed = 0.0f;
                }
                break;
            case "SKAL":
                {
                    IsSKALGame = true;
                    Anim.runtimeAnimatorController = SkalAnim;
                    skalinfo.enabled = true;
                    transform.position = CharaterManager.instance.SKALSpawnPosition[PlayerIndex - 1];
                    GameObject clone = Instantiate(IndexImage, this.transform.position + new Vector3(0, 5,0), Quaternion.Euler(new Vector3(90.0f,0,0)));
                    clone.GetComponent<SpriteRenderer>().sprite = indeximages[PlayerIndex - 1];
                    Destroy(clone, 2.0f);

                }
                break;
            case "OTO":
                {
                    ISOTOGame = true;
                    otoinfo.enabled = true;
                    Anim.runtimeAnimatorController = OtoAnim;
                    transform.position = CharaterManager.instance.OTOSpawnPosition[PlayerIndex - 1];
                    GameObject clone = Instantiate(IndexImage, this.transform.position + new Vector3(0, 5,0), Quaternion.Euler(new Vector3(90.0f, 0, 0)));
                    clone.GetComponent<SpriteRenderer>().sprite = indeximages[PlayerIndex - 1];
                    Destroy(clone, 2.0f);
                }
                break;
            case "EndingScene":
                {
                    IsEnding = true;
                    MaxSpeed = 0.0f;
                    Anim.runtimeAnimatorController = EndingAnim;
                    transform.position = CharaterManager.instance.EndingSpawnPosition[PlayerIndex - 1];
                    transform.eulerAngles = CharaterManager.instance.EndingSpawnRotate[PlayerIndex - 1];
                }
                break;
                
        }
    }
    private void OnDestroy()
    {

        SceneManager.sceneLoaded -= LoadedsceneEvent;
    }
    public void Awake()
    {
        originPos = transform.position;
        rigid = GetComponent<Rigidbody>();
        IsGrounded = false;
        IsCanMove = true;
        fCanMoveTime = 3.0f;
        fAccSpeed = 1.0f;
        fNowSpeed = 0;
        MaxSpeed = 8.0f;
        Anim = GetComponent<Animator>();

        if (CharaterManager.instance == null)
            return;
        SceneChangeInit();
        SceneManager.sceneLoaded += LoadedsceneEvent;
        int index = CharaterManager.instance.PlayerCharacterIndex[PlayerIndex];
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).GetChild(index).gameObject.SetActive(true);
    }
    public void OnPressAButton()
    {
        if (IsMain == true)
        {
            Lobiinfo.PressAButton();
        }
        if (IsGameExplane == true)
        {
            explaneinfo.PressAButton();
        }
        if (IsSKALGame == true)
        {
            skalinfo.PressAButton();
        }
        if (ISOTOGame == true)
        {
            otoinfo.PressAButton();
        }
    }
    public void OnPressBButton()
    {
        if (IsMain == true)
        {
            Lobiinfo.PressBButton();
        }
        if (IsGameExplane == true)
        {
            explaneinfo.PressBButton();
        }
        if (IsSKALGame == true)
        {
            skalinfo.PressBButton();
        }
        if (ISOTOGame == true)
        {
            otoinfo.PressBButton();
        }
    }
    public void SetAccSpeed(float _value)
    {
        fAccSpeed = _value;
    }
   public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        Dir = new Vector3(movement.x, 0, movement.y);
    }
    private void MovementKeyInput()
    {
        if (IsMain == true|| IsEnding == true) return;
        CheckGrounded();
        if (IsCanMove == false)
        {
            Dir = Vector3.zero;
            fNowSpeed = 0.0f;
            return;
        }

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
            rigid.AddForce(Vector3.right * Dir.x * Speed* fAccSpeed * Time.deltaTime);
        }
        if (Dir.z != 0)
        {
            rigid.AddForce(Vector3.forward * Dir.z * Speed * fAccSpeed * Time.deltaTime);
        }
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, MaxSpeed);

    }
    private void FixedUpdate()
    {
        MovementKeyInput();
    }
    // Update is called once per frame
    void Update()
    {
        if (NowScneneName != SceneManager.GetActiveScene().name)
        {
            SceneChangeInit();
        }
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
