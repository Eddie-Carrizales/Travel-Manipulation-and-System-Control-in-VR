using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController charCntrl;
    [Tooltip("The speed at which the character will move.")]
    public float speed;
    [Tooltip("The camera representing where the character is looking.")]
    public GameObject cameraObj;
    [Tooltip("Should be checked if using the Bluetooth Controller to move. If using keyboard, leave this unchecked.")]
    public bool joyStickMode;

    public RaycastInteractionManager raycastInteractionManager;

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
        speed = 5.0f;
    }

    // Update is called once per frames
    void Update()
    {
        bool movement_is_constrained = raycastInteractionManager.movement_is_constrained;
        bool main_menu_is_active = raycastInteractionManager.main_menu_is_active;

        float horComp = 0f;
        float vertComp = 0f;

        //if main menu is not active, look around normaly, else it stays locked
        if (!main_menu_is_active)
        {
            //Get horizontal and Vertical movements
            horComp = Input.GetAxis("Horizontal");
            vertComp = Input.GetAxis("Vertical");
        }

        if (joyStickMode)
        {
            horComp = Input.GetAxis("Vertical");
            vertComp = Input.GetAxis("Horizontal") * -1;
        }

        //movement vector
        Vector3 moveVect = Vector3.zero;
        
        //Get look Direction
        Vector3 cameraLook = cameraObj.transform.forward;
        cameraLook.y = 0f;
        cameraLook = cameraLook.normalized;

        Vector3 forwardVect = cameraLook;
        Vector3 rightVect = Vector3.Cross(forwardVect, Vector3.up).normalized * -1;

        //if the movement is not constrained, then allow movement, else, then it will be zero.
        if (!movement_is_constrained)
        {
            //allow movement
            moveVect += rightVect * horComp;
            moveVect += forwardVect * vertComp;

            moveVect *= speed;
        }

        charCntrl.SimpleMove(moveVect);

    }
}
