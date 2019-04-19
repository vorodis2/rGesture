using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Glaf : MonoBehaviour{
    public GameObject m0;//главная сцена
    
    public GameObject mg;//сама игра
    public List<GameObject> arrayPrefabs;//сама игра
    public Text text1;//время
    public Text text2;//шаг

    private static LogikMenu logikMenu;//рулим меню
    public Game game;//сама игра

    private float wScan = 0f;
    private float hScan = 0f;
    private float sScan = 0f;

    private List<float> xx;


    void Start()
    {
        logikMenu = new LogikMenu(this);
        logikMenu.gameStart();
        game = new Game(this);

        List<float> numbers1 = new List<float>() { 0.0f, 0.00f,   1.0f, -0.5f,   1.0f, 1.5f,0.0f, 0.00f};
        game.addArrObj(numbers1);

        List<float> numbers = new List<float>() { 0.05f,0.05f, 0.95f,0.05f, 0.95f,0.95f, 0.05f,0.95f, 0.05f,0.05f};
        game.addArrObj(numbers);
        
    }

    void Update(){
        if (wScan!= Screen.width|| hScan != Screen.height){
            wScan = Screen.width;
            hScan = Screen.height;
            sScan = 1f;

            float ns = sScan;
            ns = hScan / wScan;        
            if (ns<1.66f){
                sScan = 1.0f * (ns / 1.66f);
            }
            logikMenu.sizeWindow(wScan, hScan, sScan);
            game.sizeWindow(wScan, hScan, sScan);
        }
        game.upDate();
    }  

    public static void setEventMy(GameObject gameObject, string str, float num){
        logikMenu.setEventMy(gameObject, str, num);       
    }
}



public class Game : MonoBehaviour
{
    private Glaf glaf;
    private List<List<float>> arrObj=new List<List<float>>();
    private GameObject content;
    public BlokDraw blokDraw;
    private DrawMouse drawMouse; 
    public Plume plume;
    private float secondTime =0f;// += Time.deltaTime;
    

    private bool activ=false;
    private int sah=0;
    private float timeProsent=1f;   
    private float time=50f;
    private float pT=50f;
    private float pT1=50f;
    private float pT2=50f;
    private float wh = 0.45f;


    public Game(Glaf g){
        glaf = g; 
        content = glaf.mg.transform.GetChild(0).gameObject; 
        content.transform.localScale = new Vector3(1f, 1f, 1f); 

        
        blokDraw = new BlokDraw(content);
        blokDraw.setPref(glaf.arrayPrefabs[0], glaf.arrayPrefabs[1]);

        Vector3 pos = new Vector3(1.0f, 3.5f, 0f); 
        blokDraw.content.transform.localPosition = pos;
        blokDraw.content.transform.localScale = new Vector3(1f, 1f, 1f); 
        plume=new Plume(content, glaf.arrayPrefabs[0]);
        drawMouse=new DrawMouse(this, content, glaf.arrayPrefabs[2], glaf.arrayPrefabs[3]);       

    }

    public void addArrObj(List<float> arr){
        arrObj.Add(arr);        
    }


    public void start(){
        activ=true;
        sah=0;
        timeProsent=1.05f;

        dragGame();
    }

    public void dragGame(){ 
        pT=secondTime;
        timeProsent-=0.05f;
        blokDraw.drawArray(arrObj[sah%arrObj.Count]);
        Glaf.setEventMy(null, "step", sah);          
        sah++;        
    }

    public void upDate(){
        secondTime += Time.deltaTime; 
        
        if(activ==true){           
            plume.upDate();
            drawMouse.upDate();
            pT1=secondTime-pT;
            pT2=Mathf.Round(time*timeProsent-pT1);
            Glaf.setEventMy(null, "time", pT2);          
            if(pT2<0f){
                activ=false;
                Glaf.setEventMy(null, "menu", pT2);
            }
        }        
    } 

    public void sizeWindow(float _wScan, float _hScan, float _sScan){
        plume.sizeWindow(_wScan,  _hScan, _sScan);
        drawMouse.sizeWindow(_wScan,  _hScan, _sScan);
        content.transform.localScale = new Vector3(8f, 1f, 1f); 
        content.transform.localScale = new Vector3(1f, 1f, 1f); 

    }  
}


