using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    //outline variables
    private Outline cube1_Outline;
    private Outline cube2_Outline;
    private Outline cube3_Outline;
    private Outline sphere1_Outline;
    private Outline sphere2_Outline;
    private Outline sphere3_Outline;

    //collision variables
    private CollisionDetection cube1_Collision;
    private CollisionDetection cube2_Collision;
    private CollisionDetection cube3_Collision;
    private CollisionDetection sphere1_Collision;
    private CollisionDetection sphere2_Collision;
    private CollisionDetection sphere3_Collision;

    private bool isOriginalColor;
    private bool isTeleporting;

    //Gameobjects
    private GameObject currentSphere;
    private GameObject character;
    private GameObject cube1;
    private GameObject cube2;
    private GameObject cube3;
    private GameObject sphere1;
    private GameObject sphere2;
    private GameObject sphere3;

    private Renderer objectRenderer;
    private Color originalColor;

    //Mappings
    //CURRENT CONFIGURATION = MAC
    //private string XButton = "js11";
    //private string YButton = "js5";

    //CURRENT CONFIGURATION = Android
    private string XButton = "js2";
    private string YButton = "js3";

    void Start()
    {
        //Using gameobject.find to find all the objects in the scene
        character = GameObject.Find("Character");

        cube1 = GameObject.Find("Cube1");
        cube2 = GameObject.Find("Cube2");
        cube3 = GameObject.Find("Cube3");

        sphere1 = GameObject.Find("Sphere1");
        sphere2 = GameObject.Find("Sphere2");
        sphere3 = GameObject.Find("Sphere3");

        //getting all the outline components from the game objects
        cube1_Outline = cube1.GetComponent<Outline>();
        cube2_Outline = cube2.GetComponent<Outline>();
        cube3_Outline = cube3.GetComponent<Outline>();
        sphere1_Outline = sphere1.GetComponent<Outline>();
        sphere2_Outline = sphere2.GetComponent<Outline>();
        sphere3_Outline = sphere3.GetComponent<Outline>();

        //getting all the collisionDetection scripts from the game objects
        cube1_Collision = cube1.GetComponent<CollisionDetection>();
        cube2_Collision = cube2.GetComponent<CollisionDetection>();
        cube3_Collision = cube3.GetComponent<CollisionDetection>();
        sphere1_Collision = sphere1.GetComponent<CollisionDetection>();
        sphere2_Collision = sphere2.GetComponent<CollisionDetection>();
        sphere3_Collision = sphere3.GetComponent<CollisionDetection>();

        objectRenderer = cube3.GetComponent<Renderer>();

        originalColor = objectRenderer.material.color;
    }

    void Update()
    {
        // if else statement to perform translation, rotation, color change, and teleportation
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
        else if (Input.GetButtonDown(YButton) && sphere1_Outline.enabled == true && !isTeleporting)
        {
            sphere1_Outline.enabled = false;
            StartCoroutine(TeleportCoroutine(sphere1));

        }
        else if (Input.GetButtonDown(YButton) && sphere2_Outline.enabled == true && !isTeleporting)
        {
            sphere2_Outline.enabled = false;
            StartCoroutine(TeleportCoroutine(sphere2));
            
        }
        else if (Input.GetButtonDown(YButton) && sphere3_Outline.enabled == true && !isTeleporting)
        {
            sphere3_Outline.enabled = false;
            StartCoroutine(TeleportCoroutine(sphere3));
        }
    }

    //moves the object in the -x direction
    void TranslateObject()
    {
        cube1.transform.position += Vector3.back * 0.1f;
    }

    //rotates the object around its axis
    void RotateObject()
    {
        cube2.transform.RotateAround(cube2.transform.position, Vector3.up, 4f);
    }

    //changes the objects color between original color and black
    void ChangeObjectColor()
    {
        if (!isOriginalColor)
        {
            objectRenderer.material.color = originalColor;
        }
        else
        {
            objectRenderer.material.color = Color.black;
        }

        isOriginalColor = !isOriginalColor;
    }

    //teleports to the object
    IEnumerator TeleportCoroutine(GameObject currentSphere)
    {

        isTeleporting = true;

        Vector3 finalPosition = currentSphere.transform.position;

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

        currentSphere.SetActive(false);

        isTeleporting = false;
    }
}
