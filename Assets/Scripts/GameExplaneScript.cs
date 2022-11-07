using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameExplaneScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] Players;

    [SerializeField]
    TextMeshProUGUI[] playerReadText;

    [SerializeField]
    Image ExplaneImage;

    [SerializeField]
    Sprite[] gameepxlanelist;

    [SerializeField]
    GameObject CountDownImage;

    [SerializeField]
    Sprite[] countdownnumber;


    bool[] KeyDownCheck = new bool[4];
    float[] keydowntick = new float[4];
    bool[] IsReady = new bool[4];

    float fGameStartTime;

    // Start is called before the first frame update
    void Start()
    {
        if (ExplaneImage.gameObject.activeSelf == false)
            ExplaneImage.gameObject.SetActive(true);
        if (CharaterManager.instance.NextSceneName == "SKAL")
            ExplaneImage.sprite = gameepxlanelist[0];
        if (CharaterManager.instance.NextSceneName == "OTO")
            ExplaneImage.sprite = gameepxlanelist[1];

        for (int i = 0; i < Players.Length; i++)
            Players[i].SetActive(false);

        for (int i = 0; i < CharaterManager.instance.MaxPlayerIndex; i ++)
        {
            Players[i].SetActive(true);
            playerReadText[i].text = "Readt To Press A";
        }

        fGameStartTime = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterSelecter();
        GameStartCheck();
    }
    private void CharacterSelecter()
    {
        for (int i = 0; i < 1; i++)
        {
            string InputPath = "PAD" + (i + 1).ToString();
            if (Players[i].activeSelf == false)//비활성화상태일때
            {
            }
            else//활성화상태일때
            {
               
                if (Input.GetButtonDown(InputPath + "_Button_A"))
                {
                    if (IsReady[i] == false)
                    {
                        IsReady[i] = true;
                        playerReadText[i].text = "Ready";
                    }
                    else
                    {
                        IsReady[i] = false;
                        playerReadText[i].text = "Press A to Ready";
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
                CountDownImage.GetComponent<Image>().sprite = countdownnumber[Mathf.CeilToInt(fGameStartTime) - 1];
                fGameStartTime -= Time.deltaTime;
                if (fGameStartTime <= 0)
                {
                    SceneManager.LoadScene(CharaterManager.instance.NextSceneName);
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
}
