using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using _Project.Scripts.InventoryScripts;
using _Project.Scripts.MissionsScripts;
using _Project.Scripts.PlayerScripts;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.InteractScripts.CarInteractScripts
{
    // скрипт отвечающий за взаимодействие с дверью машины
    public class InteractionDoorCar : MonoBehaviour, IInteractable
    {
        [SerializeField] TMP_Text textInteractionDoorCar;

        [Header("Animator")]
        [SerializeField] Animator playerAnimator;
        [SerializeField] Animator carDoorAnimator;

        [Header("References From Other Classes")]
        [SerializeField] TransitionsController transitionsController;
        [SerializeField] CheckCompleteTasksNotepad checkCompleteTasksNotepad;
        [SerializeField] DialogueWithSaler dialogueWithSaler;
        [SerializeField] DrivingPlayer drivingPlayer;

        [Header("Parent Object")]
        [SerializeField] Transform carParentObject; 

        [Header("Child Object")]
        [SerializeField] Transform player;

        public void Interact()
        {
            if (checkCompleteTasksNotepad.completeMissionItemsForSurvival == true 
                && dialogueWithSaler.hasBoughtItemsInShop == true 
                && drivingPlayer.isHoldKeyF == 1) 
            {
                drivingPlayer.isHoldKeyF = 0;

                drivingPlayer.isInCar = true;

                playerAnimator.SetTrigger("isEnteringCar");

                player.SetParent(carParentObject); // set car how parent object

                carDoorAnimator.SetBool("isOpenAndCloseDoor", true);

                transitionsController.TransitionTeleportCar();
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