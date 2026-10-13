using System.Collections;
using _Project.Scripts.CameraScripts;
using _Project.Scripts.InteractScripts.CarInteractScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

namespace _Project.Scripts.PlayerScripts
{
    public class DrivingPlayer : MonoBehaviour
    {
        public const string HoldKeyF = "hold_key_f";
        public const string RigBuilderListCountKey = "rig_builder_list_count";

        [Header("Camera")]
        [SerializeField] Camera mainCamera;

        [Header("Character Controller")]
        [SerializeField] CharacterController characterController;

        [Header("Cinemachine")]
        [SerializeField] GameObject cinemachineCamera;

        [Header("Colliders")]
        [SerializeField] BoxCollider carCollider;

        [Header("Animator")]
        [SerializeField] Animator playerAnimator;
        [SerializeField] Animator carDoorAnimator;

        [Header("Player Transform")]
        [SerializeField] Transform playerTransform;

        [Header("Image Interact 'Hold'")]
        public Image imageInteractHold;

        [Header("RigBuilder <<IK>> ")]
        [SerializeField] RigBuilder rigBuilder;

        int RigDoorHandleTriggerForExitingCar = 5;
        int RigDoorHandleTriggerForEnteringCar = 6;

        [Header("Collider Left Door Car")]
        [SerializeField] SphereCollider sphereColliderLeftDoor;


        [Header("Parent Object")]
        [SerializeField] Transform parentObject;

        [Header("Child Object")]
        [SerializeField] Transform player;

        //[HideInInspector]
        public bool isInCar = false;
        [HideInInspector]
        public int isHoldKeyF = 0; // false
        [HideInInspector]
        public int rigBuilderCount = 0;

        [HideInInspector]
        public int rigHeadIndex = 0;

        [Header("References From Other Classes")]
        [SerializeField] InteractionDoorCar interactionDoorCar;
        [SerializeField] CameraController cameraController;

        Vector3 newRotationPlayer = new Vector3(0f, 90f, 0f);

        Quaternion rotationZero = new Quaternion(0f, 0f, 0f, 0f);

        bool isMovePlayerForEventAnimation = false;
        bool isStopPlayerForEventAnimation = false;


        private void Start()
        {
            imageInteractHold.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (isMovePlayerForEventAnimation == true
                && isStopPlayerForEventAnimation == false)
            {
                // проблема во второй локации в том что игрок начинает движение по глобальным координатам,
                // А должен по локальным!!!
                characterController.Move(Vector3.right * .3f * Time.deltaTime);

            }

            if (Input.GetKey(KeyCode.F) && imageInteractHold.fillAmount < 1 && imageInteractHold.gameObject.activeSelf == true
            && isHoldKeyF == 0)
            {
                imageInteractHold.fillAmount += 0.4f * Time.deltaTime;

                imageInteractHold.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 6f);


                if (imageInteractHold.fillAmount == 1)
                {
                    imageInteractHold.gameObject.SetActive(false);

                    ExitingCar();

                    isHoldKeyF = 1; // hold key end

                }

            }

            else if (imageInteractHold.fillAmount < 1 && imageInteractHold.gameObject.activeSelf == true)
            {
                imageInteractHold.fillAmount -= 0.2f * Time.deltaTime;

                imageInteractHold.transform.DOScale(new Vector3(1f, 1f, 1f), 6f);
            }
        }

        public void ExitingCar()
        {
            carCollider.isTrigger = true;

            mainCamera.transform.localRotation = rotationZero;

            cinemachineCamera.SetActive(false);

            for (int i = 0; i < rigBuilder.layers.Count; i++)
            {
                rigBuilder.layers[i].active = false;
            }



            // RigBuilder IK

            // временно отключил IK
            //rigBuilder.layers[RigDoorHandleTriggerForExitingCar].active = true;
            //rigBuilder.layers[RigDoorHandleTriggerForEnteringCar].active = false;

            //--------------

            isInCar = false;

            cameraController.ResetCameraRotation();

            carDoorAnimator.SetBool("isOpenAndCloseDoor", true);

            playerAnimator.SetTrigger("isExitingCar");

            player.SetParent(parentObject);

            StartCoroutine(EndPlayAnimationExitingCarCoroutine());

        }

        IEnumerator EndPlayAnimationExitingCarCoroutine()
        {
            yield return new WaitForSeconds(7);

            this.transform.localEulerAngles = newRotationPlayer; // for right rotate player after the end animation "exit from car"

            playerAnimator.SetTrigger("isIdle");

            imageInteractHold.fillAmount = 0;

            carDoorAnimator.SetBool("isOpenAndCloseDoor", false);

            rigBuilder.layers[rigHeadIndex].active = true;

            cinemachineCamera.SetActive(true);

            yield return new WaitForSeconds(2);
            carCollider.isTrigger = false;
        }

        [ContextMenu("Reset Key Driver (Польз.)")]
        public void DeleteKeys()
        {
            PlayerPrefs.DeleteKey(HoldKeyF);
            PlayerPrefs.DeleteKey(RigBuilderListCountKey);

            Debug.Log("Удаление ключей Водителя");
        }

        public void MovePlayerEvent()
        {
            isMovePlayerForEventAnimation = true;

            isStopPlayerForEventAnimation = false;
        }

        public void StopPlayerEvent() 
        {
            isMovePlayerForEventAnimation = false;

            isStopPlayerForEventAnimation = true;
        }

    }
}