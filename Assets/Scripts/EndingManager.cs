using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndingManager : MonoBehaviour
{
    [SerializeField]
    Image[] playerranks;
    [SerializeField]
    Sprite[] rankimage;

    [SerializeField]
    GameObject[] playerobject;
    [SerializeField]
    GameObject[] playerNAmeObject;

    float returntime;
    // Start is called before the first frame update
    void Start()
    {
        returntime = 10.0f;
        for (int i = 0; i < CharaterManager.instance.MaxPlayerIndex; i++)
        {
            playerranks[i].gameObject.SetActive(true);
            playerobject[i].SetActive(true);
            playerNAmeObject[i].SetActive(true);
            playerranks[i].sprite = rankimage[CharaterManager.instance.Ranking[i]];
        }
    }

    private void FixedUpdate()
    {
        returntime -= Time.deltaTime;
        if (returntime <= 0)
        {
            CharaterManager.instance.ChangeScene("MainScene");
        }
    }
}
