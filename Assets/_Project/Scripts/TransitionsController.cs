using System.Collections;
using _Project.Scripts.CarScripts;
using _Project.Scripts.MissionsScripts;
using _Project.Scripts.PlayerScripts;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class TransitionsController : MonoBehaviour
    {
        [Header("Cinemachine")]
        [SerializeField] GameObject cinemachineCamera;

        [Header("RigBuilder")]
        [SerializeField] RigBuilder rigBuilder;

        [Header("UI")]
        [SerializeField] Image beginTransitionWindow;

        [SerializeField] Image teleportPlayerTransitionWindow;

        [SerializeField] Image sleepTransitionWindow;

        [Header("References From Other Classes")]
        [SerializeField] TeleportCar teleportCar;
        [SerializeField] SunController sunController;
        [SerializeField] ShowMissionsManager showMissionsManager;
        [SerializeField] DrivingPlayer drivingPlayer;


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

            cinemachineCamera.SetActive(true);
            rigBuilder.layers[drivingPlayer.rigHeadIndex].active = true;

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

        public IEnumerator EndTimerCoroutine() 
        {
            ChangeAlphaValueForBeginWindowToBlackPlayerTeleport();

            yield return new WaitForSeconds(.1f);
            ChangeAlphaValueForBeginWindowToWhitePlayerTeleport();
        }

        public void ChangeAlphaValueForBeginWindowToWhite() => beginTransitionWindow.DOColor(new Color(0, 0, 0, 0), timerForBeginTransition);
        public void ChangeAlphaValueForBeginWindowToBlack() => beginTransitionWindow.DOColor(new Color(0, 0, 0, 255), .5f);

        public void ChangeAlphaValueForBeginWindowToBlackPlayerTeleport() => teleportPlayerTransitionWindow.DOColor(new Color(0, 0, 0, 255), .5f);
        public void ChangeAlphaValueForBeginWindowToWhitePlayerTeleport() => teleportPlayerTransitionWindow.DOColor(new Color(0, 0, 0, 0), .5f);
    }
}