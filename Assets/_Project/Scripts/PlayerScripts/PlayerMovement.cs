using _Project.Scripts.InterfaceScripts;
using _Project.Scripts.AudioScripts;
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

        public float minSpeed = 2f, maxSpeed = 5f;
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
                    && staminaSliderController.endStamina == true)
                {
                    maxSpeed = minSpeed;
                    currentSpeed = maxSpeed;

                    IsKeyPressLeftShift = false;
                    Walk();
                }

                else if (Input.GetKey(KeyCode.LeftShift) 
                    && IsKeyPressLeftShift == false
                    && crouchController.crouchActive == false 
                    && staminaSliderController.endStamina == false)
                {
                    maxSpeed = 5f;
                    currentSpeed = maxSpeed;

                    IsKeyPressLeftShift = true;
                    Run();
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
            currentSpeed = minSpeed;

            MovePlayer = transform.TransformDirection(inputKeyboard.x, 0f, inputKeyboard.z);

            characterController.Move(MovePlayer * currentSpeed * Time.deltaTime);

            playerAnimation.ChangeAnimationWalk(inputKeyboard.z, inputKeyboard.x);

            staminaSliderController.IncreasedStaminaWalk();
        }

        public void Run()
        {
            currentSpeed = maxSpeed;

            MovePlayer = transform.TransformDirection(inputKeyboard.x, 0f, inputKeyboard.z);

            characterController.Move(MovePlayer * currentSpeed * Time.deltaTime);

            playerAnimation.ChangeAnimationRun(inputKeyboard.z);

            staminaSliderController.DecreasedStamina();

        }

    }

}
