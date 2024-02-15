using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    private GameObject currentSphere;
    private Renderer objectRenderer;
    private Color originalColor;

    private Outline cube1_Outline;
    private Outline cube2_Outline;
    private Outline cube3_Outline;
    private Outline sphere1_Outline;
    private Outline sphere2_Outline;
    private Outline sphere3_Outline;

    private CollisionDetection cube1_Collision;
    private CollisionDetection cube2_Collision;
    private CollisionDetection cube3_Collision;
    private CollisionDetection sphere1_Collision;
    private CollisionDetection sphere2_Collision;
    private CollisionDetection sphere3_Collision;

    private bool isOriginalColor;
    private GameObject character;
    private GameObject cube1;
    private GameObject cube2;
    private GameObject cube3;
    private GameObject sphere1;
    private GameObject sphere2;
    private GameObject sphere3;

    //Mappings
    //CURRENT CONFIGURATION = MAC
    //private string XButton = "js11";
    //private string YButton = "js5";

    //CURRENT CONFIGURATION = Android
    private string XButton = "js2";
    private string YButton = "js3";

    // Start is called before the first frame update
    void Start()
    {
        // Find the object with the specified name
        character = GameObject.Find("Character");

        cube1 = GameObject.Find("Cube1");
        cube2 = GameObject.Find("Cube2");
        cube3 = GameObject.Find("Cube3");

        sphere1 = GameObject.Find("Sphere1");
        sphere2 = GameObject.Find("Sphere2");
        sphere3 = GameObject.Find("Sphere3");

        cube1_Outline = cube1.GetComponent<Outline>();
        cube2_Outline = cube2.GetComponent<Outline>();
        cube3_Outline = cube3.GetComponent<Outline>();
        sphere1_Outline = sphere1.GetComponent<Outline>();
        sphere2_Outline = sphere2.GetComponent<Outline>();
        sphere3_Outline = sphere3.GetComponent<Outline>();

        cube1_Collision = cube1.GetComponent<CollisionDetection>();
        cube2_Collision = cube2.GetComponent<CollisionDetection>();
        cube3_Collision = cube3.GetComponent<CollisionDetection>();
        sphere1_Collision = sphere1.GetComponent<CollisionDetection>();
        sphere2_Collision = sphere2.GetComponent<CollisionDetection>();
        sphere3_Collision = sphere3.GetComponent<CollisionDetection>();

        // Get the Renderer component attached to the cube3 GameObject
        objectRenderer = cube3.GetComponent<Renderer>();

        // Store the original color of the object
        originalColor = objectRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {

        // Move the car when X key is pressed and hover movement is enabled
        if (cube1_Collision.isColliding == false && Input.GetButton(XButton) && cube1_Outline.enabled == true)
        {
            TranslateObject();
        }
        else if (Input.GetButton(XButton) && cube2_Outline.enabled == true)
        {
            RotateObject();
        }
        else if (Input.GetButtonDown(XButton) && cube3_Outline.enabled == true)
        {
            ChangeObjectColor();
        }
        else if (Input.GetButtonDown(YButton) && (sphere1_Outline.enabled == true))
        {
            TeleportToObject(sphere1);
        }
        else if (Input.GetButtonDown(YButton) && (sphere2_Outline.enabled == true))
        {
            TeleportToObject(sphere2);
        }
        else if (Input.GetButtonDown(YButton) && (sphere3_Outline.enabled == true))
        {
            TeleportToObject(sphere3);
        }
    }

    void TranslateObject()
    {
        // Move the cube 1 centimeter in the x direction every frame
        cube1.transform.position += Vector3.back * 0.1f;
    }

    void RotateObject()
    {
        // Rotate the cube by 1 degree around the y-axis
        cube2.transform.RotateAround(cube2.transform.position, Vector3.up, 5f);
    }

    void ChangeObjectColor()
    {
        // Toggle between the original color and black
        if (!isOriginalColor)
        {
            objectRenderer.material.color = originalColor;
        }
        else
        {
            objectRenderer.material.color = Color.black;
        }
        // Update the toggle state
        isOriginalColor = !isOriginalColor;
    }

    void TeleportToObject(GameObject currentSphere)
    {
        character.transform.position = currentSphere.transform.position;
        Destroy(currentSphere);
    }
}
