using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform CameraPoint;
    private Transform CameraTransform;
    private Vector3 CurrentVelocity =Vector3.zero;
    private void Awake()
    {
        CameraTransform = gameObject.transform;
        CameraPoint = GameObject.Find("CameraPoint").GetComponent<Transform>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = new Vector3(CameraPoint.position.x-3f,CameraPoint.position.y+2,CameraPoint.position.z-3f);
        CameraTransform.position = Vector3.SmoothDamp(CameraTransform.position, cameraPosition,ref CurrentVelocity, 0.05f);

    }
}
