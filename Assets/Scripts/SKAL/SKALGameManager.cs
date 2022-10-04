using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SKALGameManager : MonoBehaviour
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

    [SerializeField]
    Camera[] PlayerCams;

    [SerializeField]
    int nPlayersIndex=2;

    private void Start()
    {
        fCurrentTime = fGamePlayTime;
        IsGamePause = true;
        bIsRaglanok = false;
        RaglanokImage.SetActive(false);
        StartCoroutine("GameStartCount");

     
        for (int i = 0; i < nPlayersIndex; i++)
        {
            PlayerCams[i].rect = new Rect(new Vector2(i * (1.0f / (nPlayersIndex)), 0), new Vector2(1.0f / (nPlayersIndex), 1));
        }

    }
    IEnumerator GameStartCount()
    {
        for (int i = 3; i > 0; i--)
        {
            TimeText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        IsGamePause = false;
        yield return null;
    }
    private void UpDateGUI()
    {
        TimeText.text = Mathf.FloorToInt(fCurrentTime / 60.0f).ToString() + ":" + Mathf.FloorToInt(fCurrentTime % 60.0f).ToString();

    }
    private void Update()
    {
        if (IsGamePause == false)
        {
            UpDateGUI();
            fCurrentTime -= Time.deltaTime;
            if (fCurrentTime <= 0)
            {//��������
                fCurrentTime = 0;
            }
            if (fCurrentTime <= 30 &&bIsRaglanok == false)
            {
                bIsRaglanok = true;
                RaglanokImage.SetActive(true);
            }
        }
    }

}
