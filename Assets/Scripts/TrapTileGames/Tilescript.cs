using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilescript : MonoBehaviour
{
    Vector3 originPos;
    bool IsFakeTile;
    bool IsTriggerOn;

    float MaxFallSpeed = 50;
    float CurrentFallSpeed;
    MeshRenderer meshrender;

    // Start is called before the first frame update
    void Start()
    {
      //  Init();
    }
    public void Init()
    {
        if(meshrender == null)
            meshrender = GetComponent<MeshRenderer>();
        originPos = this.transform.position;
        IsFakeTile = true;
        IsTriggerOn = false;
        meshrender.material.color = Color.white;
        CurrentFallSpeed = 0;
    }
    public void SetFakeTile(bool value)
    {
        IsFakeTile = value;
        if (IsFakeTile == false)
        {
            meshrender.material.color = Color.red;
            transform.gameObject.layer = 6;
        }
    }
    public bool GetFakeTile()
    {
        return IsFakeTile;
    }
    private void Update()
    {
        if (IsTriggerOn)
        {

            if(CurrentFallSpeed <=MaxFallSpeed)
                CurrentFallSpeed += Time.deltaTime * 5.0f;

            this.transform.position -= new Vector3(0, 1, 0) * CurrentFallSpeed;

            //if (transform.position.y <= -15.0f)
            //{
            //    transform.position = originPos;
            //    Init();
            //}
        }
    }
    private void initinvoke()
    {
        transform.position = originPos;
        Init();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (IsFakeTile == true)
            {
                if (IsTriggerOn == false)
                {
                    CurrentFallSpeed = 0;
                    IsTriggerOn = true;
                    Invoke("initinvoke", 1.25f);
                }
            }
        }
    }

}
