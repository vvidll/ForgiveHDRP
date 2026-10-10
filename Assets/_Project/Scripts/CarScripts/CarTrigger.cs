using _Project.Scripts.PlayerScripts;
using UnityEngine;

namespace _Project.Scripts.CarScripts
{
    // скрипт отвечающий за проверку коллизии игрока и телепорта игрока в нужную позицию(не доделан)
    public class CarTrigger : MonoBehaviour
    {
        [Header("Parent Object")]
        [SerializeField] Transform parentObject;

        [Header("Child Object")]
        [SerializeField] Transform player;

        [SerializeField] DrivingPlayer drivingPlayer;

        private void Update()
        {
            if (drivingPlayer.isInCar == false && drivingPlayer.isHoldKeyF == 1)
                player.SetParent(parentObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out DrivingPlayer drivingPlayer))
            {
                //drivingPlayer.isInCar = true;

                if (drivingPlayer.isHoldKeyF == 1)
                {
                    drivingPlayer.isInCar = false;
                    
                    player.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
                }
            }
        }
    }
}