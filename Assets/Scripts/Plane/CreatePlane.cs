using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatePlane : MonoBehaviour
{
    private float randomNumber;
    private int[] HaveCreateNum;
    private GameObject[] planeGameObject;
    private GameObject[] blackPlane;
    void Awake()
    {
        planeGameObject = new GameObject[Config.PLANETABLE.Length];
        blackPlane = new GameObject[9];
        HaveCreateNum = new int[]{0,0};
        for(int i = 0;i < Config.PLANETABLE.Length;i++)
        {
            planeGameObject[i]=GameObject.Find(Config.PLANETABLE[i]);

        }
        

    }

    void Start()
    {
        
    }


    void Update()
    {

    }

    public void HandleCreatePlane(Vector3 cubePosition,Vector3 targetPosition)
    {
        
        randomNumber = Random.Range(0, 4);
        if (randomNumber <= 1 && (cubePosition.x - planeGameObject[0].transform.position.x) > 3)
        {
            planeGameObject[0].transform.position = targetPosition;
        }else if (randomNumber > 1 && randomNumber<= 2&& (cubePosition.x - planeGameObject[1].transform.position.x) > 3)
        {
            planeGameObject[1].transform.position = targetPosition;
        }else if (randomNumber > 2 && randomNumber<= 3&& (cubePosition.x - planeGameObject[2].transform.position.x) > 3)
        {
            planeGameObject[2].transform.position = targetPosition;
        }else if (randomNumber > 3 && randomNumber<= 4&& (cubePosition.x - planeGameObject[3].transform.position.x) > 3)
        {
            planeGameObject[3].transform.position = targetPosition;
        }
        else
        {
            HandleCreatePlane(cubePosition, targetPosition);
        }
    }

    
}
