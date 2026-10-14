using _Project.Scripts.MissionsScripts;
using UnityEngine;

public class TriggerSuitableLocation : MonoBehaviour
{
    [SerializeField] ShowMissionsManager showMissions;
    [SerializeField] BuildingPlaceCollision[] buildingPlaceCollisions;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
            showMissions.ShowMissionSetUpBonfire();
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
