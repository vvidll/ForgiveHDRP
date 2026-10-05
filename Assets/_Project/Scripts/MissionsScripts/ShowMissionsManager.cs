using _Project.Scripts.CarScripts;
using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using _Project.Scripts.InventoryScripts;
using _Project.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.MissionsScripts
{
    public class ShowMissionsManager : MonoCache
    {
        [Header("References From Other Classes")]
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] CheckCompleteTasksNotepad checkCompleteTasksNotepad;
        [SerializeField] DialogueWithSaler dialogueWithSaler;
        [SerializeField] TeleportCar teleportCar;
        [SerializeField] BuildingManager buildingManagerTent;

        // -- Mission For Buy Items For Survival --
        [Header("Window Mission For Buy Items For Survival")]
        [SerializeField] GameObject windowMissionBuyItemsForSurvival;

        // --------------------------------

        // -- Mission Set In The Car --
        [Header("Window Mission Sit In The Car")]
        [SerializeField] GameObject windowMissionSitInCar;

        [Header("Hint Sit In The Car")]
        [SerializeField] Image imageHintMissionSitInCar;

        // --------------------------------

        // -- Mission Find Suitable Location --
        [Header("Window Mission Find Suitable Location")]
        [SerializeField] GameObject windowMissionFindSuitableLocation;

        // --------------------------------

        // -- Mission Set Up a Bonfire --
        [Header("Window Mission Set Up Bonfire")]
        [SerializeField] GameObject windowMissionSetUpBonfire;

        [Header("Button 'Use' Set Up Bonfire ")]
        [SerializeField] Button btnSetUpBonfire;

        // --------------------------------

        // -- Mission Place a Tent --
        [Header("Window Mission Place a Tent")]
        [SerializeField] GameObject windowMissionPlaceTent;

        [Header("Button 'Use' Place a Tent ")]
        [SerializeField] Button btnPlaceTent;

        // --------------------------------

        // -- Mission Prepare Food --
        [Header("Window Mission Prepare Food")]
        [SerializeField] GameObject windowMissionPrepareFood;

        [SerializeField] GameObject cannedFoodItem, canOpenerItem;

        // --------------------------------

        // -- Mission Put a Sleeping Bag --
        [Header("Window Mission Put a Sleeping Bag")]
        [SerializeField] GameObject windowMissionPutSleepingBag;

        [Header("Button 'Use' Put a Sleeping Bag ")]
        [SerializeField] Button btnPutSleepingBag;

        // --------------------------------

        // -- Mission Go Sleep --
        [Header("Window Mission Go Sleep")]
        [SerializeField] GameObject windowMissionGoSleep;

        [Header("Window Mission Check the Car")]
        [SerializeField] GameObject windowMissionCheckCar;

        // --------------------------------

        [Header("Main Window Missions")]
        [SerializeField] GameObject panelMissions;

        [HideInInspector]
        public bool isHasPrepareFood = false;

        public override void OnTick()
        {
            if (drivingPlayer.isInCar == false) 
                ShowMissionBuyItemsShopForSurvival(); 

            if (checkCompleteTasksNotepad.completeMissionItemsForSurvival == true && 
                dialogueWithSaler.hasBoughtItemsInShop == true) 
                ShowMissionSitInCar();


            if (teleportCar.isHasNewLocation == true && drivingPlayer.isInCar == false)
                ShowMissionFindSuitableLocation();

            if (buildingManagerTent.isHasPlacedItem == true && isHasPrepareFood == false)
                ShowMissionPrepareFood();
        }

        // должна быть подсказка для открытия блокнота


        public void ShowMissionBuyItemsShopForSurvival() 
        {
            panelMissions.SetActive(true);
            //windowMissionBuyItemsForSurvival.SetActive(true); включить когда начинается игра
        }

        public void ShowMissionSitInCar() 
        {
            windowMissionBuyItemsForSurvival.SetActive(false);

            windowMissionSitInCar.SetActive(true);
            imageHintMissionSitInCar.gameObject.SetActive(true);
        }

        public void ShowMissionFindSuitableLocation() 
        {
            windowMissionSitInCar.SetActive(false);
            imageHintMissionSitInCar.gameObject.SetActive(false);

            windowMissionFindSuitableLocation.SetActive(true);

        }

        public void ShowMissionSetUpBonfire() 
        {
            windowMissionFindSuitableLocation.SetActive(false);

            windowMissionSetUpBonfire.SetActive(true);
            btnSetUpBonfire.gameObject.SetActive(true);
        }

        public void ShowMissionPlaceTent() 
        {
            windowMissionSetUpBonfire.SetActive(false);
            btnSetUpBonfire.gameObject.SetActive(false);

            windowMissionPlaceTent.SetActive(true);
            btnPlaceTent.gameObject.SetActive(true);
        }

        public void ShowMissionPrepareFood() 
        {
            windowMissionPlaceTent.SetActive(false);
            btnPlaceTent.gameObject.SetActive(false);

            windowMissionPrepareFood.SetActive(true);
            cannedFoodItem.SetActive(true);
            canOpenerItem.SetActive(true);

            isHasPrepareFood = true;
        }

        public void ShowMissionPutSleepingBag() 
        {
            windowMissionPrepareFood.SetActive(false);
            cannedFoodItem.SetActive(false);
            canOpenerItem.SetActive(false);

            windowMissionPutSleepingBag.SetActive(true);
            btnPutSleepingBag.gameObject.SetActive(true);
        }

        public void ShowMissionGoSleep() 
        {
            windowMissionPutSleepingBag.SetActive(false);
            btnPutSleepingBag.gameObject.SetActive(false);

            windowMissionGoSleep.SetActive(true);
        }

        public void ShowMissionCheckCar() 
        {
            windowMissionGoSleep.SetActive(false);

            windowMissionCheckCar.SetActive(true);
        }
    }
}
