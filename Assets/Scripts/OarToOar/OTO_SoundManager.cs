using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTO_SoundManager : MonoBehaviour
{
    static public OTO_SoundManager instance;
    [SerializeField]
    AudioClip Dive;
    [SerializeField]
    AudioClip Jump1;
    [SerializeField]
    AudioClip Jump2;
    [SerializeField]
    AudioClip Punch;

    [SerializeField]
    GameObject SoundPRefab;
    private void Start()
    {
        instance = this;
    }
    public void CallDive()
    {
        GameObject clone = Instantiate(SoundPRefab, Vector3.zero, Quaternion.identity);
        clone.GetComponent<EffectSound>().init(Dive);
    }
    public void CallJump()
    {
        int rand = Random.Range(0, 2);
        GameObject clone = Instantiate(SoundPRefab, Vector3.zero, Quaternion.identity);
        if (rand == 0)
        {
            clone.GetComponent<EffectSound>().init(Jump1);
        }
        else
        {
            clone.GetComponent<EffectSound>().init(Jump2);
        }
    }
    public void CallPunch()
    {
        GameObject clone = Instantiate(SoundPRefab, Vector3.zero, Quaternion.identity);
        clone.GetComponent<EffectSound>().init(Punch);
    }

}
