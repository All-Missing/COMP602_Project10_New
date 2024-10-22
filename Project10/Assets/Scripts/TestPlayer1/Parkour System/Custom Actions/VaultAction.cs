using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Parkour System/Custom Actions/New vault action")]

public class VaultAction : ParkourAction
{    

    public override bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        if (!base.CheckIfPossible(hitData, player))
            return false;
        
        var hitPoint = hitData.forwardHit.transform.InverseTransformPoint(hitData.forwardHit.point);

        //Check condition if character player is mirroring the right hand properly, when
        // player from left back side to face direction to the fence object met or
        // player from right front side to face direction to the fence object met 
        if ((hitPoint.z < 0 && hitPoint.x < 0) || (hitPoint.z > 0 && hitPoint.x > 0))
        {
            // Mirror Animation
            Mirror = true;
            matchBodyPart = AvatarTarget.RightHand;
        }
        else
        {
            // Don't Mirror
            Mirror = false;
            matchBodyPart = AvatarTarget.LeftHand;
        }

        return true;
   }
   
}
