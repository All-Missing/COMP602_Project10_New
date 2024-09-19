using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffSet = new Vector3 (0, 2.5f, 0);
    [SerializeField] float forwardRayLength = 0.8f;
    [SerializeField] float heightRayLength = 5;
    [SerializeField] LayerMask obstacleLayer;

    public ObstacleHitData ObstacleCheck()
    {
        var hitData = new ObstacleHitData();

        var forwardOrigin = transform.position + forwardRayOffSet;
        hitData.forwardHitFound = Physics.Raycast(forwardOrigin, transform.forward,
            out hitData.forwardHit,forwardRayLength, obstacleLayer);

        //Debug for handling RayCast forward direction
        Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength, hitData.forwardHitFound ? Color.red : Color.white);

        if (hitData.forwardHitFound)
        {
            var heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;

            hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down,
                out hitData.heightHit, heightRayLength, obstacleLayer);

            //Debug for handling Raycast from Vertical Y height direction
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, hitData.heightHitFound ? Color.red : Color.white);
        }

        return hitData;
    }

    // public bool LedgeCheck(Vector3 moveDir)
    // {
    //     if (moveDir == Vector3.zero)
    //         return false;
    //     float originOffSet = 0.5f;
    //     var origin = transform.position + moveDir;    
    // }
    
}



public struct ObstacleHitData
{

    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}