public class DrawMouse : MonoBehaviour{
    private GameObject content=new GameObject();
    private GameObject gameObject;
    private GameObject gameObject1;
    private BlokDraw blokDraw;
    private float wScan=100f;
    private float hScan=100f;
    private float sScan=1f;
    private bool b=false;
    private bool boolMous=false;
    private float xx;
    private float yy;
    private List<float> numbers;
    private Vector2 vect = new Vector3(0f, 0f); 
    private Vector2 vect1 = new Vector3(0f, 0f); 
    private float sah; 
    private float dist;    
    private float dist1; 
    private Game game;
    public DrawMouse(Game _game, GameObject _content, GameObject _gameObject, GameObject _gameObject1){        
        game=_game;
        content.transform.parent=_content.transform;
        content.transform.localPosition = new Vector3(0f, 0f, 0f);
        content.transform.localScale = new Vector3(1f, 1f, 1f); 
        gameObject=_gameObject;
        gameObject1=_gameObject1;

        blokDraw = new BlokDraw(content);
        blokDraw.content.transform.localPosition = new Vector3(0f, 0f, 0f);
        blokDraw.content.transform.localScale = new Vector3(1f, 1f, 1f); 

        blokDraw.setPref(_gameObject, _gameObject1);
    }

    public void reStart(){
        b=true;
        //FIXE хер его знает как маштобируеться и где затуп))) подогнал тыком
        blokDraw.clear();
        blokDraw.content.transform.localPosition = new Vector3(0f, 0f, 0f);
        xx=((Input.mousePosition.x-wScan/2f)/wScan*2f*wScan/65f);
        yy=((Input.mousePosition.y-hScan/2f)/hScan*2f*hScan/65f);
        vect.x=xx;
        vect.y=yy;
        vect1.x=xx;
        vect1.y=yy;
        sah=0;
        numbers = new List<float>();
        numbers.Add(xx);
        numbers.Add(yy);
        //blokDraw.drawArray(numbers);
        
    }
    public void reStop(){
        b=false;
        if(numbers.Count>1){  
            numbers.Add(blokDraw.arr[0]);
            numbers.Add(blokDraw.arr[1]);         
            blokDraw.setMinMax(game.blokDraw.min, game.blokDraw.max);
            blokDraw.content.transform.localPosition = new Vector3(1f, 3.5f, 0f);

            bool b=true;
            bool b1=true;
            for (var i = 0; i < game.blokDraw.arr.Count-1; i+=2){
                b1=false;
                dist1=999f;
                for (var j = 0; j < blokDraw.arr.Count-1; j+=2){
                    dist=Mathf.Sqrt(Mathf.Pow((blokDraw.arr[j] - game.blokDraw.arr[i]), 2) + Mathf.Pow((blokDraw.arr[j+1] - game.blokDraw.arr[i+1]), 2));
                    print(dist);
                    if(dist1>dist)dist1=dist;
                    if(dist<0.2f){
                        b1=true;
                    }
                }
                
                if(b1==false)b=false;
            }

            if(b==true){
                game.dragGame();
                blokDraw.clear();
                game.plume.ok();
                

            }

        }
        //setMinMax
    }


    public void upDate(){
        if (Input.GetButtonDown("Fire1")) {
            if(b==false){
                boolMous = true;
                reStart();
            }
        }

        if (Input.GetButtonUp("Fire1")) {
            boolMous = false;
            if(b==true){
                reStop();
            }
        }

        if(b==true){           
            xx=((Input.mousePosition.x-wScan/2f)/wScan*2f*wScan/65f);
            yy=((Input.mousePosition.y-hScan/2f)/hScan*2f*hScan/65f);

            dist = Mathf.Sqrt(Mathf.Pow((vect.x - xx), 2) + Mathf.Pow((vect.y - yy), 2));
            if(dist>0.2){
                dist1 = Mathf.Sqrt(Mathf.Pow((vect1.x - xx), 2) + Mathf.Pow((vect1.y - yy), 2));
                
                if(dist1<0.2f){
                    if(sah==20){
                        numbers.Add(xx*1f);
                        numbers.Add(yy*1f);
                        
                        blokDraw.drawArray(numbers);
                        
                        vect1.x = xx;
                        vect1.y = yy;
                        vect.x=xx;
                        vect.y=yy;
                        sah=0;                            
                    }
                    sah++;
                }else{
                   vect1.x= xx;
                   vect1.y= yy;
                   sah=0;
                }
            }
        }

    }

    public void sizeWindow(float _wScan, float _hScan, float _sScan){
        wScan=_wScan;
        hScan=_hScan;
        sScan=_sScan;
    } 
}


///Рисуем спички от масива
public class Plume : MonoBehaviour{
    private GameObject content=new GameObject();
    private GameObject gameObject;
    private bool boolMous=false;
    private List<BoxPlume> array = new List<BoxPlume>();
    private BoxPlume boxPlume;

