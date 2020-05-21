using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    private GameObject Cube;
    // Start is called before the first frame update
    private Vector3 cubeVector3;
    public Vector3 CubeVector3
    {
        set { CubeVector3 = cubeVector3; }
        get { return cubeVector3; }
    }

    private CreatePlane AboutCreatPlane;
    private GameObject _whiteGame;
    private GameObject _blackGame;

    private GameObject _blackGame0;
    private GameObject _blackGame1;
    private GameObject _whiteGame0;
    private GameObject _whiteGame1;

    private Ray _ray_0;
    private Ray _ray_1;
    private Ray _ray_2;
    private Ray _ray_3;
    
    private int CreateSub;
    private bool gameOver;
    private SystemManager _systemManager;
    public bool GameOver
    {
        set { GameOver = gameOver; }
        get { return gameOver; }
    }

    void Start()
    {
        Cube = gameObject;
        AboutCreatPlane = GameObject.Find("Manager").GetComponent<CreatePlane>();
        _whiteGame = GameObject.Find("whitePoint");
        _blackGame = GameObject.Find("blackPoint");
        _blackGame0 = GameObject.Find("blackPoint_0");
        _blackGame1 = GameObject.Find("blackPoint_1");
        _whiteGame0 = GameObject.Find("whitePoint_0");
        _whiteGame1 = GameObject.Find("whitePoint_1");
        _systemManager = GameObject.Find("SystemManager").GetComponent<SystemManager>();
        CreateSub = 1;
        _systemManager.BeginRecordTime();

    }

    void Update()
    {
        gameOver = WhiteRayTest() || BlackRayTest()||_systemManager.GameOver || false;

        

        cubeVector3 = Cube.transform.position;
        bool Over0 = WhiteRayTest();
        bool Over1 = BlackRayTest();
        Debug.Log(Over0+"\n"+Over1);

        if (cubeVector3.x > (5 * CreateSub-3))
        {
            Debug.Log("do");
            AboutCreatPlane.HandleCreatePlane(cubeVector3,new  Vector3((5*CreateSub)+1,-0.5f,0));
            CreateSub++;
        }

    }

    public void BoxRun()
    {
        if (!gameOver)
        {
            Vector3 cubePoint = new Vector3(Cube.transform.position.x+0.5f, 0.5f, Cube.transform.position.z-0.5f);
            
            Cube.transform.RotateAround(cubePoint,new Vector3(0,0,1),90f);
        }
    }

    public void BoxJump()
    {
        
        if (!gameOver)
        {
            Vector3 targetPoint = new Vector3(Cube.transform.position.x + 2f, Cube.transform.position.y ,
                Cube.transform.position.z);
            Cube.transform.position = Vector3.Slerp(Cube.transform.position,targetPoint, 1f);
            if(Cube.transform.position == targetPoint)
                Cube.transform.position = Vector3.Slerp(Cube.transform.position,
                    new Vector3(Cube.transform.position.x + 1f, Cube.transform.position.y, Cube.transform.position.z),
                    1f);

        }
    }

    private bool WhiteRayTest()
    {
        
        Vector3 localWhitePosition = _whiteGame.transform.position;
        _ray_0 = new Ray(localWhitePosition,_whiteGame0.transform.position - localWhitePosition);
        _ray_1 = new Ray(localWhitePosition,_whiteGame1.transform.position - localWhitePosition);
        RaycastHit raycastHit_0;
        RaycastHit raycastHit_1;
        bool touchOrNot = false;
        if(Physics.Raycast(_ray_0, out raycastHit_0, Mathf.Infinity))  
        {  
            // 如果射线与平面碰撞，打印碰撞物体信息  
            //Debug.Log("碰撞对象: " + raycastHit_0.collider.tag);  
            // 在场景视图中绘制射线  
            //Debug.DrawLine(_ray_0.origin, raycastHit_0.point, Color.red);
            if (raycastHit_0.collider.tag == "BlackPlane")
            {
                touchOrNot = true;
            }
        }
        if(Physics.Raycast(_ray_1, out raycastHit_1, Mathf.Infinity))  
        {  
            // 如果射线与平面碰撞，打印碰撞物体信息  
            //Debug.Log("碰撞对象: " + raycastHit_1.collider.gameObject.name);  
            // 在场景视图中绘制射线  
            //Debug.DrawLine(_ray_1.origin, raycastHit_1.point, Color.red); 
            if (raycastHit_1.collider.tag == "BlackPlane")
            {
                touchOrNot = true;
            }
        }

        return touchOrNot;
    }
    
    private bool BlackRayTest()
    {
        Vector3 localBlackPosition = _whiteGame.transform.position;
        _ray_0 = new Ray(localBlackPosition,_blackGame0.transform.position - localBlackPosition);
        _ray_1 = new Ray(localBlackPosition,_blackGame1.transform.position - localBlackPosition);
        RaycastHit raycastHit_0;
        RaycastHit raycastHit_1;
        bool touchOrNot = false;
        if(Physics.Raycast(_ray_0, out raycastHit_0, Mathf.Infinity))  
        {  
            // 如果射线与平面碰撞，打印碰撞物体信息  
            //Debug.Log("碰撞对象: " + raycastHit_0.collider.tag);  
            // 在场景视图中绘制射线  
            //Debug.DrawLine(_ray_0.origin, raycastHit_0.point, Color.red);
            if (raycastHit_0.collider.tag == "WhitePlane")
            {
                touchOrNot = true;
            }
        }
        if(Physics.Raycast(_ray_1, out raycastHit_1, Mathf.Infinity))  
        {  
            // 如果射线与平面碰撞，打印碰撞物体信息  
            //Debug.Log("碰撞对象: " + raycastHit_1.collider.gameObject.name);  
            // 在场景视图中绘制射线  
            //Debug.DrawLine(_ray_1.origin, raycastHit_1.point, Color.red); 
            if (raycastHit_1.collider.tag == "WhitePlane")
            {
                touchOrNot = true;
            }
        }

        return touchOrNot;
    }


    

}
