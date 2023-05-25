using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;

    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", Color.green);
                break;
            case 2:
                block.SetColor("_BaseColor", Color.yellow);
                break;
            case 5:
                block.SetColor("_BaseColor", Color.blue);
                break;
            default:
                block.SetColor("_BaseColor", Color.red);
                break;
        }
        renderer.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("beforeinvoke");
        onDestroyed.Invoke(PointValue);  // 이벤트리스너에 저장된 메서드에 인자를 전달하는 것 같네
        //Debug.Log("afterinvoke");
        
        //slight delay to be sure the ball have time to bounce
        Destroy(gameObject, 0.2f);
    }
}