    private float sah=0f;
    private int p=-1;
    private Transform cam;
    private float wScan=100f;
    private float hScan=100f;
    private float sScan=1f;
    public Plume(GameObject _content, GameObject _gameObject){        
        
        content.transform.parent=_content.transform;
        content.transform.localPosition = new Vector3(0f, 0f, 0f);
        content.transform.localScale = new Vector3(1f, 1f, 1f); 
        gameObject=_gameObject;
        
    }



    private void craet(float _x, float _y){
        boxPlume=null;
        p=-1;        
        for (var i = 0; i < array.Count; i++){            
            if(array[i].activ==false){
                p = i;                
                i=9999;                
            }
        }
        if(p==-1){
            array.Add(new BoxPlume(content, gameObject));  
            p=array.Count-1;

        }

                array[p].start(_x, _y);
    } 



    public void ok(){
        for (var i = 0; i < 300; i++){
            craet(Random.Range(-5f, 5f),Random.Range(-5f, 5f)); 
        }
    }

    public void upDate(){
        if (Input.GetButtonDown("Fire1")) {
            boolMous = true;
        }
        if (Input.GetButtonUp("Fire1")) {
            boolMous = false;
        }

        
        for (var i = 0; i < array.Count; i++){
            array[i].upDate();
        }

        if(boolMous==true){
            //FIXE хер его знает как маштобируеться и где затуп))) подогнал тыком
            float xx=((Input.mousePosition.x-wScan/2f)/wScan*2f*wScan/65f);
            float yy=((Input.mousePosition.y-hScan/2f)/hScan*2f*hScan/65f);

            craet(xx,yy);                
            craet(xx,yy);      
            craet(xx,yy);
        }        
    } 

    public void sizeWindow(float _wScan, float _hScan, float _sScan){
        wScan=_wScan;
        hScan=_hScan;
        sScan=_sScan;
    } 
}



///Рисуем спички от масива
public class BoxPlume : MonoBehaviour{
    private GameObject content=new GameObject();
    private GameObject gameObject;
    public bool activ=false;   
    private Vector3 dVect = new Vector3(0f, 0f, 0f); 
    private Vector2 sVect = new Vector3(0f, 0f); 

    private GameObject wreckClone; 
    private Vector3 pos = new Vector3(0.0f, 0f, 0f); 
    private Vector3 scale = new Vector3(1f, 1f, 1f); 
    private float tim = 0f;
    private float sss = 0f; 



    public BoxPlume(GameObject _content, GameObject _gameObject){        
        content.transform.parent=_content.transform;
        content.transform.localPosition = new Vector3(0f, 0f, 0f);

        gameObject = _gameObject;
    }


    public void start(float _x, float _y){
        activ=true;
        sVect.x=_x;
        sVect.y=_y;

        content.transform.localPosition = new Vector3(_x, _y, 0f);

        dVect.x=Random.Range(-0.1f, 0.1f);
        dVect.y=Random.Range(-.1f, .1f);
        dVect.z=Random.Range(0.02f, .1f);

        pos.x=0f;
        pos.y=0f;
        pos.z=0f;
        tim=1f;

        sss=Random.Range(0.1f, .3f);
        scale.x=sss;
        scale.y=sss;     
                            
        wreckClone = (GameObject)Instantiate(gameObject, pos, Quaternion.identity);            
        wreckClone.transform.parent = content.transform;
        wreckClone.transform.localPosition = pos;
        wreckClone.transform.localScale = scale;
        
    } 



    public void upDate(){        
        if(activ==true){
            tim-=0.01f;
            if(tim<0f){
                activ = false;
                Destroy(wreckClone);              
            }else{                
                pos.x+=dVect.x*dVect.z*tim;
                pos.y+=dVect.y*dVect.z*tim; 
                wreckClone.transform.localPosition = pos;

                scale.x=sss*tim;
                scale.y=sss*tim;       
                wreckClone.transform.localScale = scale;                           
            }
        }
    } 
}


///Рисуем спички от масива
public class BlokDraw : MonoBehaviour{
    private GameObject gameObject;
    private GameObject gameObject1;
    public GameObject content = new GameObject();
    public List<float> arr;
    private List<GameObject> arrayOld; 

    public Vector2 min = new Vector2(0.0f, 0f); 
    public Vector2 max = new Vector2(0.0f, 0f);    

    public BlokDraw(GameObject _content){  
        content.transform.parent=_content.transform;
        content.transform.localPosition = new Vector3(0f, 0f, 0f);  
    }

