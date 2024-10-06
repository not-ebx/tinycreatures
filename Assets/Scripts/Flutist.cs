using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flutist : MonoBehaviour
{
    public GameObject mousePrefab; 
    public Transform launchPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchMouse();
        }
    }

    void LaunchMouse()
    {
        Instantiate(mousePrefab, launchPoint.position, Quaternion.identity);
    }
}
