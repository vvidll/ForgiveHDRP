using System.Collections;
using _Project.Scripts.CarScripts;
using _Project.Scripts.MissionsScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class TransitionsController : MonoBehaviour
    {
        [SerializeField] Image beginTransitionWindow;
        [SerializeField] Image sleepTransitionWindow;

        [Header("References From Other Classes")]
        [SerializeField] TeleportCar teleportCar;
        [SerializeField] SunController sunController;
        [SerializeField] ShowMissionsManager showMissionsManager;


        float timeBeforeOnWindowTransition = 3f;
        float timeBeforeChangeAlphaValueForBeginWindow = 2f;

        float timerForBeginTransition = 5f;
        float timerRestartWindowValue = .1f;

        float timeDisableWindow = 7f;

        private void Awake()
        {
            // on when need transition in start game
            //ChangeAlphaValueForBeginWindow();
        }

        public void TransitionTeleportCar()
        {
            StartCoroutine(TeleportCarCoroutine());

            Debug.Log("transition active");
        }

        IEnumerator TeleportCarCoroutine()
        {
            yield return new WaitForSeconds(timeBeforeOnWindowTransition);
            beginTransitionWindow.gameObject.SetActive(true);

            teleportCar.TeleportToPointOldLocation();

            showMissionsManager.ShowMissionFindSuitableLocation();

            yield return new WaitForSeconds(timeBeforeChangeAlphaValueForBeginWindow);
            ChangeAlphaValueForBeginWindowToWhite();

        }

        public IEnumerator DrivingToNewLocationCoroutine() 
        {
            // restart change alpha value
            yield return new WaitForSeconds(timerRestartWindowValue);
            ChangeAlphaValueForBeginWindowToBlack();

            teleportCar.TeleportToNewLocation();

            ChangeAlphaValueForBeginWindowToWhite();

            yield return new WaitForSeconds(timeDisableWindow);
            beginTransitionWindow.gameObject.SetActive(false);

        }

        public IEnumerator ChangeTimeDayToNightCoroutine() 
        {
            //sleepTransitionWindow.gameObject.SetActive(true);
            ChangeAlphaValueForBeginWindowToBlack();

            yield return new WaitForSeconds(timerRestartWindowValue);
            sunController.ChangeRotationSun();

            ChangeAlphaValueForBeginWindowToWhite();
        }

        public void ChangeAlphaValueForBeginWindowToWhite() => beginTransitionWindow.DOColor(new Color(0, 0, 0, 0), timerForBeginTransition);
        public void ChangeAlphaValueForBeginWindowToBlack() => beginTransitionWindow.DOColor(new Color(0, 0, 0, 255), .5f);
    }
}