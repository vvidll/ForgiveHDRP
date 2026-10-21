using _Project.Scripts.InterfaceScripts;
using UnityEngine;

namespace _Project.Scripts.PlayerScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] CrouchController crouchController;
        [SerializeField] StaminaSliderController staminaSliderController;
        //[SerializeField] TerrainLayersController terrainLayersController;
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] PlayerAnimation playerAnimation;

        [SerializeField] CharacterController characterController;
        //[SerializeField] float speedWalk = 2f;
        //[SerializeField] float speedRun = 5f;

        public float walkSpeed = 2f, runSpeed = 5f, runAdrenalineSpeed = 7f;
        public float currentSpeed = 0;

        [HideInInspector]
        public Vector3 MovePlayer;

        [HideInInspector]
        public Vector3 inputKeyboard;

        [HideInInspector]
        public bool IsKeyPressLeftShift = false;

        [HideInInspector]
        public bool isHasKeyboardInput = false; 
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if(drivingPlayer.isInCar == false)
                CheckMovementWalkAndRun();
        }

        public void CheckMovementWalkAndRun()
        {
            InputKeyboard();

            if (crouchController.countPressKeyC == 0 && crouchController.crouchActive == false) // dont touch this
                Walk();
                
            IsKeyPressLeftShift = false;

            if (Input.GetKey(KeyCode.W)) // here all working!!!
            {
                if (Input.GetKey(KeyCode.LeftShift)
                    && IsKeyPressLeftShift == true
                    && crouchController.crouchActive == false
                    && staminaSliderController.endStamina == true
                    && staminaSliderController.isHasAdrenaline == false)
                {
                    IsKeyPressLeftShift = false;
                    Walk();
                }

                else if (Input.GetKey(KeyCode.LeftShift)
                    && IsKeyPressLeftShift == false
                    && crouchController.crouchActive == false
                    && staminaSliderController.endStamina == false
                    && staminaSliderController.isHasAdrenaline == false)
                {
                    IsKeyPressLeftShift = true;
                    Run();
                }

                else if (Input.GetKey(KeyCode.LeftShift)
                    && staminaSliderController.isHasAdrenaline == true)
                {
                    RunAdrenaline();
                }

            }

            if (Input.GetKeyUp(KeyCode.LeftShift)) // is working!!!                                                
            {
                IsKeyPressLeftShift = false; // Stop running
                Walk();
            }
        }

        void InputKeyboard()
        {
            isHasKeyboardInput = true;
            inputKeyboard = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        }

        public void Walk()
        {
            Movement(currentSpeed, walkSpeed);

            playerAnimation.ChangeAnimationWalk(inputKeyboard.z, inputKeyboard.x);

            staminaSliderController.IncreasedStaminaWalk();
        }

        public void Run()
        {
            Movement(currentSpeed, runSpeed);

            playerAnimation.ChangeAnimationRun(inputKeyboard.z);

            staminaSliderController.DecreasedStamina();

        }

        public void RunAdrenaline() 
        {
            Movement(currentSpeed, runAdrenalineSpeed);

            playerAnimation.ChangeAnimationAdrenaline();

        }

        void Movement(float currentSpeed, float speedMovement) 
        {
            currentSpeed = speedMovement;

            MovePlayer = transform.TransformDirection(inputKeyboard.x, 0f, inputKeyboard.z);

            characterController.Move(MovePlayer * currentSpeed * Time.deltaTime);
        }

    }

}
