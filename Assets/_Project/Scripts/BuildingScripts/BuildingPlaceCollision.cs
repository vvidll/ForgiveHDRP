using UnityEngine;

// скрипт отвечающий за отображение коллизии спецаильный мест с землей,
// чтобы игрок видел где можно поставить объект, а где нет
public class BuildingPlaceCollision : MonoBehaviour
{
    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] Material placeMaterial;

    [SerializeField] Color colorGreen;
    [SerializeField] Color colorRed;

    //[HideInInspector]
    public bool isGroundedForPlaceBuildings = false;

    private void Update()
    {
        isGroundedForPlaceBuildings = Physics.Raycast(transform.position, -Vector3.up, .1f, groundLayerMask);

        if (isGroundedForPlaceBuildings)
        {
            Debug.Log("collision enter!");
            placeMaterial.color = colorGreen;
        }

        else 
        {
            Debug.Log("collision exit!");
            placeMaterial.color = colorRed;
        }
    }
}
