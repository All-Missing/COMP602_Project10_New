using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player, cameraTransform;

    // Update is called once per frame
    void Update()
    {
        cameraTransform.LookAt(player);
    }
}
