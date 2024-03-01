using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastInteractionManager : MonoBehaviour
{
    //--All variables used--

    //Script references
    public MainMenuController mainMenuController;

    //GameObjects
    private GameObject character;
    public GameObject mainMenu;
    public GameObject menuCanvasPrefab;
    private GameObject menuCanvasInstance;
    private GameObject lastSelectedObject;
    private GameObject copiedObject;
    private GameObject cutObject;

    //Raycast variables
    public LineRenderer lineRenderer;
    private LayerMask myLayerMask1;
    private LayerMask myLayerMask2;
    public float maxDistance = 10f;

    //other variables
    private Outline outline_component;
    private Button lastHighlightedButton;

    //booleans
    private bool is_cutting = false;
    private bool is_teleporting = false;
    public bool movement_is_constrained = false;
    public bool main_menu_is_active = false;
    private bool menu_is_active = false;

    //----Button Mappings----
    //CURRENT CONFIGURATION = MAC
    private string X_Button = "js11";
    private string Y_Button = "js5";
    private string A_Button = ""; 
    private string B_Button = "js10";
    private string OK_Button = "js7";

    //CURRENT CONFIGURATION = Android
    //private string X_Button = "js2";
    //private string Y_Button = "js3";
    //private string A_Button = "js10";
    //private string B_Button = "js5";
    //private string OK_Button = "";

   void Start()
    {   
        //Find layers for raycast
        myLayerMask1 = LayerMask.GetMask("raycast_objects");
        myLayerMask2 = LayerMask.GetMask("ground");

        //find character gameobject
        character = GameObject.Find("Character");
    }

    void Update()
    {   
        //if ok_button is pressed and the main menu is not active, then we open the main menu
        if (Input.GetButtonDown(OK_Button) && !main_menu_is_active)
        {
            // hide any open object menu
            HideMenu();

            // constraint the movement of the player
            movement_is_constrained = true;

            // activate the main menu
            mainMenu.SetActive(true);

            // set main menu active bool on (so other scripts can check if its active)
            main_menu_is_active = true;

            //set to first button in the main menu and highlight that button
            mainMenuController.currentIndex = 0;
            mainMenuController.HighlightButton(0);
        }

        // Get the position of the sphere named "raycast_origin"
        // Note: the reason I added a sphere slightly below camera view was to that the raycast 
        // appeared below the player so that they could see the raycast coming out in front of them
        Vector3 rayOrigin = GameObject.Find("raycast_origin").transform.position;

        // calculate the ray direction based on the camera's forward direction
        Vector3 rayDirection = Camera.main.transform.forward;

        // Perform the raycasts
        RaycastHit hit;
        // Condition checks whether ray cast hits a unique object and main menu is not active
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance, myLayerMask1) && !main_menu_is_active)
        {
            // If the raycast hits unique object, we will draw the line to the hit point
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);

            //Debug.Log("Raycast1 hit: " + hit.collider.gameObject.name);

            // Get the GameObject that the raycast hits
            GameObject hitObject = hit.collider.gameObject;

            // get the outline component from the hit object (so we can enable outline)
            outline_component = hitObject.GetComponent<Outline>();

            //If the hit unique object has an outline (which it always should)
            if (outline_component != null)
            {
                //Debug.Log("Found the outline");
                outline_component.enabled = true; // enable the outline of the unique object

                //if X button is pressed we want to open the object menu
                if (Input.GetButtonDown(X_Button))
                {
                    //Debug.Log("X_Button Pressed");
                    ShowMenu(hit.point); //open object menu next to object

                    lastSelectedObject = hitObject; //save the hit object for instantiation later
                }
            }

            //If an object menu is open
            if (menu_is_active)
            {
                //Debug.Log("menu is active");

                // Check if the object hit has a button component (meaning if we hit a button)
                Button button = hitObject.GetComponent<Button>();

                // if the object hit has a buton (meaning it is a button)
                if (button != null)
                {
                    //Debug.Log("button hit");
                    HighlightButton(button); // Highlight the button hit

                    // Check if button B is pressed, if so we will click it
                    if (Input.GetButtonDown(B_Button))
                    {
                        // Simulate a click on the highlighted button
                        //Debug.Log("button clicked");
                        button.onClick.Invoke(); //click the button
                    }
                }
            }

        }
        // Condition checks whether ray cast hits the ground/plane and main menu is not active
        else if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance, myLayerMask2) && !main_menu_is_active)
        {
            // If the raycast hits ground/plane, draw the line to the hit point
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);

            // Debug.Log("Raycast2 hit: " + hit.collider.gameObject.name);

            // Get the GameObject that the raycast hits
            GameObject hitObject = hit.collider.gameObject;

            // --- Pasting GameObjects----
            // if object menu is not active and button A is pressed
            if (!menu_is_active && Input.GetButtonDown(A_Button))
            {
                //If we previously clicked cut
                if (is_cutting)
                {
                    PasteCutObject(hit.point); //paste the cut object
                }
                //else if we clicked copy
                else
                {
                    PasteCopiedObject(hit.point); //paste last copied object
                }
            }

            // ---Teleporting Character---
            //teleports to that location on the ground
            if (Input.GetButtonDown(Y_Button) && !is_teleporting && !movement_is_constrained)
            {
                //Debug.Log("teleported");
                // had to use coroutine because unity update function would not always teleport player
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
        // Condition checks whether ray cast hits any other gameobject (other than unique and plane), and main menu is not active
        else if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance) && !main_menu_is_active)
        {
            // If the raycast hits any other object, draw the line to the hit point
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);

            //Debug.Log("Raycast3 hit: " + hit.collider.gameObject.name);

            // Disable the outline components previously enabled
            DisableAllOutlines();

            // If no object is hit, unhighlight the last highlighted button
            if (lastHighlightedButton != null)
            {
                UnhighlightButton(lastHighlightedButton);
            }

        }
        // if the ray cast is not hitting anything (for example looking at the sky), and main menu is not active
        else if (!main_menu_is_active)
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
    
    //method to paste the cut object
    void PasteCutObject(Vector3 position)
    {   
        //We basically just change its position to the new position
        cutObject.transform.position = position;
        cutObject.SetActive(true); //make it visible (since we disabled when we cut)
    }

    //method to copy the copied object
    void PasteCopiedObject(Vector3 position)
    {
        // Instantiate a copy of the copied object at the hit point
        GameObject instance = Instantiate(copiedObject, position, Quaternion.identity);
        instance.SetActive(true);
    }

    // method to highlight the button
    void HighlightButton(Button button)
    {
        // Unhighlight the last highlighted button
        if (lastHighlightedButton != null)
        {
            //unhighlight the last highlighted button
            UnhighlightButton(lastHighlightedButton);
        }

        // Highlight the new button
        ColorBlock colors = button.colors;
        colors.normalColor = Color.blue;
        button.colors = colors;

        //save last highlighted button
        lastHighlightedButton = button;
    }

    // method to unhighlight the button
    void UnhighlightButton(Button button)
    {
        //change button color back to default color
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        button.colors = colors;

        //set las highlighted to nothing
        lastHighlightedButton = null;
    }

    // method to show the object menu
    void ShowMenu(Vector3 position)
    {
        // calls hidemenu to ensure only one menu is active at a time
        HideMenu();

        // Add an offset to the position so that object menu appears above object
        position = position + new Vector3(0, 1, 0);

        // instantiave the object menu at the hit point position (plus some offset)
        menuCanvasInstance = Instantiate(menuCanvasPrefab, position, Quaternion.identity);

        //lock character movement, can only move head until menu is closed
        movement_is_constrained = true;

        // Set menu as active
        menu_is_active = true;
    }

    //method to hide the object menu
    void HideMenu()
    {
        // destroy any object menu open
        if (menuCanvasInstance != null)
        {
            Destroy(menuCanvasInstance);
        }

        //no object menu active
        menu_is_active = false;

        //remove character movement constraint
        movement_is_constrained = false;
    }

    // method to disable all the outlines of all objects 
    void DisableAllOutlines()
    {
        // Disable all outlines in the scene
        Outline[] outlines = FindObjectsOfType<Outline>();

        //disable each outline found
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }

    //teleports character to the position
    // taken from my previous assignment, was required due to the update function being called many times
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

    //-----public methods for the object menu buttons ----

    // method to copy the object
    public void CopyObject()
    {
        //the last object we pressed x on is the one we save for copying
        copiedObject = lastSelectedObject;

        //we close the object menu open
        HideMenu();
        
        //we set cutting to false in case we previously cut
        is_cutting = false;
    }

    // method to cut object
    public void CutObject()
    {
        //we set cutting to true
        is_cutting = true;

        //the last object we pressed x on is the one we save for later copying
        cutObject = lastSelectedObject;

        // Hide the last selected object
        lastSelectedObject.SetActive(false);

        //we close the object menu
        HideMenu();
    }

    // method to exit the object menu
    //does nothing other than hide menu and remove movement constraint
    public void ExitMenu()
    {
        //we destroy the instantiated menu
        HideMenu();

        //allow character movement again
        movement_is_constrained = false;
    }

}
