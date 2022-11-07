using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobiManager : MonoBehaviour
{
    Vector3 MainCamPosition = new Vector3(-66.9229965f, 5.7670002f, 139.587997f);
    Vector3 CharaterSelectCamPosition = new Vector3(-74.7040024f, 2.91100001f, 150.113007f);
    // Start is called before the first frame update
    [SerializeField]
    GameObject Logo;
    [SerializeField]
    GameObject Pressstart;

    [SerializeField]
    GameObject JoinObject;

    bool IsCharacterSelect;
    float CamMovingTime;
    bool IsCamMoveDone;


    [SerializeField]
    GameObject[] Players = new GameObject[4];

    [SerializeField]
    TextMeshProUGUI[] readytext = new TextMeshProUGUI[4];

    [SerializeField]
    GameObject CountDownImage;

    [SerializeField]
    Sprite[] countdownnumber;
    
    bool[] KeyDownCheck = new bool[4];
    float[] keydowntick = new float[4];
    bool[] IsReady = new bool[4];

    float fGameStartTime;
    

    void Start()
    {
        this.transform.position = MainCamPosition;
        IsCharacterSelect = false;
        CamMovingTime = 0;
        for (int i = 0; i < 4; i++)
        {
            KeyDownCheck[i] = false;
            IsReady[i] = false;
            keydowntick[i] = 0.2f;
            readytext[i].text = "Press A to Ready";
        }
        fGameStartTime = 3.0f;
        CountDownImage.SetActive(false);
        IsCamMoveDone = false;
    }

    private void GoCharacterSelect()
    {
        if (CamMovingTime < 1.0f)
        {
            CamMovingTime += Time.deltaTime * 1.0f;
        }
        else
        {
            CamMovingTime = 1.0f;
            JoinObject.SetActive(true);
            IsCamMoveDone = true;
        }
        this.transform.position = Vector3.Slerp(MainCamPosition, CharaterSelectCamPosition, CamMovingTime);
    }

    private void ChacterChange(int playerindex, int value)
    {
        for (int ii = 0; ii < 6; ii++)
        {
            Players[playerindex].transform.GetChild(ii).gameObject.SetActive(false);
        }
        CharaterManager.instance.PlayerCharacterIndex[playerindex] = value;
        if (CharaterManager.instance.PlayerCharacterIndex[playerindex] >= 6)
        {
            CharaterManager.instance.PlayerCharacterIndex[playerindex] = 0;
        }
        else if (CharaterManager.instance.PlayerCharacterIndex[playerindex] <= -1)
        {
            CharaterManager.instance.PlayerCharacterIndex[playerindex] = 5;
        }
        Players[playerindex].transform.GetChild(CharaterManager.instance.PlayerCharacterIndex[playerindex]).gameObject.SetActive(true);

    }
    private void CharacterSelecter()
    {
        for (int i = 0; i < 1; i++)
        {
            string InputPath = "PAD" + (i+1).ToString();
            if (Players[i].activeSelf == false)//비활성화상태일때
            {
                if (Input.GetButtonDown(InputPath + "_Button_A"))
                {
                    Players[i].SetActive(true);
                    CharaterManager.instance.MaxPlayerIndex++;
                    ChacterChange(i, i);
                    IsReady[i] = false;
                    readytext[i].text = "Press A to Ready";
                    readytext[i].gameObject.SetActive(true);
                }
            }
            else//활성화상태일때
            {
                if (Input.GetButtonDown(InputPath + "_Button_B"))
                {
                    Players[i].SetActive(false);
                    CharaterManager.instance.MaxPlayerIndex--;
                    readytext[i].gameObject.SetActive(false);
                }
                if (Input.GetButtonDown(InputPath + "_Button_A"))
                {
                    if (IsReady[i] == false)
                    {
                        IsReady[i] = true;
                        readytext[i].text = "Ready";
                    }
                    else
                    {
                        IsReady[i] = false;
                        readytext[i].text = "Press A to Ready";
                    }
                }
                if (KeyDownCheck[i] == false)
                {
                    float IsPressLeft = Input.GetAxis(InputPath + "_DPAD_Horizontal");

                    if (IsPressLeft >= 0.9f)
                    {
                        KeyDownCheck[i] = true;
                        ChacterChange(i, CharaterManager.instance.PlayerCharacterIndex[i] + 1);
                    }
                    else if (IsPressLeft <= -0.9f)
                    {
                        KeyDownCheck[i] = true;
                        ChacterChange(i, CharaterManager.instance.PlayerCharacterIndex[i] - 1);
                    }
                }
                else
                {
                    keydowntick[i] -= Time.deltaTime;
                    if (keydowntick[i] <= 0.0f)
                    {
                        KeyDownCheck[i] = false;
                        keydowntick[i] =0.2f;
                    }
                }
            }
        }
    }
    private void GameStartCheck()
    {
        bool check = false;
        for (int i = 0; i < 4; i++)
        {
            if (Players[i].activeSelf == true)
            {
                if (IsReady[i] == true)
                {
                    check = true;
                }
            }
        }
        if (CharaterManager.instance.MaxPlayerIndex > 0)
        {
            if (check == true)
            {
                if (CountDownImage.activeSelf == false)
                    CountDownImage.SetActive(true);
                CountDownImage.GetComponent<Image>().sprite = countdownnumber[Mathf.CeilToInt(fGameStartTime)-1];
                fGameStartTime -= Time.deltaTime;
                if (fGameStartTime <= 0)
                {
                    //    SceneManager.LoadScene("OarToOar");
                    CharaterManager.instance.NextSceneName = "SKAL";
                        SceneManager.LoadScene("SKAL");
                }
            }
            else
            {
                if (CountDownImage.activeSelf == true)
                    CountDownImage.SetActive(false);
                fGameStartTime = 3.0f;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsCharacterSelect == false)
        {          
            if (Input.anyKeyDown)
            {
                IsCharacterSelect = true;
                CamMovingTime = 0;
                Logo.AddComponent<FadeOut>();
                Pressstart.AddComponent<FadeOut>();
                IsCamMoveDone = false;
            }
        }
        else
        {
           //if(IsCamMoveDone == false)
           //    GoCharacterSelect();

            CharacterSelecter();
            GameStartCheck();
        }
    }
    private void FixedUpdate()
    {
        if (IsCharacterSelect == true)
        {
            if (IsCamMoveDone == false)
                GoCharacterSelect();
        }
    }
}
