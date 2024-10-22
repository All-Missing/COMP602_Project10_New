using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ESC")) { //Check if user press Escape key
            Debug.Log("User press key 'ECS' !");
            Application.Quit();
        }
    }
}
