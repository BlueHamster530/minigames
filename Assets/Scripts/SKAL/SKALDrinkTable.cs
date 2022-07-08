using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKALDrinkTable : MonoBehaviour
{
    SKALPlayerInfomation[] Playerinfo = new SKALPlayerInfomation[4];
    bool[] IsInSidePlayer = new bool[4];
    private void Start()
    {
        for(int i = 0; i < 4; i ++)
            IsInSidePlayer[i] = false;
    }
    public bool GenseiTable(int _index)
    {
        bool result = false;
        for (int i = 0; i < 4; i++)
        {
            if (i == _index) continue;

            if (IsInSidePlayer[i] == true&& Playerinfo[i] != null)
            {
                Playerinfo[i].AttckDoingDrink();
                result = true;
            }
        }
        return result;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Playerinfo[other.transform.GetComponent<SKALPlayerInfomation>().PlayerIndex] == null)
            {
                Playerinfo[other.transform.GetComponent<SKALPlayerInfomation>().PlayerIndex] = other.transform.GetComponent<SKALPlayerInfomation>();
            }
            other.transform.GetComponent<SKALPlayerInfomation>().IsNearInTable = true;
            other.transform.GetComponent<SKALPlayerInfomation>().SetDrinkTable(this);
            IsInSidePlayer[other.transform.GetComponent<SKALPlayerInfomation>().PlayerIndex] = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponent<SKALPlayerInfomation>().IsNearInTable = false;
            IsInSidePlayer[other.transform.GetComponent<SKALPlayerInfomation>().PlayerIndex] = false;          
            other.transform.GetComponent<SKALPlayerInfomation>().SetDrinkTable(null);

            if (Playerinfo[other.transform.GetComponent<SKALPlayerInfomation>().PlayerIndex] != null)
            {
                Playerinfo[other.transform.GetComponent<SKALPlayerInfomation>().PlayerIndex] = null;
            }
        }
    }

}
