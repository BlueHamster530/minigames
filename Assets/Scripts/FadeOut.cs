using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    Image image;

    float _time;
    private void Start()
    {
        image = GetComponent<Image>();
        _time = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime * 0.03f;
        image.color = Color.Lerp(image.color, new Color(0, 0, 0, 0), _time);
        if (_time <= 0.0f)
        {
            gameObject.SetActive(false);
        }
        
    }
}
