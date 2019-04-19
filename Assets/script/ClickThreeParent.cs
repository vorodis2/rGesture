using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickThreeParent : MonoBehaviour
{
    public string eventString = "xz#########";
    public float eventNum = 0f;

    private float scale = 1f;
    private float scale1 = 1.1f;

   
    void Start()
    {
        scale = transform.localScale.y;
        scale1 = transform.localScale.y*1.05f;     

    }


    void OnMouseDown(){
        transform.localScale = new Vector3(scale1, scale1, 1);
        //print(transform.parent.gameObject);//бля дебилы впихнули парент в трансформации)))          
        
    }

    void OnMouseUp(){
        transform.localScale = new Vector3(scale, scale, 1);
        Glaf.setEventMy(transform.parent.gameObject, eventString, eventNum);
    }




}