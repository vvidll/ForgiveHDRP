using _Project.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Scripts.ProjectSettingsScripts
{
    public class SettingsAnimationCarAndPlayerPositionStartGame : MonoCache
    {
        [Header("Before change position and rotation player\n" +
            "YOU NEED SAVE DATA In <ContextMenu> -> three points")]

        [Header("Through <ContextMenu> -> three points on this script\n " +
            "< ON > Start Animation and Player position enable in Play Mode\n " +
            "< OFF > Start Animation and Player position before Start Game")]

        [Header("Animator")]
        [SerializeField] Animator playerAnimator;

        [Header("Car Animator")]
        public Animator carAnimator;

        [Header("Set Parent For Player")]
        public Transform player;
        public Transform parent;
        [SerializeField] Transform parentCar;

        [Header("Start Point")]
        [SerializeField] Transform pointStartGame;

        [Header("Teleport Player In Car")]
        [SerializeField] Transform pointTeleportPlayerInCar;

        [Header("RigBuilder")]
        [SerializeField] RigBuilder rigBuilder;

        [Header("Driving Player")]
        [SerializeField] DrivingPlayer drivingPlayer;

        [HideInInspector]
        public Vector3 positionCarAfterParking = new Vector3(96.56f, 18.742f, 177.81f);
        [HideInInspector]
        public Quaternion rotationCarAfterParking = new Quaternion(0f, 90.287f, 0f, 0f);

        Vector3 currentPositionPlayer;
        Quaternion currentRotationPlayer;

        Vector3 currentPositionCar;
        Quaternion currentRotationCar;

        int isSaveRotationAndPosition = 0;

        const string SaveRotationAndPositionKey = "Save_Rotation_And_Position";

        public bool isEnoughPlayInitialAnimation = false;

        //[HideInInspector]
        public bool isFirstLaunchGame = false;

        public bool isParking = false;

        private void Awake()
        {
            if (isSaveRotationAndPosition == 0)
                Debug.LogError("SAVE THE DATA IN Script: --GAME_MANAGER-- In Hierarchy -> 'SettingsAnimationCarAndPlayerPositionStartGame' -> 'three points ON the script' -> 'SAVE Player position and Rotation'\n" +
                    "also for initial launch the game, you need ON Initil Animation for Car and Player");
        }

        public override void OnTick()
        {
            FirstLaunchGame();
        }

        public void FirstLaunchGame()
        {
            // first start game (first scene) before first checkpoint
            if (isFirstLaunchGame == true && isEnoughPlayInitialAnimation == false)
            {
                Debug.Log("call if");
                carAnimator.SetTrigger("isDrivingStore");
                playerAnimator.SetTrigger("isDrivingStartGame");

                playerAnimator.SetTrigger("StartTrigger");
                carAnimator.SetTrigger("StartTrigger");

                isEnoughPlayInitialAnimation = true;
            }

            if (isFirstLaunchGame == false && isEnoughPlayInitialAnimation == false)
            {
                Debug.Log("call else");

                isEnoughPlayInitialAnimation = true;
            }

            if (drivingPlayer.isInCar == true && isEnoughPlayInitialAnimation == false)
            {
                player.SetParent(parentCar);
                isEnoughPlayInitialAnimation = true;
            }

            if (drivingPlayer.isInCar == false && isEnoughPlayInitialAnimation == false)
            {
                player.SetParent(parent);
                isEnoughPlayInitialAnimation = true;
            }
        }

        [ContextMenu("LOAD Player/Car position and Rotation")]
        void LoadPositionAndRotationPlayer()
        {
            if (PlayerPrefs.HasKey(SaveRotationAndPositionKey))
            {
                isSaveRotationAndPosition = PlayerPrefs.GetInt(SaveRotationAndPositionKey);

                player.position = currentPositionPlayer;
                player.rotation = currentRotationPlayer;

                parentCar.position = currentPositionCar;
                parentCar.rotation = currentRotationCar;

                isSaveRotationAndPosition = 0;
            }
        }

        [ContextMenu("SAVE Player/Car position and Rotation")]
        void SavePositionAndRotationPlayer()
        {
            currentPositionPlayer = player.position;
            currentRotationPlayer = player.rotation;

            currentPositionCar = parentCar.position;
            currentRotationCar = parentCar.rotation;

            isSaveRotationAndPosition = 1;

            PlayerPrefs.SetInt(SaveRotationAndPositionKey, isSaveRotationAndPosition);
        }

        [ContextMenu("ON Initial Car and Player position")]
        void EnableInitialTransformBeforeStartGame()
        {
            // start the game -> on in play mode for animation work

            parentCar.gameObject.SetActive(true);

            parentCar.position = pointStartGame.position;

            parentCar.position = pointStartGame.position;
            parentCar.rotation = new Quaternion(0f, 0f, 0f, 1f);

            drivingPlayer.isInCar = true;

            if (drivingPlayer.isInCar == true)
                player.SetParent(parentCar);

            player.position = pointTeleportPlayerInCar.position;
            player.rotation = new Quaternion(0f, .7f, 0f, .7f);


            for (int i = 1; i < rigBuilder.layers.Count && i < 5; i++)
            {
                rigBuilder.layers[i].active = true;
            }
        }

        [ContextMenu("OFF Initial Car and Player position")]
        void DisableInitialTransformBeforeStartGame()
        {
            drivingPlayer.isInCar = false;

            if (drivingPlayer.isInCar == false)
                player.SetParent(parent);

            LoadPositionAndRotationPlayer();

            for (int i = 1; i < rigBuilder.layers.Count && i < 5; i++)
            {
                rigBuilder.layers[i].active = false;
            }

            OffInitialAnimation();
        }

        [ContextMenu("ON Initial Animation Car and Player")]
        void OnInitialAnimations()
        {
            isFirstLaunchGame = true;
            isParking = false;
        }
        [ContextMenu("OFF Initial Animation Car and Player")]
        public void OffInitialAnimation()
        {
            isFirstLaunchGame = false;
            isParking = true;

            carAnimator.enabled = false;
        }
    }
}