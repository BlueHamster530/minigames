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
    Playerinput[] Players;

    private void Start()
    {
        fCurrentTime = fGamePlayTime;
        IsGamePause = true;
        bIsRaglanok = false;
        RaglanokImage.SetActive(false);
        StartCoroutine("GameStartCount");

    }
    IEnumerator GameStartCount()
    {

        for (int i = 0; i < nPlayersIndex; i++)
        {
            Players[i].IsCanMove = false;
        }
        for (int i = 3; i > 0; i--)
        {
            TimeText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
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
        TimeBar.value = (fCurrentTime / 180.0f);
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
}
