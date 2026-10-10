using _Project.Scripts.PlayerScripts;
using UnityEngine;

namespace _Project.Scripts.CarScripts
{

    // скрипт отвечающий за включение анимации выхода из машины
    public class CarAnimation : MonoBehaviour
    {
        [SerializeField] DrivingPlayer drivingPlayer;

        public void ExitFromCar() => drivingPlayer.imageInteractHold.gameObject.SetActive(true);
       

    }
}