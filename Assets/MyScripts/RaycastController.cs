using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public float maxDistance = 10f;
    public LayerMask myLayerMask;
    public LineRenderer lineRenderer;
    public float distanceFromCamera = 0.2f; // Distance from camera to start the raycast
    private Outline outline_component;

   void Start()
    {
        myLayerMask = LayerMask.GetMask("raycast_objects");
    }

    void Update()
    {
        // Get the position of the sphere named "raycast_origin"
        Vector3 rayOrigin = GameObject.Find("raycast_origin").transform.position;

        // Calculate the ray direction based on the camera's forward direction
        Vector3 rayDirection = Camera.main.transform.forward;

        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance, myLayerMask))
        {
            // If the raycast hits something, draw the line to the hit point
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);

            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            // Get the GameObject that the raycast hits
            GameObject hitObject = hit.collider.gameObject;

            // Check if the hit object has the desired component
            // Replace 'YourComponent' with the actual component type you're looking for
            outline_component = hitObject.GetComponent<Outline>();

            if (outline_component != null)
            {
                // Do something with the component
                // For example, you can access its properties or call its methods
                Debug.Log("Found the component!");
                outline_component.enabled = true;
            }
        }
        else
        {
            // If the raycast doesn't hit anything, draw the line to the maximum distance
            Vector3 rayEnd = rayOrigin + rayDirection * maxDistance;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, rayEnd);

            // Disable the outline component if it was previously enabled
            if (outline_component != null)
            {
                outline_component.enabled = false;
            }
        }
    }

}
