using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using _Project.Scripts.InventoryScripts;
using _Project.Scripts.MissionsScripts;
using _Project.Scripts.OdinSerializerSavesAndLoads;
using _Project.Scripts.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Scripts.InteractScripts.CarInteractScripts
{
    // скрипт отвечающий за взаимодействие с дверью машины
    public class InteractionDoorCar : MonoBehaviour, IInteractable
    {
        [SerializeField] OdinSerializerControllerSaveAndLoad odinSerializerControllerSaveAndLoad;

        [SerializeField] TMP_Text textInteractionDoorCar;

        [Header("Layer Mask Object")]
        [SerializeField] GameObject leftDoorObject;

        [Header("Cinemachine")]
        [SerializeField] GameObject cinemachineCamera;

        [Header("RigBuilder")]
        [SerializeField] RigBuilder rigBuilder;

        [Header("Animator")]
        [SerializeField] Animator playerAnimator;
        [SerializeField] Animator carDoorAnimator;

        [Header("References From Other Classes")]
        [SerializeField] TransitionsController transitionsController;
        [SerializeField] CheckCompleteTasksNotepad checkCompleteTasksNotepad;
        [SerializeField] DialogueWithSaler dialogueWithSaler;
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] ShowMissionsManager showMissionsManager;

        [Header("Parent Object")]
        public Transform carParentObject; 

        [Header("Child Object")]
        [SerializeField] Transform player;

        int RigDoorHandleTriggerForExitingCar = 5;
        int RigDoorHandleTriggerForEnteringCar = 6;

        //[HideInInspector]
        //public bool isParentCar = true;

        private void Update()
        {
            if (drivingPlayer.isInCar == true 
                && showMissionsManager.isNeedCarCheck == false 
                && leftDoorObject.layer == LayerMask.NameToLayer("Interaction"))
                leftDoorObject.layer = LayerMask.NameToLayer("Default");

        }

        public void Interact()
        {
            if (checkCompleteTasksNotepad.completeMissionItemsForSurvival == true 
                && dialogueWithSaler.hasBoughtItemsInShop == true 
                && drivingPlayer.isHoldKeyF == 1) 
            {
                Debug.Log("Interact with door the car");

                drivingPlayer.isHoldKeyF = 0;

                drivingPlayer.isInCar = true;

                playerAnimator.SetTrigger("isEnteringCar");

                cinemachineCamera.SetActive(false);
                rigBuilder.layers[drivingPlayer.rigHeadIndex].active = false;

                // RigBuilder IK

                // временно отключил IK
                //rigBuilder.layers[RigDoorHandleTriggerForExitingCar].active = false;
                //rigBuilder.layers[RigDoorHandleTriggerForEnteringCar].active = true;

                //--------------

                carDoorAnimator.SetBool("isOpenAndCloseDoor", true);

                transitionsController.SitCarCoroutine().Forget();

                // set car how parent object
                drivingPlayer.isParentDinamicObjects = false;
                //isParentCar = false; // set false(for odin serializer) for choise him how parent object

                //Debug.Log("isParentCar: " + isParentCar);
            }


        }

        public void Description()
        {
            textInteractionDoorCar.text = "sit in the car";
            //return textInteractionDoorCar.text;
        }

        public void CloseDoor() =>
            carDoorAnimator.SetBool("isOpenAndCloseDoor", false);
        
    }


}