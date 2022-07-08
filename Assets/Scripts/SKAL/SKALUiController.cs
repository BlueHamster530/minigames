using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SKALUiController : MonoBehaviour
{
    [SerializeField]
    Slider DrinkSlider;
    [SerializeField]
    Slider intoxicationStackSlider;
    [SerializeField]
    TextMeshProUGUI ScoreText;
    [SerializeField]
    TextMeshProUGUI intoxicationStackText;

    SKALPlayerInfomation PlayerInfo;
    public void Init(SKALPlayerInfomation _playerinfo)
    {
        PlayerInfo = _playerinfo;
    }
    public void SetScoreText(int _value)
    {
        ScoreText.text = _value.ToString();
    }
    private void Update()
    {
        ScoreText.text = PlayerInfo.nScore.ToString();
        DrinkSlider.value = (float)PlayerInfo.GetnBottleCount() /((float)PlayerInfo.GetnintoxicationStack()+3.0f);
        intoxicationStackSlider.value = ((float)PlayerInfo.GetnintoxicationStack()) / 5.0f;
        intoxicationStackText.text = PlayerInfo.GetnintoxicationStack().ToString()+"/5";

    }


}
