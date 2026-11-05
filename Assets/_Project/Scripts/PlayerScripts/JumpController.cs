using System;
using System.Collections;
using _Project.Scripts.AudioScripts;
using _Project.Scripts.InterfaceScripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.PlayerScripts
{
    public class JumpController : GravityController
    {
        [SerializeField] StaminaSliderController staminaSliderController;
        [SerializeField] AudioMoveManager audioManager;
        [SerializeField] DrivingPlayer drivingPlayer;
        
        [SerializeField] Animator animator;
        [SerializeField] float jumpForce = 2f;

        int _isJumping = 0; // 1 - run without jump, 2 - run with jump

        public int countPressSpace = 0;

        private void Update()
        {
            if (drivingPlayer.isInCar == false) 
            {
                CheckJumping();
            }

        }

        public void CheckJumping()
        {
            CheckGravity();

            if (IsGrounded == true && staminaSliderController.sliderStamina.value > 50)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    switch (countPressSpace) 
                    {
                        case 0:
                            countPressSpace = 1;
                            _isJumping = 1; // 1 - run without jump
                            animator.SetBool("isJumping", true);

                            audioManager.PlayAudioForGrassStartJump();

                            Jump();
                            break;

                        case 1:
                            countPressSpace = 2; // fix two-jump, when player quickly press to space key
                            break;
                    }

                }
            }
        }

        public void Jump()
        {
            try 
            { 
                Velocity.y += Mathf.Sqrt(jumpForce * -2f * Gravity);

                staminaSliderController.DecreasedStaminaFromJump();
                StopAnimateJumping().Forget();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        async UniTaskVoid StopAnimateJumping()
        {
            try 
            { 
                await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());

                if (_isJumping == 1)
                    animator.SetBool("isJumping", false);

                audioManager.PlayAudioForGrassEndJump();

                countPressSpace = 0;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}