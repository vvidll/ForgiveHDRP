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
        [SerializeField] AudioMoveManager audioManager;
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] PlayerAnimation playerAnimation;

        [SerializeField] CharacterController characterController;
        [SerializeField] float speedWalk = 2f;
        [SerializeField] float speedRun = 3f;

        [HideInInspector]
        public Vector3 MovePlayer;

        [HideInInspector]
        public Vector3 inputKeyboard;

        [HideInInspector]
        public bool IsKeyPressLeftShift = false;

        [HideInInspector]
        public bool isHasKeyboardInput = false; 
        
        [HideInInspector]
        public bool isHasWalking = false;
        
        [HideInInspector]
        public bool isHasRunning = false;

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
            if(crouchController.countPressKeyC == 0 && crouchController.crouchActive == false) // dont touch this
                Walk();
                
            IsKeyPressLeftShift = false;

            if (Input.GetKey(KeyCode.W)) // here all working!!!
            {
                if (Input.GetKey(KeyCode.LeftShift) 
                    && IsKeyPressLeftShift == false 
                    && crouchController.crouchActive == false 
                    && staminaSliderController.endStamina == false
                    && isHasWalking == true)
                {
                    isHasWalking = false;

                    //audioManager.StopAudioForGrassWalk();

                    IsKeyPressLeftShift = true;
                    Run();
                }

                if (staminaSliderController.endStamina == true 
                    && isHasRunning == true)
                {
                    isHasRunning = false;

                    Walk();
                }


                if (Input.GetKeyUp(KeyCode.LeftShift)) // is working!!!
                    IsKeyPressLeftShift = false; // Stop running
 
            }
        }

        void InputKeyboard()
        {
            isHasKeyboardInput = true;
            inputKeyboard = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        }

        public void Walk()
        {
            isHasWalking = true;

            InputKeyboard();

            MovePlayer = transform.TransformDirection(inputKeyboard.x, 0f, inputKeyboard.z);

            characterController.Move(MovePlayer * speedWalk * Time.deltaTime);

            playerAnimation.ChangeAnimationWalk(inputKeyboard.z, inputKeyboard.x);

            staminaSliderController.IncreasedStaminaWalk();

            //audioManager.PlayAudioForGrassWalk();
        }

        public void Run()
        {
            isHasRunning = true;

            MovePlayer = transform.TransformDirection(inputKeyboard.x, 0f, inputKeyboard.z);

            characterController.Move(MovePlayer * speedRun * Time.deltaTime);

            playerAnimation.ChangeAnimationRun(inputKeyboard.z);

            staminaSliderController.DecreasedStamina();

            //audioManager.PlayAudioForGrassRun();
        }

    }

}
