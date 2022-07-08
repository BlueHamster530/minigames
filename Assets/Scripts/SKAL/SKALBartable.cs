using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKALBartable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           other.transform.GetComponent<SKALPlayerInfomation>().IsNearInBarTable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponent<SKALPlayerInfomation>().IsNearInBarTable = false;
        }
    }

}
