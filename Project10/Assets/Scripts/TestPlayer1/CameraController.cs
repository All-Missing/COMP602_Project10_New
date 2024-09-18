using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5;

    [SerializeField] float minVerticalAngle = -10;
    [SerializeField] float maxVerticalAngle = 45;

    [SerializeField] Vector2 framingOffSet;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationX;
    float rotationY;
    
    float invertXValue;
    float invertYValue;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;    
    }

    void Update()
    {
        invertXValue = (invertX) ? -1 : 1;
        invertYValue = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Camera Y") * invertYValue * rotationSpeed;
        rotationX = Math.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        rotationY += Input.GetAxis("Camera X") * invertXValue * rotationSpeed;
        

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPosition = followTarget.position + new Vector3(framingOffSet.x, framingOffSet.y);


        transform.position = focusPosition -  targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;            
    }

    // Method to create property
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
