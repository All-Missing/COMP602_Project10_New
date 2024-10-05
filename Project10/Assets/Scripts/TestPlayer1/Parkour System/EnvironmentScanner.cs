using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffSet = new Vector3 (0, 2.5f, 0);
    [SerializeField] float forwardRayLength = 0.8f;
    [SerializeField] float heightRayLength = 5f;
    [SerializeField] float ledgeRayLength = 10f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float ledgeHeightThreshold = 0.75f;

    public ObstacleHitData ObstacleCheck()
    {
        var hitData = new ObstacleHitData();

        var forwardOrigin = transform.position + forwardRayOffSet;
        hitData.forwardHitFound = Physics.Raycast(forwardOrigin, transform.forward,
            out hitData.forwardHit,forwardRayLength, obstacleLayer);

        //Debug for handling RayCast forward direction
        Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength, (hitData.forwardHitFound) ? Color.red : Color.white);

        if (hitData.forwardHitFound)
        {
            var heighOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;

            hitData.heightHitFound = Physics.Raycast(heighOrigin, Vector3.down,
            out hitData.heightHit, heightRayLength, obstacleLayer);

            //Debug for handling Raycast from Vertical Y height direction
            Debug.DrawRay(heighOrigin, Vector3.down * heightRayLength, (hitData.heightHitFound) ? Color.red : Color.white);
        }

        return hitData;
    }

    //Implement Ledge dection
    public bool LedgeCheck(Vector3 moveDir)
    {
        if (moveDir == Vector3.zero)
            return false;
        
        float originOffSet = 0.5f;
        var origin = transform.position + (moveDir * originOffSet) + Vector3.up;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, ledgeRayLength, obstacleLayer))
        {
            //Debug to dra ray if the ledge is dectect, then draw green line
            Debug.DrawRay(origin, Vector3.down * ledgeRayLength, Color.green);
            float height = transform.position.y - hit.point.y;

            if (height > ledgeHeightThreshold)
            {
                return true;
            }
        }

        return false;
    }
    
}

public struct ObstacleHitData
{

    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}
