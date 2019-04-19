using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SManu : MonoBehaviour
{
    private bool active = true;
    private float speed = 2f;
    public float finishInt = 160f;
    private bool actTween = false;
    private RectTransform rec;
    private float number = 0f;
    private float sahAnim = 0f;

    private float wScan = 0f;
    private float hScan = 0f;
    private float sScan = 0f;

    private float hsSMy = 1f;
    private float hSMy = 200f;

    // Start is called before the first frame update
    void Start()   {
        rec = GetComponent<RectTransform>();
        setActiv(false,0f);
       
        hsSMy = transform.localScale.x;
       
        print("++++++++++");
        // Invoke("spawn", 1f);//setTimeout
    }
    /*void spawn(){        
         print("WaitAndPrint " + Time.time);
         setActiv(true, 2.5f);

    }*/

    void Update() {       
        if (actTween == true){           
            if (active == true)
            {
                sahAnim += speed;                
                transform.localPosition = new Vector3(0f, (1-sahAnim) * finishInt, 0f);                
                number = Mathf.Round(sahAnim * 1000f) / 1000f;               
                if (number == 1.0f){
                    actTween = false;
                }
            }
            else
            {
                sahAnim -= speed;
                transform.localPosition = new Vector3(0f, (1-sahAnim) * finishInt, 0f);
                number = Mathf.Round(sahAnim * 1000f) / 1000f;                
                if (number == 0.0f){
                    actTween = false;
                }
              
            }           
        }
    }

    public void setActiv(bool b, float s) {
        speed = s;       
        if (active != b) {
            if (speed == 0){//нахрен твины 
                if (active == true){
                    sahAnim = 0f;
                    transform.localPosition = new Vector3(0f, finishInt, 0f);
                } else{
                    sahAnim = 1f;
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            } else {
                actTween = true;
            }
            active = b;
        }
    }


    public void sizeWindow(float _wScan, float _hScan, float _sScan)
    {
        wScan = _wScan;
        hScan = _hScan;
        sScan = _sScan;        
        
        transform.localScale = new Vector3(hsSMy* _sScan, hsSMy* _sScan, 1f);
       
    }


}
