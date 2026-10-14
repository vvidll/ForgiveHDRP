using _Project.Scripts.PlayerScripts;
using UnityEngine;

namespace _Project.Scripts.CarScripts
{

    // скрипт отвечающий за появление иконки зажатия для выхода из машины
    public class CarAnimation : MonoBehaviour
    {
        [SerializeField] DrivingPlayer drivingPlayer;

        public void ExitFromCar()
        {
            Debug.Log("image active!");
            drivingPlayer.imageInteractHold.gameObject.SetActive(true);
        }

    }
}