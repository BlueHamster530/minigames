using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Object_Twinkle : MonoBehaviour
{
    MeshRenderer meshRender;
    int nCount;
    Vector3 OriginPos;
    bool IsPositionLock;
    Vector3 ReturnPosition;
    // Start is called before the first frame update
    public void Init(int _Count, bool _PositionLock, Vector3 _returnpos)
    {
        meshRender = GetComponentInChildren<MeshRenderer>();
        nCount = _Count;
        OriginPos = this.transform.position;
        IsPositionLock = _PositionLock;
        ReturnPosition = _returnpos;
        StartCoroutine("Twinkle");
    }
    private void Update()
    {
        if (IsPositionLock == true)
            transform.position = OriginPos;
    }
    private IEnumerator Twinkle()
    {
        while (nCount >=0)
        {
            if(nCount%2 ==1)
                meshRender.material.color = new Color(0, 0, 0, 0);
            else
                meshRender.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            nCount--;
            yield return new WaitForSeconds(0.2f);
        }
        IsPositionLock = false;
        meshRender.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        transform.position = ReturnPosition;
        Destroy(GetComponent<Effect_Object_Twinkle>());
        yield return 0;
    }

}
