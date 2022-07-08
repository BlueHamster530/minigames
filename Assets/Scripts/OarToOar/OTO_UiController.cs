using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OTO_UiController : MonoBehaviour
{
    // Start is called before the first frame update
    OTO_PlayerInformation PlayerInfo;
    [SerializeField]
    TextMeshProUGUI ScoreText;
    public void Init(OTO_PlayerInformation _playerinfo)
    {
        PlayerInfo = _playerinfo;
    }
    public void SetScoreText(int _value)
    {
        ScoreText.text = _value.ToString();
    }
}
