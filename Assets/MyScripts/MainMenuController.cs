using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{   
    //--Variables Used--

    //Scripts referenced
    public RaycastInteractionManager raycastInteractionManager;
    public CharacterMovement characterMovement;

    //Menu variables
    public Button[] menuButtons;
    public TMP_Text raycast_length;
    public TMP_Text speed;
    public string newText = "";


    //Other variables
    public int currentIndex = 0; // current selected button
    private bool canNavigate = true;
    private float navigationThreshold = 0.5f; // threshold to detect joystick movement
    private float selectionCooldown = 0.5f; // cooldown between button selections

    //For MAC
    private string B_Button = "js10";

    //FOR ANDROID
    //private string B_Button = "js10";

    void Start()
    {

    }


    void Update()
    {   
        //Only allow controller manipulation is the menu is active
        if (raycastInteractionManager.main_menu_is_active)
        {   
            //Get the controller joystick vertical axis
            float verticalInput = Input.GetAxis("Vertical");

            // --These conditions check if joystick input is greater than our threshold (for up and down), and our cooldown is done--
            if (Mathf.Abs(verticalInput) > navigationThreshold && canNavigate)
            {
                // select button up or down based on direction of joystick (positive or negative)
                if (verticalInput > 0)
                {
                    //note: we needed coroutines due to update function
                    StartCoroutine(NavigateOption(currentIndex - 1));
                }
                else if (verticalInput < 0)
                {
                    StartCoroutine(NavigateOption(currentIndex + 1));
                }
            }

            // We use the B button to select a button
            if (Input.GetButtonDown(B_Button))
            {
                menuButtons[currentIndex].onClick.Invoke(); // clicks the current button
            }

        }
    }

    //method used by coroutine due to the update loop
    //this will work as sort of a cooldown and will also loop our button selection
    IEnumerator NavigateOption(int newIndex)
    {
        // We will prevent navigation until cooldown ends
        canNavigate = false;

        // condition to loop the button selection 
        if (newIndex < 0)
        {
            //go to the last option
            newIndex = menuButtons.Length - 1;
        }
        else if (newIndex >= menuButtons.Length)
        {
            //go to the first option
            newIndex = 0;
        }

        // Highlight the new selected option
        HighlightButton(newIndex);

        // update our index
        currentIndex = newIndex;

        // wait for cooldown to end
        yield return new WaitForSeconds(selectionCooldown);

        // allow navigation again
        canNavigate = true;
    }

    //Function to resume interactions
    public void Resume()
    {
        //restores the virtual environment interactions
        raycastInteractionManager.main_menu_is_active = false;
        
        //enables character movement -> set constrained to false
        raycastInteractionManager.movement_is_constrained = false;

        //hide main menu -> set active false
        raycastInteractionManager.mainMenu.SetActive(false);
    }

    //We use this function to toggle between raycast lengths
    public void Change_Raycast_Length()
    {
        //Get the current max distance from the other script
        float currentLength = raycastInteractionManager.maxDistance;

        // toggle between lengths (1, 10, 50)
        if (currentLength == 1.0f)
        {
            //set length to 10m
            newText = "Raycast Length: 10m";
            raycastInteractionManager.maxDistance = 10.0f;
            raycast_length.text = newText;

        }
        else if (currentLength == 10.0f)
        {
            //set length to 50m
            newText = "Raycast Length: 50m";
            raycastInteractionManager.maxDistance = 50.0f;
            raycast_length.text = newText;
        }
        else
        {
            //set length to 1m
            newText = "Raycast Length: 1m";
            raycastInteractionManager.maxDistance = 1.0f;
            raycast_length.text = newText;
        }
    }

    //We use this function to toggle between character speeds
    public void Change_Character_Speed()
    {   
        //Get the current speed from the other script
        float currentSpeed = characterMovement.speed;

        // toggle between speeds (low, medium, high)
        if (currentSpeed == 5.0f)
        {
            //set speed to medium
            newText = "Speed: Medium";
            characterMovement.speed = 10.0f;
            speed.text = newText;
        }
        else if (currentSpeed == 10.0f)
        {
            //set speed to High
            newText = "Speed: High";
            characterMovement.speed = 20.0f;
            speed.text = newText;
        }
        else
        {
            //set speed to low
            newText = "Speed: Low";
            characterMovement.speed = 5.0f;
            speed.text = newText;
        }
    }

    // stops or quits the game
    public void Quit_Game()
    {
        // Quit the application
        #if UNITY_EDITOR
            //Stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //quit app
            Application.Quit();
        #endif
    }

    // function to highlight the color at the current index
    public void HighlightButton(int index)
    {
        // reset all the buttons in the menu to normal color
        foreach (Button button in menuButtons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            button.colors = colors;
        }

        // -highlight the button at current index-

        // Get the button color
        ColorBlock buttonColor = menuButtons[index].colors;

        //change the button color to orange
        Color orange = new Color(1f, 0.5f, 0f, 1f);
        buttonColor.normalColor = orange;
        menuButtons[index].colors = buttonColor;
    }
}
