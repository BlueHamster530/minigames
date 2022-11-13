using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameExplaneScript : MonoBehaviour
{

    [SerializeField]
    public TextMeshProUGUI[] playerReadText;

    [SerializeField]
    Image ExplaneImage;

    [SerializeField]
    Sprite[] gameepxlanelist;

    [SerializeField]
    GameObject CountDownImage;

    [SerializeField]
    Sprite[] countdownnumber;


    public bool[] IsReady = new bool[4];

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

        for (int i = 0; i < CharaterManager.instance.MaxPlayerIndex; i ++)
        {
                playerReadText[i].transform.parent.gameObject.SetActive(true);
               playerReadText[i].text = "Readt To Press A";
        }

        fGameStartTime = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GameStartCheck();
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
