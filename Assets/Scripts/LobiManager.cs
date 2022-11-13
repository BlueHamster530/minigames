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
    public TextMeshProUGUI[] readytext = new TextMeshProUGUI[4];

    [SerializeField]
    GameObject CountDownImage;

    [SerializeField]
    Sprite[] countdownnumber;
    
    public bool[] IsReady = new bool[4];

    float fGameStartTime;
    

    void Start()
    {
        this.transform.position = MainCamPosition;
        IsCharacterSelect = false;
        CamMovingTime = 0;
        for (int i = 0; i < 4; i++)
        {
            IsReady[i] = false;
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
    private void GameStartCheck()
    {
        bool check = true;
        for (int i = 0; i < CharaterManager.instance.MaxPlayerIndex; i++)
        {
                if (IsReady[i] == false)
                {
                    check = false;
                }
        }
        if (check == true)
        {
            if (CharaterManager.instance.MaxPlayerIndex > 0)
            {
                if (CountDownImage.activeSelf == false)
                    CountDownImage.SetActive(true);
                CountDownImage.GetComponent<Image>().sprite = countdownnumber[Mathf.CeilToInt(fGameStartTime)-1];
                fGameStartTime -= Time.deltaTime;
                if (fGameStartTime <= 0)
                {
                    //    SceneManager.LoadScene("OarToOar");
                    CharaterManager.instance.NextSceneName = "SKAL";
                        SceneManager.LoadScene("GameExplaneScene");
                }
        }
        }
        else
        {
            if (CountDownImage.activeSelf == true)
                CountDownImage.SetActive(false);
            fGameStartTime = 3.0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsCharacterSelect == false)
        {          
            //if (Input.anyKeyDown)
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

          //  CharacterSelecter();
          //  CharacterSelecterByKeyBoard();
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
/*

 */