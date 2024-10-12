using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [SerializeField] List<ParkourAction> parkourActions;
    [SerializeField] ParkourAction jumpDownAction;

    bool inAction;

    EnvironmentScanner environmentScanner;
    Animator animator;
    PlayerController1 playerController;

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController1>();
    }

    private void Update()
    {
        var hitData = environmentScanner.ObstacleCheck();

        if (Input.GetButton("Jump") && !inAction)
        {
            
            if (hitData.forwardHitFound)
            {
                //For debugging obstacle objects found
                Debug.Log("Obstacle Found " + hitData.forwardHit.transform.name);
                foreach (var action in parkourActions)
                {
                    if (action.CheckIfPossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }

            }
        }

        // Check if a character is on ledge of object before perfoming jumping down animation
        if (playerController.IsOnLedge && !inAction && !hitData.forwardHitFound && Input.GetButton("Jump"))
        {
            // Double check if a ledge position is too high, then performing jumping down animation
            if (playerController.LedgeData.angle <= 50)
            {
                playerController.IsOnLedge = false;
                StartCoroutine(DoParkourAction(jumpDownAction));
            }

        }
    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        inAction = true;
        playerController.SetControl(false);

        animator.SetBool("mirrorAction", action.Mirror);
        animator.CrossFade(action.AnimName, 0.2f);
        yield return null;

        var animaState = animator.GetNextAnimatorStateInfo(0);
        if (!animaState.IsName(action.AnimName))
            Debug.LogError("The parkour animation is wrong!"); 

        float timer = 0f;
        while (timer <= animaState.length)
        {
            timer += Time.deltaTime;

            // Rotate the player towards the obstacle
            if (action.RotateToObstacle)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.RotationSpeed * Time.deltaTime);
            
            if (action.EnableTargetMatching)
                MatchTarget(action);
            
            //prevent player perform animation transition during in air state, gravity handles player position Y.
            if (animator.IsInTransition(0) && timer > 0.5f)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(action.PostActionDelay);

        playerController.SetControl(true);
        inAction = false;    
    }

    void MatchTarget(ParkourAction action)
    {
        if (animator.isMatchingTarget) return;

        animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBodyPart, new MatchTargetWeightMask(action.MatchPosWeight, 0),
            action.MatchStartTime, action.MatchTargetTime);
    }
}


