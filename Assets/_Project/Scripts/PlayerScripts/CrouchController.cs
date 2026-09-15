using System;
using UnityEngine;

namespace _Project.Scripts.PlayerScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class CrouchController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        //[SerializeField] PlayerMovement playerMovement;
        [SerializeField] CharacterController characterController;
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] float speedCrouch = 0.5f;

        public int countPressKeyC = 0;
        
        [SerializeField] float smoothTimeForCrouchWalking = 0.15f;

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
        }

        public void CrouchDeactivate()
        {
            animator.SetBool("isCrouching", false);
            crouchActive = false;
        }

        public void Crouch() 
        {
            float horizontalCrouch = Input.GetAxisRaw("Horizontal");
            float verticalCrouch = Input.GetAxisRaw("Vertical");
            
            moveCrouch = transform.TransformDirection(horizontalCrouch, 0f, verticalCrouch);
            
            if (verticalCrouch != 0 || horizontalCrouch != 0 && Input.GetKeyDown(KeyCode.C)) // activate crouch, when player walking
                animator.SetFloat("xCrouch", 0f, smoothTimeForCrouchWalking,  Time.deltaTime);

            if (verticalCrouch == 0 || horizontalCrouch == 0 && crouchActive == true) // activate crouch "idle" animation, when player stay
                                                                                                        // on the place, but button C is press
                animator.SetFloat("xCrouch", 0f, smoothTimeForCrouchWalking,  Time.deltaTime);

            if (verticalCrouch != 0 || horizontalCrouch != 0 && crouchActive == true) // activate crouch walk 
                CrouchWalk();
        }
        
        public void CrouchWalk()
        {
            characterController.Move(moveCrouch * speedCrouch * Time.deltaTime);   
            animator.SetFloat("xCrouch", 1f, smoothTimeForCrouchWalking,  Time.deltaTime);
        }

    }
}
