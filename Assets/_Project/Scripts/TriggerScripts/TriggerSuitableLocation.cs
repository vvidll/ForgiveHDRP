using _Project.Scripts.CheckpointsScripts;
using _Project.Scripts.MissionsScripts;
using UnityEngine;

namespace _Project.Scripts.TriggerScripts
{
    public class TriggerSuitableLocation : MonoBehaviour
    {
        [SerializeField] ShowMissionsManager showMissions;
        [SerializeField] BuildingPlaceCollision[] buildingPlaceCollisions;
        [SerializeField] CheckpointManager checkpointManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                showMissions.ShowMissionSetUpBonfire();

                checkpointManager.checkpointsArray[4].SetActive(true); // finded suitable location
            }

        }

        // почему-то при выходе из подходящей зоны(для постройки) объекты можно разместить
        private void OnTriggerExit(Collider other)
        {
            for (int i = 0; i < buildingPlaceCollisions.Length; i++)
            {
                if (other.CompareTag("PlacedObjects"))
                    buildingPlaceCollisions[i].isGroundedForPlaceBuildings = false;
            }
        }
    }
}