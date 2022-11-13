using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameexplaneinfomation : MonoBehaviour
{

    public bool IsReady;
    [SerializeField]
    GameExplaneScript gameExplane;
    Playerinput pinput;
    int index;
    bool keydowncheck;
    float keydowntick;

    private void OnEnable()
    {
        IsReady = false;
        gameExplane = GameObject.Find("Main Camera").GetComponent<GameExplaneScript>();
        pinput = GetComponent<Playerinput>();
        index = pinput.GetPlayerInDex();
        gameExplane.IsReady[index - 1] = IsReady;
        gameExplane.playerReadText[index - 1].text = "Press A To Ready";
        gameExplane.playerReadText[index - 1].gameObject.SetActive(true);
        keydowncheck = false;
        keydowntick = 0.2f;

    }
    public void PressAButton()
    {
        if (IsReady == false)
        {
            IsReady = true;
            gameExplane.IsReady[index - 1] = IsReady;
            gameExplane.playerReadText[index - 1].text = "Ready";
        }
        else
        {
            IsReady = false;
            gameExplane.IsReady[index - 1] = IsReady;
            gameExplane.playerReadText[index - 1].text = "Press A To Ready";
        }
    }
    public void PressBButton()
    {
        if (IsReady == true)
        {
            IsReady = false;
            gameExplane.IsReady[index - 1] = IsReady;
            gameExplane.playerReadText[index - 1].text = "Press A To Ready";
        }
    }
}
