using System;
using UnityEngine;

namespace _Project.Scripts.PlayerScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class CrouchController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] PlayerAnimation playerAnimation;
        [SerializeField] PlayerMovement playerMovement;
        [SerializeField] CharacterController characterController;
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] float speedCrouch = 0.5f;

        public int countPressKeyC = 0;
        
        public bool crouchActive = false;
        private Vector3 moveCrouch;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if(drivingPlayer.isInCar == false)
                CheckCrouch();
        }

        public void CheckCrouch()
        {
            if(countPressKeyC == 1 && crouchActive == true) // dont touch this
                Crouch();
            
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                switch (countPressKeyC)
                {
                    case 0:
                        countPressKeyC = 1;
                        CrouchActivate();
                        break;

                    case 1:
                        countPressKeyC = 0;
                        CrouchDeactivate();
                        break;

                }
            }
        }

        public void CrouchActivate()
        {
            animator.SetBool("isCrouching", true);
            crouchActive = true;

            Crouch();
        }

        public void CrouchDeactivate()
        {
            animator.SetBool("isCrouching", false);
            crouchActive = false;
        }
        
        public void Crouch()
        {
            moveCrouch = transform.TransformDirection(playerMovement.inputKeyboard.x, 0f, playerMovement.inputKeyboard.z);

            playerAnimation.ChangeAnimationCrouch(playerMovement.inputKeyboard.z, playerMovement.inputKeyboard.x, crouchActive);

            characterController.Move(moveCrouch * speedCrouch * Time.deltaTime);

        }

    }
}
