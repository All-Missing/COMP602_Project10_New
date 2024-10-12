using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{

    EnvironmentScanner envScanner;

    private void Awake()
    {
        envScanner = GetComponent<EnvironmentScanner>();    
    }


    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            if (envScanner.ClimbLedgeCheck(transform.forward, out RaycastHit ledgeHit))
            {
                Debug.Log("Climb Ledge Found");
            }
        }    
    }
}
