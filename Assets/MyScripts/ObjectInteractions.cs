using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject hoveredObject;
    private Renderer objectRenderer;
    private Color originalColor;
    private Outline objectOutline;
    private bool collisionOccurred;
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
    private string XButton = "js11";
    private string YButton = "js5";

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


        // Get the Renderer component attached to the cube3 GameObject
        objectRenderer = cube3.GetComponent<Renderer>();

        // Store the original color of the object
        originalColor = objectRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the car when X key is pressed and hover movement is enabled
        if (!collisionOccurred && Input.GetButtonDown(XButton) && hoveredObject.name == "Cube1")
        {
            TranslateObject();
        }
        else if (Input.GetButtonDown(XButton) && hoveredObject.name == "Cube2")
        {
            RotateObject();
        }
        else if (Input.GetButtonDown(XButton) && hoveredObject.name == "Cube3")
        {
            ChangeObjectColor();
        }
        else if (Input.GetButtonDown(YButton) && (hoveredObject.name == "Sphere1" || hoveredObject.name == "Sphere2" || hoveredObject.name == "Sphere3"))
        {
            TeleportToObject();
        }
    }

    void TranslateObject()
    {
        // Move the cube 1 centimeter in the x direction every frame
        cube1.transform.position += Vector3.right * 0.01f;
    }

    void RotateObject()
    {
        // Rotate the cube by 1 degree around the y-axis
        cube2.transform.Rotate(Vector3.up, 1f);
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

    void TeleportToObject()
    {
        character.transform.position = hoveredObject.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (gameObject.tag == "assignment2Objects")
        {
            hoveredObject = gameObject; // Store reference to the object when pointer enters
            
            //Get outline scripts from the game objects
            Outline objectOutline = hoveredObject.GetComponent<Outline>();
            objectOutline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoveredObject != null)
        {
            hoveredObject = null; // Clear reference when pointer exits
            objectOutline.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        // Check if the collided object has a specific tag and collides with "cube1"
        if (collision.gameObject.CompareTag("assignment2Objects") && collision.gameObject == cube1)
        {
            // Occurs when collision happens with an object with the specified tag and "cube1"
            collisionOccurred = true;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        // Check if the collided object has a specific tag and collides with "cube1"
        if (collision.gameObject.CompareTag("assignment2Objects") && collision.gameObject == cube1)
        {
            // Occurs when collision happens with an object with the specified tag and "cube1"
            collisionOccurred = false;
        }
    }
}
