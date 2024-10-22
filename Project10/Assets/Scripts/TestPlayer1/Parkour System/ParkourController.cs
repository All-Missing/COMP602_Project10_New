using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [SerializeField] List<ParkourAction> parkourActions;
    bool inAction;

    EnvironmentScanner environmentScanner;
    Animator animator;
    PlayerController1 playerController;
    CharacterSoundManager soundManager;

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController1>();
        soundManager = FindObjectOfType<CharacterSoundManager>();
    }

    private void Update()
    {
        if (Input.GetButton("Jump") && !inAction)
        {
            var hitData = environmentScanner.ObstacleCheck();
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
    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        inAction = true;

        if (soundManager != null) // Play vaulting sound
        {
            soundManager.PlayVaultingSound();
            Debug.Log("Playing Vault Sound");
        }
        else
        {
            Debug.Log(soundManager);
        }

        playerController.SetControl(false);

        if (soundManager != null) // Play vaulting sound
        {
            soundManager.PlayVaultingSound();
        }

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


