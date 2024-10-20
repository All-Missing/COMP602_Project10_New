using System.Collections;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    ClimbPoint currentPoint;
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
                    currentPoint = ledgeHit.transform.GetComponent<ClimbPoint>();
                    playerController.SetControl(false);
                    StartCoroutine(JumpToLedge("IdleToHang", ledgeHit.transform, 0.41f, 0.54f));
                }
            }
        }
        else
        {
            // Once character is hanging a ledge object, character performs hop up/right/drop
            float h = Mathf.Round(Input.GetAxisRaw("Horizontal"));
            float v = Mathf.Round(Input.GetAxisRaw("Vertical"));
            var inputDir = new Vector2(h, v);

            // If player in action, then it won't any perform animations below
            if (playerController.InAction || inputDir == Vector2.zero)
                return;

            var neigbour = currentPoint.GetNeighbour(inputDir);            
            if (neigbour == null) // If there is no next neighbour obj, character won't perform any animation
                return;

            // This perform hop up, hop down animations while player is hanging and pressing jump hotkey
            if (neigbour.connectionType == ConnectionType.Jump && Input.GetButton("Jump"))
            {
                currentPoint = neigbour.point;

                if (neigbour.direction.y == 1)
                    StartCoroutine(JumpToLedge("HangHopUp", currentPoint.transform, 0.35f, 0.65f));
                else if (neigbour.direction.y == -1)
                    StartCoroutine(JumpToLedge("HangHopDown", currentPoint.transform, 0.31f, 0.65f));
                else if (neigbour.direction.x == 1)
                    StartCoroutine(JumpToLedge("HangHopRight", currentPoint.transform, 0.20f, 0.50f));
                else if (neigbour.direction.x == -1)
                    StartCoroutine(JumpToLedge("HangHopLeft", currentPoint.transform, 0.20f, 0.50f));         
            }
            else if (neigbour.connectionType == ConnectionType.Move)
            {
                currentPoint = neigbour.point;

                if (neigbour.direction.x == 1)
                    StartCoroutine(JumpToLedge("ShimmyRight", currentPoint.transform, 0.0f, 0.38f));
                else if (neigbour.direction.x == -1)
                    StartCoroutine(JumpToLedge("ShimmyLeft", currentPoint.transform, 0.0f, 0.38f));
                

            }    

        }
    }

    IEnumerator JumpToLedge(string anim, Transform ledge, float matchStartTime, float matchTargetTime,
        AvatarTarget hand=AvatarTarget.RightHand)
    {
        var matchParams = new MatchTargetParams()
        {
            //Ensure that all the ledge objects are facing forward vector
            pos = GetHandPos(ledge, hand),
            bodyPart = hand,
            startTime = matchStartTime,
            targetTime = matchTargetTime,
            posWeight = Vector3.one
        };

        var targetRot = Quaternion.LookRotation(-ledge.forward);

        yield return playerController.DoAction(anim, matchParams, targetRot, true);

        playerController.IsHanging = true;
    }

    Vector3 GetHandPos(Transform ledge, AvatarTarget hand)
    {
        var hDir = (hand == AvatarTarget.RightHand) ? ledge.right : -ledge.right;
        return ledge.position + (ledge.forward * 0.1f) + (Vector3.up * 0.1f) - (hDir * 0.25f);
    }

}
