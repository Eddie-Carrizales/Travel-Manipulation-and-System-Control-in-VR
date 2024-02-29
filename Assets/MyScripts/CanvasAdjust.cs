using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAdjust : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;
        
        // Make sure we have a camera to face
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene!");
            return;
        }

        // Call the FaceCamera method immediately upon start
        FaceCamera();
    }

    void Update()
    {
        // Call the FaceCamera method continuously
        FaceCamera();
    }

    void FaceCamera()
    {
        // Calculate the direction from the canvas to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Ensure the canvas faces the camera while keeping its up direction (Y-axis) unchanged
        transform.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up) * Quaternion.Euler(0, 180, 0);
    }
}

