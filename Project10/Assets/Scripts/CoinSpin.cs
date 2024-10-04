using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    public float spinSpeed = 300f;

    void Update()
    {
       
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}