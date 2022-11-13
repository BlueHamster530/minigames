using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SKALGameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TimeText;

    float fCurrentTime;
    [SerializeField]
    float fGamePlayTime;

    [SerializeField]
    GameObject RaglanokImage;

    [SerializeField]
    Slider TimeBar;

    bool IsGamePause;
    public bool bIsRaglanok { get; set; } = false;

    [SerializeField]
    int nPlayersIndex=2;

    [SerializeField]
    Playerinput[] Players = new Playerinput[4];

    [SerializeField]
    Sprite[] countdowniamge;
    [SerializeField]
    Image countdown;

    private void Start()
    {
        fCurrentTime = fGamePlayTime;
        nPlayersIndex = CharaterManager.instance.MaxPlayerIndex;

        IsGamePause = true;
        bIsRaglanok = false;
        RaglanokImage.SetActive(false);
        StartCoroutine("GameStartCount");
        countdown.gameObject.SetActive(true);
        countdown.sprite = countdowniamge[2];
    }
    public void AddPlayerinput(int index, Playerinput input)
    {
        Players[index] = input;
    }
    IEnumerator GameStartCount()
    {

        print("asd3");
        for (int i = 0; i < nPlayersIndex; i++)
        {
            Players[i].IsCanMove = false;
        }
        for (int i = 2; i >= 0; i--)
        {
            countdown.sprite = countdowniamge[i];
            yield return new WaitForSeconds(1);
        }

        countdown.gameObject.SetActive(false);
        IsGamePause = false;

        for (int i = 0; i < nPlayersIndex; i++)
        {
            Players[i].IsCanMove = true;
        }
        yield return null;
    }
    private void UpDateGUI()
    {
        TimeText.text = Mathf.FloorToInt(fCurrentTime / 60.0f).ToString() + ":" + Mathf.FloorToInt(fCurrentTime % 60.0f).ToString("00");
        TimeBar.value = (fCurrentTime / fGamePlayTime);
    }
    private void Update()
    {
        if (IsGamePause == false)
        {
            UpDateGUI();
            fCurrentTime -= Time.deltaTime;
            if (fCurrentTime <= 0)
            {//게임종료
                fCurrentTime = 0;
                GameTimeOver();
            }
            if (fCurrentTime <= 30 &&bIsRaglanok == false)
            {
                bIsRaglanok = true;
                RaglanokImage.SetActive(true);
                Invoke("DisableRagranokImage",1.0f);
            }
        }
    }
    private void DisableRagranokImage()
    {
        RaglanokImage.SetActive(false);
    }
    private void GameTimeOver()
    {
        int []Ranking = { 1, 1, 1, 1 };
        for (int i = 0; i < nPlayersIndex; i++)
        {
            Ranking[i] = 1;
            for (int ii = 0; ii < nPlayersIndex; ii++)
            {
                if (Players[i].gameObject.GetComponent<SKALPlayerInfomation>().nScore < Players[ii].gameObject.GetComponent<SKALPlayerInfomation>().nScore)
                {
                    Ranking[i]++;
                }
            }
        }

        for (int i = 0; i < nPlayersIndex; i++)
        {
            CharaterManager.instance.PlayerScore[i] += Ranking[i];
            print(i.ToString() + " : " + CharaterManager.instance.PlayerScore[i]);
        }

        CharaterManager.instance.RefreashRank();
        if (CharaterManager.instance.IsReMatch == false)
        {
            CharaterManager.instance.NextSceneName = "OTO";
            CharaterManager.instance.ChangeScene("GameExplaneScene");
        }
        else
        {
            if (CharaterManager.instance.IsJoonBok == true)
            {
                CharaterManager.instance.ReMatchEvent();
            }
            else
            {
                CharaterManager.instance.ChangeScene("EndingScene");
            }

        }
    }

}
