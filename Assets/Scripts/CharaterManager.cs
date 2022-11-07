using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharaterManager : MonoBehaviour
{

    static public CharaterManager instance;
    public int[] PlayerCharacterIndex = new int[4];
    public int MaxPlayerIndex;
    public string NextSceneName;
    public int[] PlayerScore = new int[4];
    public int[] Ranking = { 0, 0, 0, 0 };


    public bool IsReMatch;
    public bool IsJoonBok;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ChangeScene(string _Name)
    {
        SceneManager.LoadScene(_Name);
    }
    public void RefreashRank()
    {
        for (int i = 0; i < MaxPlayerIndex; i++)
        {
            Ranking[i] = 1;
            for (int ii = 0; ii < MaxPlayerIndex; ii++)
            {
                if (PlayerScore[i] < PlayerScore[ii])
                {
                    Ranking[i]++;
                }
            }
        }
        IsJoonBok = false;
        for (int i = 0; i < MaxPlayerIndex; i++)
        {
            for (int ii = 0; ii < MaxPlayerIndex; ii++)
            {
                if (i == ii) continue;
                if (Ranking[i] == Ranking[ii])
                    IsJoonBok = true;
            }
        }
    }
    public void ReMatchEvent()
    {
        int gametype = Mathf.Clamp(Random.Range(0, 2),0,1);
        if (gametype == 0)
        {
            NextSceneName = "SKAL";
        }
        else
        {
            NextSceneName = "OTO";
        }
        ChangeScene("GameExplaneScene");
    }
}
