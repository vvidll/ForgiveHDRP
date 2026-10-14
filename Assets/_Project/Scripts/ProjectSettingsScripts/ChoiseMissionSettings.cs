using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.MissionsScripts;
using _Project.Scripts.PlacedItemsScripts;
using UnityEngine;

public class ChoiseMissionSettings : MonoBehaviour
{

    [Header("Work In Play Mode!\n" +
        "This Script Need For Quickly Change Missions\n" +
        "Key < PageUp > for skip mission")]

    [Header("References From Other Classes")]
    [SerializeField] TransitionsController transitionsController;
    [SerializeField] EnemyWatcherController enemyWatcherController;
    [SerializeField] SleepingBagController sleepingBagController;

    [SerializeField] List<GameObject> windowsMissions;

    [SerializeField] ShowMissionsManager showMissionsManager;

    [Header("Main Window Missions")]
    [SerializeField] GameObject panelMissions;

    public int currentMission = 0;

    public bool isCurrentFirstMissionActive = false;

    private void Awake()
    {
        panelMissions.SetActive(true);
    }

    void Update()
    {
        ChoiseMission();
    }

    public void ChoiseMission() 
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            isCurrentFirstMissionActive = true;

            currentMission++;

            if (currentMission >= windowsMissions.Count)
                currentMission = 8;

            ShowCurrentMission();
        }

        /*if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMission--;

            if (currentMission <= 0)
                currentMission = 0;

            ShowCurrentMission();
        }*/
    }

    public void ShowCurrentMission() 
    {

        switch (currentMission)
        {
            case 0:
                showMissionsManager.ShowMissionBuyItemsShopForSurvival();
                Debug.Log("Show Mission Buy Items Shop For Survival active!");
                break;

            case 1:
                showMissionsManager.ShowMissionSitInCar();
                Debug.Log("Show Mission Sit In Car active!");
                break;

            case 2:
                showMissionsManager.ShowMissionFindSuitableLocation();
                break;

            case 3:
                showMissionsManager.ShowMissionSetUpBonfire();
                break;

            case 4:
                showMissionsManager.ShowMissionPlaceTent();
                break;

            case 5:
                showMissionsManager.ShowMissionPrepareFood();
                break;

            case 6:
                showMissionsManager.ShowMissionPutSleepingBag();
                break;

            case 7:
                showMissionsManager.ShowMissionGoSleep();
                break;

            case 8:
                sleepingBagController.isChangeTimeDay = true;
                enemyWatcherController.isActiveEnemyWatcher = true;
                transitionsController.StartCoroutine(transitionsController.ChangeTimeDayToNightCoroutine());
                showMissionsManager.ShowMissionCheckCar();
                break;
        }
    }

    


    
}
