using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAdjust : MonoBehaviour
{   
    //Our variables
    private Camera mainCamera;

    void Start()
    {
        //Get our characters camera
        mainCamera = Camera.main;
        
        //If there is no main camera give error
        if (mainCamera == null)
        {   
            Debug.LogError("There is no main camera.");
            return;
        }

        // Call to function
        AdjustObject();
    }

    void Update()
    {

    }

    //This function will adjust our canvas so that it is facing the user
    void AdjustObject()
    {
        //Calculate the direction
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Make the object face the camera (the quaternon only allows it to move in x and z so that it stays upright)
        transform.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up) * Quaternion.Euler(0, 180, 0);
    }
}