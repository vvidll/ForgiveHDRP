using _Project.Scripts.MissionsScripts;
using UnityEngine;

// скрипт отвечающий за общую работу постройки объектов
public class BuildingManager : MonoBehaviour
{
    //[SerializeField] BuildingPlaceCollision[] buildingPointsCollisions;
    [SerializeField] BuildingPlaceCollision buildingPlaceCollisions;

    [SerializeField] Transform parentObjectTerrain;
    [SerializeField] Transform currentObject;

    [SerializeField] GameObject buildingPlace;
    //[SerializeField] GameObject[] buildingPoints;

    public bool isHasPlacedItem = false;

    [SerializeField] public string nameItemForBuilding;

    private void Update()
    {
        ChangePositionModelY();
        ChangeRotateModelY();
        ChangePositionModelZ();

        if (buildingPlaceCollisions.isGroundedForPlaceBuildings == true && Input.GetMouseButtonDown(0))
        {
            currentObject.SetParent(parentObjectTerrain);
            buildingPlace.SetActive(false);
            this.enabled = false;

            isHasPlacedItem = true;
        }
    }

    public void ChangePositionModelY()
    {
        if (Input.GetKey(KeyCode.Y) && transform.localPosition.y <= 1f)
            transform.localPosition += new Vector3(0, .01f, 0);

        if (Input.GetKey(KeyCode.H) && transform.localPosition.y >= -.5f)
            transform.localPosition += new Vector3(0, -.01f, 0);
    }

    public void ChangeRotateModelY()
    {
        if(Input.GetKey(KeyCode.Z))
            transform.RotateAround(transform.position, Vector3.up, 1f);

        if(Input.GetKey(KeyCode.X))
            transform.RotateAround(transform.position, -Vector3.up, 1f);

    }

    public void ChangePositionModelZ() 
    {
        if(Input.GetKey(KeyCode.J))
            transform.localPosition += new Vector3(0, 0, .05f);

        if (Input.GetKey(KeyCode.M))
            transform.localPosition += new Vector3(0, 0, -.05f);
    }
}
