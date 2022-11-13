using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKALSoundManager : MonoBehaviour
{
    static public SKALSoundManager instance;
    [SerializeField]
    AudioClip Refill;
    [SerializeField]
    AudioClip Crash;
    [SerializeField]
    AudioClip Drinking;
    [SerializeField]
    AudioClip Drunken;

    [SerializeField]
    GameObject SoundPRefab;
    private void Awake()
    {
        instance = this;
    }
    public void CallRefill()
    {
        GameObject clone = Instantiate(SoundPRefab, Vector3.zero, Quaternion.identity);
        clone.GetComponent<EffectSound>().init(Refill);
    }
    public void CallCrash()
    {
        GameObject clone = Instantiate(SoundPRefab, Vector3.zero, Quaternion.identity);
        clone.GetComponent<EffectSound>().init(Crash);
    }
    public void CallDrinking()
    {
        GameObject clone = Instantiate(SoundPRefab, Vector3.zero, Quaternion.identity);
        clone.GetComponent<EffectSound>().init(Drinking);
    }
    public void CallDrunken()
    {
        GameObject clone = Instantiate(SoundPRefab, Vector3.zero, Quaternion.identity);
        clone.GetComponent<EffectSound>().init(Drunken);
    }

}
