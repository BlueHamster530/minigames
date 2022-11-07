using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSound : MonoBehaviour
{

    AudioSource source;
    public void init(AudioClip clip)
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
