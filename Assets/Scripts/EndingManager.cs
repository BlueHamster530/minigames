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
    GameObject[] playerNAmeObject;


    float returntime;
    // Start is called before the first frame update
    void Start()
    {
        int[] Ranking = { 1, 1, 1, 1 };
        for (int i = 0; i < CharaterManager.instance.MaxPlayerIndex; i++)
        {
            Ranking[i] = 1;
            for (int ii = 0; ii < CharaterManager.instance.MaxPlayerIndex; ii++)
            {
                if (CharaterManager.instance.PlayerScore[i] > CharaterManager.instance.PlayerScore[ii])
                {
                    Ranking[i]++;
                }
            }
        }
        returntime =10.0f;



        for (int i = 0; i < CharaterManager.instance.MaxPlayerIndex; i++)
        {

            int index = CharaterManager.instance.PlayerCharacterIndex[i];
            playerranks[i].gameObject.SetActive(true);
            playerNAmeObject[i].SetActive(true);
            playerranks[i].sprite = rankimage[Ranking[i]-1];
        }
    }

    private void FixedUpdate()
    {
        returntime -= Time.deltaTime;
        if (returntime <= 0)
        {
            CharaterManager.instance.GetComponent<InputSystemgamemanagertest>().DeleatAllDevide();
            CharaterManager.instance.ChangeScene("MainScene");
        }
    }
}
