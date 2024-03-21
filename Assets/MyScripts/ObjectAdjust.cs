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

    //This method will adjust our object to that when we are going to place the object, it faces the user.
    void AdjustObject()
    {
        //Calculate the direction
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Make the object you are going to place face the camera (it will only rotate around y-axis)
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.up); // get direction to where it will face
        Vector3 eulerRotation = targetRotation.eulerAngles; //convert to euler angles
        transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0); //rotate it
    }
}