using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void Start()
    {
        fCurrentTime = fGamePlayTime;
        IsGamePause = true;
        bIsRaglanok = false;
        RaglanokImage.SetActive(false);
        StartCoroutine("GameStartCount");
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
    private void UpdateGUI()
    {
        TimeText.text = Mathf.FloorToInt(fCurrentTime / 60.0f).ToString() + ":" + Mathf.FloorToInt(fCurrentTime % 60.0f).ToString();

    }
    // Update is called once per frame
    void Update()
    {
        if (IsGamePause == false)
        {
            UpdateGUI();
            fCurrentTime -= Time.deltaTime;
            if (fCurrentTime <= 0)
            {//????????
                fCurrentTime = 0;
            }
            if (fCurrentTime <= 60 && bIsRaglanok == false)
            {
                bIsRaglanok = true;
                RaglanokImage.SetActive(true);
            }
        }
    }
}
