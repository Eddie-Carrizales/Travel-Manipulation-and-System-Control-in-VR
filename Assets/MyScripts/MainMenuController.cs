using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{   
    //text components
    public TMP_Text raycast_length;
    public TMP_Text speed;
    public string newText = "";

    public Button[] menuButtons;
    public int currentIndex = 0;
    private float previousVerticalInput = 0f;
    public float inputThreshold = 0.3f; // Threshold value for considering input as zero

    public RaycastInteractionManager raycastInteractionManager;
    public CharacterMovement characterMovement;

    //For MAC
    private string B_Button = "js10";

    //FOR ANDROID
    //private string B_Button = "js10";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (raycastInteractionManager.main_menu_is_active)
        {
            float verticalInput = Input.GetAxis("Vertical");

            // Check if vertical input has returned to zero from a previous non-zero value
            if (Mathf.Abs(previousVerticalInput) > inputThreshold && Mathf.Abs(verticalInput) <= inputThreshold)
            {
                previousVerticalInput = 0f; // Reset previous input value
                return; // Exit Update without further handling input
            }

            // If vertical input is positive or negative and the previous input was zero
            if (verticalInput > inputThreshold && Mathf.Abs(previousVerticalInput) <= inputThreshold)
            {
                // Move selection up
                currentIndex = (currentIndex == 0) ? menuButtons.Length - 1 : currentIndex - 1;
                HighlightButton(currentIndex);
                previousVerticalInput = verticalInput; // Set previous input to current input
            }
            else if (verticalInput < -inputThreshold && Mathf.Abs(previousVerticalInput) <= inputThreshold)
            {
                // Move selection down
                currentIndex = (currentIndex == menuButtons.Length - 1) ? 0 : currentIndex + 1;
                HighlightButton(currentIndex);
                previousVerticalInput = verticalInput; // Set previous input to current input
            }

            // Check for button press (you can use any button here, like "Submit")
            if (Input.GetButtonDown(B_Button))
            {
                // Perform action associated with the selected button
                menuButtons[currentIndex].onClick.Invoke();
            }

        }
    }

    public void Resume()
    {
        //restores virtual environment interactions -> we can add a bool to raycast if conditions so that raycast cannot do anything
        raycastInteractionManager.main_menu_is_active = false;
        
        //enables character movement -> change bool
        raycastInteractionManager.movement_is_constrained = false;

        //hide main menu -> set active false
        raycastInteractionManager.mainMenu.SetActive(false);
    }

    public void Change_Raycast_Length()
    {
        float currentLength = raycastInteractionManager.maxDistance;

        // Toggle between length options
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

    public void Change_Character_Speed()
    {
        float currentSpeed = characterMovement.speed;

        // Toggle between speed options
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

    public void Quit_Game()
    {

    }

    public void HighlightButton(int index)
    {
        // Reset all buttons to normal state
        foreach (Button button in menuButtons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            button.colors = colors;
        }

        // Highlight the selected button
        ColorBlock selectedColors = menuButtons[index].colors;
        selectedColors.normalColor = Color.yellow; // Change this color as needed
        menuButtons[index].colors = selectedColors;
    }
}
