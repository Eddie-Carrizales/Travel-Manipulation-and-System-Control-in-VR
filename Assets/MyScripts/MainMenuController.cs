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
    public int currentIndex = 0;
    private float previousVerticalInput = 0f;
    public float inputThreshold = 0.3f; // Threshold value for considering input as zero

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
            
            //---Here I made it so that joystick functions as a D-pad--

            // This if just checks if the input has come back to its original position, before considering more input
            if (Mathf.Abs(previousVerticalInput) > inputThreshold && Mathf.Abs(verticalInput) <= inputThreshold)
            {   
                //reset the previous
                previousVerticalInput = 0f;
                return;
            }

            // --These conditions check if joystick input is positive or negative (for up and down)--

            // If vertical input is positive and previous was 0 (Note also added a thershold so that its more sensitive and doenst have to go back exactly to 0)
            if (verticalInput > inputThreshold && Mathf.Abs(previousVerticalInput) <= inputThreshold)
            {
                // moves to the button above
                if (currentIndex == 0)
                {
                    currentIndex = menuButtons.Length - 1;
                }
                else
                {
                    currentIndex--;
                }

                //Highlight the current button
                HighlightButton(currentIndex); 
                previousVerticalInput = verticalInput; // set previous to our current input
            }
            // If vertical input is negative and previous was 0 (Note also added a thershold so that its more sensitive and doenst have to go back exactly to 0)
            else if (verticalInput < -inputThreshold && Mathf.Abs(previousVerticalInput) <= inputThreshold)
            {
                // moves to the button below
                if (currentIndex == menuButtons.Length - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }

                //Highlight the current button
                HighlightButton(currentIndex);
                previousVerticalInput = verticalInput; // set previous to our current input
            }

            // We use the B button to select
            if (Input.GetButtonDown(B_Button))
            {
                menuButtons[currentIndex].onClick.Invoke(); // clicks the current button
            }

        }
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

        //change the button color
        buttonColor.normalColor = Color.blue;
        menuButtons[index].colors = buttonColor;
    }
}
