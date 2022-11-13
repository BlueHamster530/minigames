using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobiInfomation : MonoBehaviour
{
    public bool IsReady;
    [SerializeField]
    LobiManager lobimanager;
    Playerinput pinput;
    int index;
    bool keydowncheck;
    float keydowntick;
    bool CharSetting;
    private void OnEnable()
    {
        CharSetting = false;
        IsReady = false;
        lobimanager = GameObject.Find("Main Camera").GetComponent<LobiManager>();
        pinput = GetComponent<Playerinput>();
        index = pinput.GetPlayerInDex();
        lobimanager.IsReady[index - 1] = IsReady;
        lobimanager.readytext[index - 1].text = "Press A To Ready";
        lobimanager.readytext[index - 1].gameObject.SetActive(true);
        keydowncheck = false;
        keydowntick = 0.2f;
        CharaterManager.instance.PlayerCharacterIndex[index - 1] = index - 1;
        ChacterChange(index-1);
    }
    // Start is called before the first frame update

    private void ChangeEvent()
    {
        if (CharSetting == false)
        {
            ChacterChange(index - 1);
            CharSetting = true;
        }
        if (IsReady == false)
        {
            if (keydowncheck == false)
            {
                if (pinput.Dir.x >= 0.8f)
                {
                    keydowncheck = true;
                    keydowntick = 0.2f;
                    ChacterChange(CharaterManager.instance.PlayerCharacterIndex[index - 1] + 1);
                }
                else if (pinput.Dir.x <= -0.8f)
                {
                    keydowncheck = true;
                    keydowntick = 0.2f;
                    ChacterChange(CharaterManager.instance.PlayerCharacterIndex[index- 1] - 1);
                }
            }
            else
            {
                keydowntick -= Time.deltaTime;
                if (keydowntick <= 0.0f)
                {
                    keydowncheck = false;
                    keydowntick = 0.2f;
                }
            }
        }
    }
    private void ChacterChange(int value)
    {
        for (int ii = 0; ii < 6; ii++)
        {
            transform.GetChild(0).transform.GetChild(ii).gameObject.SetActive(false);
        }
        CharaterManager.instance.PlayerCharacterIndex[index-1] = value;
        if (CharaterManager.instance.PlayerCharacterIndex[index-1] >= 6)
        {
            CharaterManager.instance.PlayerCharacterIndex[index-1] = 0;
        }
        else if (CharaterManager.instance.PlayerCharacterIndex[index-1] <= -1)
        {
            CharaterManager.instance.PlayerCharacterIndex[index - 1] = 5;
        }
        transform.GetChild(0).transform.GetChild(CharaterManager.instance.PlayerCharacterIndex[index-1]).gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        ChangeEvent();
    }
    public void PressAButton()
    {
        if (IsReady == false)
        {
            IsReady = true;
            lobimanager.IsReady[index - 1] = IsReady;
            lobimanager.readytext[index - 1].text = "Ready";
        }
       else
        {
            IsReady = false;
            lobimanager.IsReady[index - 1] = IsReady;
            lobimanager.readytext[index - 1].text = "Press A To Ready";
        }
    }
    public void PressBButton()
    {
        if (IsReady == true)
        {
            IsReady = false;
            lobimanager.IsReady[index - 1] = IsReady;
            lobimanager.readytext[index - 1].text = "Press A To Ready";
        }
        else
        {
            lobimanager.readytext[index - 1].gameObject.SetActive(false);
        }
    }
    public void TextOff()
    {
        lobimanager.readytext[index - 1].gameObject.SetActive(false);
    }
}