    public void setPref(GameObject _g, GameObject _g1){
        gameObject = _g;
        gameObject1 = _g1;        
    }


    public void clear(){
        

        if (arrayOld != null){
            for (var i = 0; i < arrayOld.Count; i++)  {
                Destroy(arrayOld[i]);
            }
        }
        
    }


    public void drawArray(List<float> _arr){
        clear();
        arr=_arr;
        arrayOld = new List<GameObject>();
        min.x=9999;
        min.y=9999;
        max.x=-9999;
        max.y=-9999;
        for (var i = 0; i < _arr.Count-2; i+=2){
            dLine(_arr[i], _arr[i+1], _arr[i+2], _arr[i+3]);
        }       
    }


    private float xx,yy,ssx,ssy;
    public void setMinMax(Vector2 _min, Vector2 _max){
        ssx=(_max.x-_min.x)/(max.x-min.x);
        ssy=(_max.y-_min.y)/(max.y-min.y);
        for (var i = 0; i < arr.Count; i+=2){
            arr[i]*=ssx;
            arr[i]+=(_min.x-min.x*ssx);            
            arr[i+1]*=ssy;
            arr[i+1]+=(_min.y-min.y*ssy);
        }
        drawArray( arr);
    } 


    private float rotation;
    private float dist;
    private float scale;
    private void dLine(float x, float y, float x1, float y1){
        if(min.x>x)min.x=x;        
        if(min.y>y)min.y=y;
        if(min.x>x1)min.x=x1;        
        if(min.y>y1)min.y=y1;

        if(max.x<x)max.x=x;
        if(max.y<y)max.y=y;
        if(max.x<x1)max.x=x1;
        if(max.y<y1)max.y=y1;

        rotation =Mathf.Atan2(y - y1, x - x1) * 180.0f / Mathf.PI -90f ;//90f - 20f;
        
        dist = Mathf.Sqrt(Mathf.Pow((x - x1), 2) + Mathf.Pow((y - y1), 2));

        Vector3 pos = new Vector3(x, y, 0f);
        GameObject wreckClone;            
        wreckClone = (GameObject)Instantiate(gameObject1, pos, Quaternion.identity);            
        wreckClone.transform.parent = content.transform;
        wreckClone.transform.localPosition = pos;
        arrayOld.Add(wreckClone);

        scale=dist/100;
       
        wreckClone.transform.localScale = new Vector3(1f, dist, 1f);
        wreckClone.transform.eulerAngles = new Vector3(0f, 0f, rotation);//немного магии на последок

        pos = new Vector3(x, y, 0f);                  
        wreckClone = (GameObject)Instantiate(gameObject, pos, Quaternion.identity);            
        wreckClone.transform.parent = content.transform;
        wreckClone.transform.localPosition = pos;
        arrayOld.Add(wreckClone);
    }
}


public class LogikMenu : MonoBehaviour
{
    public Glaf glaf;
    private float sahAnimt = 0.05f;

    private float wScan = 0f;
    private float hScan = 0f;
    private float sScan = 0f;
    private float ns = 0f;
    private float lavelSkil = 0f;


    public LogikMenu(Glaf g) {
        glaf = g;
    }

    public void gameStart() {
        glaf.m0.GetComponent<SManu>().setActiv(true, 0f);
        
    }

    public void setEventMy(GameObject gameObject, string str, float num)
    {       
        if (str == "playGameMy"){
            glaf.m0.GetComponent<SManu>().setActiv(false, sahAnimt);
            glaf.mg.GetComponent<SManu>().setActiv(true, sahAnimt);
            glaf.game.start();

        }
        if (str == "step"){ glaf.text2.text = num.ToString(); }
        if (str == "time"){glaf.text1.text = num.ToString(); }

        if (str == "menu"){
            glaf.m0.GetComponent<SManu>().setActiv(true, sahAnimt);
            glaf.mg.GetComponent<SManu>().setActiv(false, sahAnimt);

        }
    }

    public void sizeWindow(float _wScan, float _hScan, float _sScan){
        wScan = _wScan;
        hScan = _hScan;
        sScan = _sScan;
        glaf.m0.GetComponent<SManu>().sizeWindow(wScan, hScan, sScan);  
        glaf.mg.GetComponent<SManu>().sizeWindow(wScan, hScan, sScan);
    }

    /*
    Это моя вторая тестовая работа на юнити теперь вместе со второй у меня 48 чаасов скилов по unity + c#. До сих пор в афиге от этого юнити и его косяков))) 
    */
}



