using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastInteractionManager : MonoBehaviour
{
    public float maxDistance = 10f;
    private LayerMask myLayerMask1;
    private LayerMask myLayerMask2;
    public LineRenderer lineRenderer;
    private Outline outline_component;

    public GameObject menuCanvasPrefab;
    private GameObject menuCanvasInstance;
    private GameObject lastSelectedObject;
    private Button lastHighlightedButton;
    private GameObject character;
    private bool is_cutting = false;
    private bool is_teleporting = false;
    public bool movement_is_constrained = false;

    private GameObject copiedObject; // Reference to the copied object
    private GameObject cutObject; // Reference to the cut object

    private bool menu_is_active = false;

    //Mappings
    //CURRENT CONFIGURATION = MAC
    private string X_Button = "js11";
    private string Y_Button = "js5";
    private string A_Button = "js7"; //trigger button
    private string B_Button = "js10";

    //CURRENT CONFIGURATION = Android
    //private string X_Button = "js2";
    //private string Y_Button = "js3";
    //private string A_Button = "js10";
    //private string B_Button = "js5";

   void Start()
    {
        myLayerMask1 = LayerMask.GetMask("raycast_objects");
        myLayerMask2 = LayerMask.GetMask("ground");
        character = GameObject.Find("Character");
    }

    void Update()
    {
        // Get the position of the sphere named "raycast_origin"
        Vector3 rayOrigin = GameObject.Find("raycast_origin").transform.position;

        // Calculate the ray direction based on the camera's forward direction
        Vector3 rayDirection = Camera.main.transform.forward;

        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance, myLayerMask1))
        {
            // If the raycast hits something, draw the line to the hit point
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);

            Debug.Log("Raycast1 hit: " + hit.collider.gameObject.name);

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


                //open a menu for object selected
                if (Input.GetButtonDown(X_Button))
                {
                    //Debug.Log("X_Button Pressed");
                    //open a menu next to object
                    ShowMenu(hit.point);

                    lastSelectedObject = hitObject;

                }
            }

            //If a menu is open and we can use button B to select a button
            if (menu_is_active)
            {
                Debug.Log("menu is active");
                //click menu button

                // Check if the hit object has a Button component
                Button button = hitObject.GetComponent<Button>();
                if (button != null)
                {
                    Debug.Log("button hit");
                    // Highlight the button
                    HighlightButton(button);

                    // Check if the input button is pressed
                    if (Input.GetButtonDown(B_Button))
                    {
                        // Simulate a click on the highlighted button
                        Debug.Log("button clicked");
                        button.onClick.Invoke();
                    }
                }
            }

        }
        //if it hits the plane/ground
        else if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance, myLayerMask2))
        {
            // If the raycast hits something, draw the line to the hit point
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);

            Debug.Log("Raycast2 hit: " + hit.collider.gameObject.name);

            // Get the GameObject that the raycast hits
            GameObject hitObject = hit.collider.gameObject;

            //paste the last copied object
            if (!menu_is_active && Input.GetButtonDown(A_Button))
            {
                if (is_cutting)
                {
                    PasteCutObject(hit.point);
                }
                else
                {
                    //paste last copied object
                    PasteCopiedObject(hit.point);
                }
            }

            //teleports to that location on the ground
            if (Input.GetButtonDown(Y_Button) && !is_teleporting && !movement_is_constrained)
            {
                Debug.Log("teleported");

                StartCoroutine(TeleportCoroutine(hit.point));
            }

            // Disable the outline components previously enabled
            DisableAllOutlines();

            // If no object is hit, unhighlight the last highlighted button
            if (lastHighlightedButton != null)
            {
                UnhighlightButton(lastHighlightedButton);
            }

        }
        //if it hits any other gameobject
        else if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance))
        {
            // If the raycast hits something, draw the line to the hit point
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);

            Debug.Log("Raycast3 hit: " + hit.collider.gameObject.name);

            // Disable the outline components previously enabled
            DisableAllOutlines();

            // If no object is hit, unhighlight the last highlighted button
            if (lastHighlightedButton != null)
            {
                UnhighlightButton(lastHighlightedButton);
            }

        }
        else
        {
            // If the raycast doesn't hit anything, draw the line to the maximum distance
            Vector3 rayEnd = rayOrigin + rayDirection * maxDistance;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, rayEnd);

            // Disable the outline components previously enabled
            DisableAllOutlines();

            // If no object is hit, unhighlight the last highlighted button
            if (lastHighlightedButton != null)
            {
                UnhighlightButton(lastHighlightedButton);
            }
        }
    }

    void PasteCutObject(Vector3 position)
    {
        cutObject.transform.position = position;
        cutObject.SetActive(true);
    }

    void PasteCopiedObject(Vector3 position)
    {
        // Instantiate a copy of the copied object at the hit point
        GameObject instance = Instantiate(copiedObject, position, Quaternion.identity);
        instance.SetActive(true);
    }

    // Method to highlight the button
    void HighlightButton(Button button)
    {
        // Unhighlight the last highlighted button
        if (lastHighlightedButton != null)
        {
            UnhighlightButton(lastHighlightedButton);
        }

        // Highlight the new button
        ColorBlock colors = button.colors;
        colors.normalColor = Color.yellow;
        button.colors = colors;

        lastHighlightedButton = button;
    }

    // Method to unhighlight the button
    void UnhighlightButton(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        button.colors = colors;

        lastHighlightedButton = null;
    }

    void ShowMenu(Vector3 position)
    {
        // Ensure only one menu is active at a time
        HideMenu();

        // Instantiate the menu canvas prefab
        menuCanvasInstance = Instantiate(menuCanvasPrefab, position, Quaternion.identity);

        // Position the menu canvas near the object
        menuCanvasInstance.transform.position = position + new Vector3(0, 1, 0); // Adjust position as needed

        //enable menu button box colliders this is to fix button highlighting bug
        // Enable interaction components after a delay
        StartCoroutine(EnableMenuButtonCollidersAfterDelay());

        //lock character movement, can only move head until menu is closed
        movement_is_constrained = true;
    }

    IEnumerator EnableMenuButtonCollidersAfterDelay()
    {
        // Wait for a certain delay
        yield return new WaitForSeconds(0.5f); // Adjust the delay as needed

        // Enable collider for all button objects
        Collider[] colliders = menuCanvasInstance.GetComponentsInChildren<Collider>(true);
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        // Set menu as active
        menu_is_active = true;
    }

    void HideMenu()
    {
        // Destroy any existing menu canvas instance
        if (menuCanvasInstance != null)
        {
            Destroy(menuCanvasInstance);
        }

        menu_is_active = false;

        movement_is_constrained = false;
    }

    //public functions for the menus
    public void CopyObject()
    {

        //the last object we pressed x on is the one we save for copying
        copiedObject = lastSelectedObject;

        //we close the menu open
        HideMenu();

        is_cutting = false;
    }

    public void CutObject()
    {
        is_cutting = true;

        //the last object we pressed x on is the one we save for copying
        //the las object we pressed x on is the one we save for later copying
        cutObject = lastSelectedObject;

        // Hide the last selected object
        lastSelectedObject.SetActive(false);

        // we set menu count to 0
        menu_is_active = true;

        //we close the menu open
        HideMenu();
    }

    void DisableAllOutlines()
    {
        // Disable all outlines in the scene
        Outline[] outlines = FindObjectsOfType<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }

    public void ExitMenu()
    {
        //we destroy the instantiated menu
        HideMenu();

        //allow character movement again
        movement_is_constrained = false;
    }

    //teleports to the position
    IEnumerator TeleportCoroutine(Vector3 position)
    {

        is_teleporting = true;

        // prevents character from clipping below ground
        var characterHeightOffset =  1.0f;

        //add an offset to prevent character from clipping below plane
        Vector3 finalPosition = position + Vector3.up * characterHeightOffset;

        float elapsedTime = 0;

        //Added this re-teleportation timer becuase it kept skiping the teleportation code
        //maybe MAC problem, but I will keep the code just in case.
        while (elapsedTime < 0.1f)
        {
            character.transform.position = finalPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //sets object position to currentSphere which is being poited at.
        character.transform.position = finalPosition;

        is_teleporting = false;
    }

}
