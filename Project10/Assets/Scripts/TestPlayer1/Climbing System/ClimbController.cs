using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    PlayerController1 playerController;
    EnvironmentScanner envScanner;

    private void Awake()
    {
        playerController = GetComponent<PlayerController1>();
        envScanner = GetComponent<EnvironmentScanner>();
    }


    private void Update()
    {
        if (!playerController.IsHanging)
        {
            if (Input.GetButton("Jump") && !playerController.InAction)
            {
                if (envScanner.ClimbLedgeCheck(transform.forward, out RaycastHit ledgeHit))
                {
                    // Debug.Log("Climb Ledge Found");
                    playerController.SetControl(false);
                    StartCoroutine(JumpToLedge("IdleToHang", ledgeHit.transform, 0.41f, 0.54f));
                }
            }
        }
        else
        {
            // Ledge to Ledge Jump
        }

    }

    IEnumerator JumpToLedge(string anim, Transform ledge, float matchStartTime, float matchTargetTime)
    {
        var matchParams = new MatchTargetParams()
        {   
            //Ensure that all the ledge objects are facing forward vector
            pos = GetHandPos(ledge),
            bodyPart = AvatarTarget.RightHand,
            startTime = matchStartTime,
            targetTime = matchTargetTime,
            posWeight = Vector3.one
        };

        var targetRot = Quaternion.LookRotation(-ledge.forward);

        yield return playerController.DoAction(anim, matchParams, targetRot, true);

        playerController.IsHanging = true;
    }

    Vector3 GetHandPos(Transform ledge)
    {
        return ledge.position + (ledge.forward * 0.1f) + (Vector3.up * 0.1f) - (ledge.right * 0.25f);
    }

}
