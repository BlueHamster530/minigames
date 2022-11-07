using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cSpriteSliderTest : MonoBehaviour
{
    SpriteRenderer renderer;
    float asd;
    // Start is called before the first frame update
    void Start()
    {
        asd = 0;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        asd += Time.deltaTime * 0.1f;
        renderer.size = new Vector3(asd, 0.77f);
    }
}
