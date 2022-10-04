using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKALPlayerCam : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform Target;
    [SerializeField]
    Vector3 Position = new Vector3(0,10,5);

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Target.position + Position;
        transform.LookAt(Target);

    }
}
