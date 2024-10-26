using UnityEngine;

namespace MinD.Runtime.Managers {

public class PlayerInputManager : Singleton<PlayerInputManager> {

    private PlayerControls playerControls;

    // LOCOMOTION
    public Vector2 movementInput;
    public bool jumpInput;
    public bool sprintInput;

    // CAMERA CONTROL
    public Vector2 rotationInput;
    public bool lockOnInput;

    // INTERACTION
    public bool interactionInput;

    // COMBAT
    public bool useMagicInput;
    public int swapMagicInput;
        // LEFT MAGIC TO -1, RIGHT MAGIC TO 1
        // IF MAGIC IS SWAPPED, RESET TO 0


    // on scene changed, Check the scene is world scene
    // if scene is not a world scene, disable the input


    private void OnEnable() {

        if (playerControls == null) {

            playerControls = new PlayerControls();
            playerControls.Enable();
            
            // LOCOMOTION
            playerControls.Locomotion.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Locomotion.Jump.performed += i => jumpInput = true; // IF INPUT IS PERFORMED, SET BOOL TO TRUE

            playerControls.Locomotion.Sprint.performed += i => sprintInput = true;
            playerControls.Locomotion.Sprint.canceled += i => sprintInput = false;
            
            
            // CAMERA CONTROL
            playerControls.CameraControl.Rotation.performed += i => rotationInput = i.ReadValue<Vector2>();
            playerControls.CameraControl.LockOn.performed += i => lockOnInput = true; // IF INPUT IS PERFORMED, SET BOOL TO TRUE
            
            
            // INTERACTION
            playerControls.Interaction.Interaction.performed += i => interactionInput = true;
            
            
            // COMBAT
            playerControls.Combat.UseMagic.started += i => useMagicInput = true;
            playerControls.Combat.UseMagic.canceled += i => useMagicInput = false;
            playerControls.Combat.SwapMagic.started += i => swapMagicInput = (int)(i.ReadValue<float>());
            
        }
    }
}

}