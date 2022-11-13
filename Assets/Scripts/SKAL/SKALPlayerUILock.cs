using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKALPlayerUILock : MonoBehaviour
{
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(target.position.x, target.position.y + 5, target.position.z+1);
    }
}
