using _Project.Scripts.MissionsScripts;
using UnityEngine;

namespace _Project.Scripts.PlacedItemsScripts
{
    public class SleepingBagController : PlacedInteractionItemsManager, IInteractableBuildings
    {
        [SerializeField] AudioCarController audioCarController;

        [SerializeField] TransitionsController transitionsController;
        [SerializeField] ShowMissionsManager showMissionsManager;

        [HideInInspector]
        public bool isChangeTimeDay = false; // поменялось время суток (нет/да)

        private void Update()
        {
            if (buildingManager.nameItemForBuilding == "SleepingBag" && buildingManager.isHasPlacedItem == true && isChangeTimeDay == false)
            {
                showMissionsManager.ShowMissionGoSleep();
            }
        }

        public void InteractBuildings()
        {
            transitionsController.StartCoroutine(transitionsController.ChangeTimeDayToNightCoroutine());
            playerInteraction.isShowDescription = false;

            isChangeTimeDay = true;

            audioCarController.ActivateAudio();
            showMissionsManager.ShowMissionCheckCar();

            if (playerInteraction.isShowDescription == false)
                DisableLayer();
        }

        public void DescriptionBuildings()
        {
            if (playerInteraction.isShowDescription == true)
                textInteraction.text = "sleep";
        }


    }
}