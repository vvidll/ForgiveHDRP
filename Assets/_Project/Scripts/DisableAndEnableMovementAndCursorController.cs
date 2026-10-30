using _Project.Scripts.CameraScripts;
using _Project.Scripts.InterfaceScripts;
using _Project.Scripts.NotepadScripts;
using _Project.Scripts.PlayerScripts;
using UnityEngine;

namespace _Project.Scripts
{
    public class DisableAndEnableMovementAndCursorController : MonoCache
    {
        [SerializeField] PlayerMovement playerMovement;
        [SerializeField] CameraController cameraController;
        [SerializeField] CrouchController crouchController;
        [SerializeField] JumpController jumpController;
        [SerializeField] NotepadController notepadController;
        [SerializeField] StaminaSliderController staminaSliderController;

        [HideInInspector]
        public bool isGameMenuActive = false;
        [HideInInspector]
        public bool isDialogueWithSalerActive = false;

        public void EnableMovementAndHideCursor()
        {
            if (isGameMenuActive == false) 
            {
                Time.timeScale = 1;
                EnableScripts();
            }

            if (isDialogueWithSalerActive == false)
                EnableScripts();

            HideCursor();
        }

        public void DisableMovementAndShowCursor()
        {
            if (isGameMenuActive == true) 
            {
                DisableScripts();
                Time.timeScale = 0;
            }     

            if (isDialogueWithSalerActive == true) 
                DisableScripts();
            

            ShowCursor();
        }

        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void DisableScripts() 
        {
            playerMovement.enabled = false;
            cameraController.enabled = false;
            crouchController.enabled = false;
            jumpController.enabled = false;
            notepadController.enabled = false;
            staminaSliderController.enabled = false;
        }

        public void EnableScripts() 
        {
            playerMovement.enabled = true;
            cameraController.enabled = true;
            crouchController.enabled = true;
            jumpController.enabled = true;
            notepadController.enabled = true;
            staminaSliderController.enabled = true;
        }
    }
}
