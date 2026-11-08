using _Project.Scripts;
using UnityEngine;

// скрипт отвечающий за переход между локациями и включения анимации у машины во второй локации
public class TriggerTeleportCarNextLocation : MonoBehaviour
{
    [SerializeField] TransitionsController transitionsController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car")) 
        {
            transitionsController.DrivingToNewLocationCoroutine().Forget();
        }

        
    }
}
