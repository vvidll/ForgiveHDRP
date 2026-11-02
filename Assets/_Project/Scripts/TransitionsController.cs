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

        // Begin Start Game Window
        [Header("Begin Start Game Window - UI")]
        [SerializeField] Image beginTransitionWindow;
        float timerForChangeToTransparentTransitionWindow = 5f;
        float timerDisableBeginStartGameWindows = 1f;

        // Sit Car Window
        [Header("Sit Car Window - UI")]
        [SerializeField] Image sitCarTransitionWindow;
        float timerBeforeSitCar = 3f;
        float timerAfterTeleportToPointFirstLocation = 3f;
        float timerDisableSitCarWindow = 1f;

        // Driving Car in to Second Location Window
        [Header("Driving Car in to Second Location Window - UI")]
        [SerializeField] Image drivingCarSecondLocationTransitionWindow;

        float timerBeforeTeleportToSecondLocation = .1f;
        float timerAfterTeleportToSecondLocation = 3f;

        // Teleport Player From Cloze Zone
        [Header("Teleport Player From Cloze Zone - UI")]
        [SerializeField] Image teleportPlayerTransitionClozeZoneWindow;
        float timerDisableTeleporCloseZoneWindow = 1f;
        float timerAfterTeleportCloseZone = 2f;

        // Sleep Window
        [Header("Sleep Window - UI")]
        [SerializeField] Image sleepTransitionWindow;
        float timerChangeSleep = .1f;
        float timerAfterChangeSleep = 1f;
        float timerDisableSleepWindow = 1f;


        float timeDisableBeginTransitionWindow = 7f;


        [Header("References From Other Classes")]
        [SerializeField] TeleportCar teleportCar;
        [SerializeField] SunController sunController;
        [SerializeField] ShowMissionsManager showMissionsManager;
        [SerializeField] DrivingPlayer drivingPlayer;



        private void Awake()
        {
            // on when need transition in start game
            StartCoroutine(BeginStartGameCoroutine());
        }

        IEnumerator BeginStartGameCoroutine() 
        {
            beginTransitionWindow.gameObject.SetActive(true);
            ChangeAlphaValueForBeginStartGameWindow();

            yield return new WaitForSeconds(timerDisableBeginStartGameWindows);
            beginTransitionWindow.gameObject.SetActive(false);
        }

        public IEnumerator SitCarCoroutine()
        {
            yield return new WaitForSeconds(timerBeforeSitCar);
            sitCarTransitionWindow.gameObject.SetActive(true);
            ChangeAlphaValueForSitCarWindowToBlack();

            cinemachineCamera.SetActive(true);
            rigBuilder.layers[drivingPlayer.rigHeadIndex].active = true;

            teleportCar.TeleportToPointOldLocation();

            showMissionsManager.ShowMissionFindSuitableLocation();

            yield return new WaitForSeconds(timerAfterTeleportToPointFirstLocation);
            ChangeAlphaValueForSitCarWindowToWhite();

            yield return new WaitForSeconds(timerDisableSitCarWindow);
            sitCarTransitionWindow.gameObject.SetActive(false);

        }

        public IEnumerator DrivingToNewLocationCoroutine() 
        {
            // restart change alpha value
            yield return new WaitForSeconds(timerBeforeTeleportToSecondLocation);

            drivingCarSecondLocationTransitionWindow.gameObject.SetActive(true);
            ChangeAlphaValueForDrivingCarToSecondLocationWindowToBlack();

            teleportCar.TeleportToNewLocation();

            yield return new WaitForSeconds(timerAfterTeleportToSecondLocation);
            ChangeAlphaValueForDrivingCarToSecondLocationWindowToWhite();

            yield return new WaitForSeconds(timeDisableBeginTransitionWindow);
            drivingCarSecondLocationTransitionWindow.gameObject.SetActive(false);

        }

        public IEnumerator ChangeTimeDayToNightCoroutine() 
        {
            sleepTransitionWindow.gameObject.SetActive(true);
            ChangeAlphaValueForSleepWindowToBlack();

            yield return new WaitForSeconds(timerChangeSleep);
            sunController.ChangeRotationSun();


            yield return new WaitForSeconds(timerAfterChangeSleep);
            ChangeAlphaValueForSleepWindowToWhite();

            yield return new WaitForSeconds(timerDisableSleepWindow);
            sleepTransitionWindow.gameObject.SetActive(false);
        }

        public IEnumerator EndTimerCloseZoneCoroutine() 
        {
            teleportPlayerTransitionClozeZoneWindow.gameObject.SetActive(true);
            ChangeAlphaValueForTeleportPlayerFromCloseZoneWindowToBlack();

            yield return new WaitForSeconds(timerAfterTeleportCloseZone);
            ChangeAlphaValueForTeleportPlayerFromCloseZoneToWhite();

            yield return new WaitForSeconds(timerDisableTeleporCloseZoneWindow);
            teleportPlayerTransitionClozeZoneWindow.gameObject.SetActive(false);
        }

        public void ChangeAlphaValueForBeginStartGameWindow() 
        { 
            beginTransitionWindow.DOColor(new Color(0, 0, 0, 0), timerForChangeToTransparentTransitionWindow);
        }

        public void ChangeAlphaValueForSitCarWindowToBlack() => sitCarTransitionWindow.DOColor(new Color(0, 0, 0, 255), .5f);
        public void ChangeAlphaValueForSitCarWindowToWhite() => sitCarTransitionWindow.DOColor(new Color(0, 0, 0, 0), timerForChangeToTransparentTransitionWindow);

        public void ChangeAlphaValueForDrivingCarToSecondLocationWindowToWhite() => drivingCarSecondLocationTransitionWindow.DOColor(new Color(0, 0, 0, 0), timerForChangeToTransparentTransitionWindow);

        public void ChangeAlphaValueForDrivingCarToSecondLocationWindowToBlack() => drivingCarSecondLocationTransitionWindow.DOColor(new Color(0, 0, 0, 255), .5f);

        public void ChangeAlphaValueForSleepWindowToWhite() => sleepTransitionWindow.DOColor(new Color(0, 0, 0, 0), timerForChangeToTransparentTransitionWindow);
        public void ChangeAlphaValueForSleepWindowToBlack() => sleepTransitionWindow.DOColor(new Color(0, 0, 0, 255), .5f);

        public void ChangeAlphaValueForTeleportPlayerFromCloseZoneWindowToBlack() => teleportPlayerTransitionClozeZoneWindow.DOColor(new Color(0, 0, 0, 255), .5f);
        public void ChangeAlphaValueForTeleportPlayerFromCloseZoneToWhite() => teleportPlayerTransitionClozeZoneWindow.DOColor(new Color(0, 0, 0, 0), timerForChangeToTransparentTransitionWindow);
    }
}