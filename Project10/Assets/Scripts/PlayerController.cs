using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;


   void Update()
   {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        var moveInput = (new Vector3(h, 0, v)).normalized;

        transform.position += moveInput * moveSpeed * Time.deltaTime;

   }
}
