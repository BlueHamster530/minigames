using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OTO_GameManger : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TimeText;

    float fCurrentTime;
    [SerializeField]
    float fGamePlayTime;

    [SerializeField]
    GameObject RaglanokImage;

    bool IsGamePause;
    public bool bIsRaglanok { get; set; } = false;
    // Start is called before the first frame update

    [SerializeField]
    Playerinput[] Players = new Playerinput[4];

    [SerializeField]
    int nPlayersIndex = 2;
    [SerializeField]
    Slider TimeBar;

    [SerializeField]
    Sprite[] countdowniamge;
    [SerializeField]
    Image countdown;

    void Start()
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
        for (int i = 0; i < nPlayersIndex; i++)
        {
            Players[i].IsCanMove = false;
        }
        for (int i = 2; i >= 0; i--)
        {
            countdown.sprite = countdowniamge[i];
            yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < nPlayersIndex; i++)
        {
            Players[i].IsCanMove = true;
            Players[i].GetComponent<OTO_PlayerInformation>().vOriginPos = Players[i].transform.position;
        }
        countdown.gameObject.SetActive(false);
        IsGamePause = false;
        yield return null;
    }
    private void UpdateGUI()
    {
        TimeText.text = Mathf.FloorToInt(fCurrentTime / 60.0f).ToString() + ":" + Mathf.FloorToInt(fCurrentTime % 60.0f).ToString("00");
        TimeBar.value = (fCurrentTime / fGamePlayTime);

    }
    // Update is called once per frame
    void Update()
    {
        if (IsGamePause == false)
        {
            UpdateGUI();
            fCurrentTime -= Time.deltaTime;
            if (fCurrentTime <= 0)
            {//게임종료
                fCurrentTime = 0;
                GameTimeOver();
            }
            if (fCurrentTime <= 60 && bIsRaglanok == false)
            {
                bIsRaglanok = true;
                RaglanokImage.SetActive(true);
                Invoke("DisableRagranokImage", 1.0f);
            }
        }
    }
    private void GameTimeOver()
    {
        int[] Ranking = { 1, 1, 1, 1 };
        for (int i = 0; i < nPlayersIndex; i++)
        {
            Ranking[i] = 1;
            for (int ii = 0; ii < nPlayersIndex; ii++)
            {
                if (Players[i].gameObject.GetComponent<OTO_PlayerInformation>().nScore < Players[ii].gameObject.GetComponent<OTO_PlayerInformation>().nScore)
                {
                    Ranking[i]++;
                }
            }
        }

        for (int i = 0; i < nPlayersIndex; i++)
        {
            CharaterManager.instance.PlayerScore[i] += Ranking[i];
            print(i.ToString() +" : " +CharaterManager.instance.PlayerScore[i]);
        }


        CharaterManager.instance.RefreashRank();
        if (CharaterManager.instance.IsReMatch == false)
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

    private void DisableRagranokImage()
    {
        RaglanokImage.SetActive(false);
    }
}
