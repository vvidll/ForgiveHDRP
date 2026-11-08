using System;
using _Project.Scripts.CarScripts;
using _Project.Scripts.MissionsScripts;
using _Project.Scripts.PlayerScripts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class TransitionsController : MonoBehaviour
    {
        // values in " INT " variable (100, 1000, .... - n) in thousands this " SECONDS " from (0,1s, 1s, .... - n) 

        [Header("Cinemachine")]
        [SerializeField] GameObject cinemachineCamera;

        [Header("RigBuilder")]
        [SerializeField] RigBuilder rigBuilder;

        // Begin Start Game Window
        [Header("Begin Start Game Window - UI")]
        [SerializeField] Image beginTransitionWindow;
        float timerForChangeToTransparentTransitionWindow = 5f;
        int timerDisableBeginStartGameWindows = 1000;

        // Sit Car Window
        [Header("Sit Car Window - UI")]
        [SerializeField] Image sitCarTransitionWindow;
        int timerBeforeSitCar = 3000;
        int timerAfterTeleportToPointFirstLocation = 3000;
        int timerDisableSitCarWindow = 1000;

        // Driving Car in to Second Location Window
        [Header("Driving Car in to Second Location Window - UI")]
        [SerializeField] Image drivingCarSecondLocationTransitionWindow;

        int timerBeforeTeleportToSecondLocation = 100;
        int timerAfterTeleportToSecondLocation = 3000;

        int timeDisableTeleportToSecondLocationWindow = 7000;

        // Teleport Player From Cloze Zone
        [Header("Teleport Player From Cloze Zone - UI")]
        [SerializeField] Image teleportPlayerTransitionClozeZoneWindow;
        int timerDisableTeleporCloseZoneWindow = 1000;
        int timerAfterTeleportCloseZone = 2000;

        // Sleep Window
        [Header("Sleep Window - UI")]
        [SerializeField] Image sleepTransitionWindow;
        int timerChangeSleep = 100; 
        int timerAfterChangeSleep = 1000;
        int timerDisableSleepWindow = 1000;


        [Header("References From Other Classes")]
        [SerializeField] TeleportCar teleportCar;
        [SerializeField] SunController sunController;
        [SerializeField] ShowMissionsManager showMissionsManager;
        [SerializeField] DrivingPlayer drivingPlayer;



        private void Awake()
        {
            // on when need transition in start game
            BeginStartGameCoroutine().Forget();
        }

        async UniTaskVoid BeginStartGameCoroutine() 
        {
            try
            {
                beginTransitionWindow.gameObject.SetActive(true);
                ChangeAlphaValueForBeginStartGameWindow();

                await UniTask.Delay(timerDisableBeginStartGameWindows, cancellationToken: this.GetCancellationTokenOnDestroy());
                beginTransitionWindow.gameObject.SetActive(false);
            }
            catch (Exception ex) { Debug.LogError(ex); }
        }

        public async UniTaskVoid SitCarCoroutine()
        {
            try
            {
                await UniTask.Delay(timerBeforeSitCar, cancellationToken: this.GetCancellationTokenOnDestroy());
                sitCarTransitionWindow.gameObject.SetActive(true);
                ChangeAlphaValueForSitCarWindowToBlack();

                cinemachineCamera.SetActive(true);
                rigBuilder.layers[drivingPlayer.rigHeadIndex].active = true;

                teleportCar.TeleportToPointOldLocation();

                showMissionsManager.ShowMissionFindSuitableLocation();

                await UniTask.Delay(timerAfterTeleportToPointFirstLocation, cancellationToken: this.GetCancellationTokenOnDestroy());
                ChangeAlphaValueForSitCarWindowToWhite();

                await UniTask.Delay(timerDisableSitCarWindow, cancellationToken: this.GetCancellationTokenOnDestroy());
                sitCarTransitionWindow.gameObject.SetActive(false);
            }
            catch (Exception ex) { Debug.LogError(ex); }

        }

        public async UniTaskVoid DrivingToNewLocationCoroutine() 
        {
            try
            {
                // restart change alpha value
                await UniTask.Delay(timerBeforeTeleportToSecondLocation, cancellationToken: this.GetCancellationTokenOnDestroy());

                drivingCarSecondLocationTransitionWindow.gameObject.SetActive(true);
                ChangeAlphaValueForDrivingCarToSecondLocationWindowToBlack();

                teleportCar.TeleportToNewLocation();

                await UniTask.Delay(timerAfterTeleportToSecondLocation, cancellationToken: this.GetCancellationTokenOnDestroy());
                ChangeAlphaValueForDrivingCarToSecondLocationWindowToWhite();

                await UniTask.Delay(timeDisableTeleportToSecondLocationWindow, cancellationToken: this.GetCancellationTokenOnDestroy());
                drivingCarSecondLocationTransitionWindow.gameObject.SetActive(false);
            }
            catch (Exception ex) { Debug.LogError(ex); }

        }

        public async UniTaskVoid ChangeTimeDayToNightCoroutine() 
        {
            try 
            { 
                sleepTransitionWindow.gameObject.SetActive(true);
                ChangeAlphaValueForSleepWindowToBlack();

                await UniTask.Delay(timerChangeSleep, cancellationToken: this.GetCancellationTokenOnDestroy());
                sunController.ChangeRotationSun();


                await UniTask.Delay(timerAfterChangeSleep, cancellationToken: this.GetCancellationTokenOnDestroy());
                ChangeAlphaValueForSleepWindowToWhite();

                await UniTask.Delay(timerDisableSleepWindow, cancellationToken: this.GetCancellationTokenOnDestroy());
                sleepTransitionWindow.gameObject.SetActive(false);
            }
            catch (Exception ex) { Debug.LogError(ex); }
        }

        public async UniTaskVoid EndTimerCloseZoneCoroutine() 
        {
            try 
            { 
                teleportPlayerTransitionClozeZoneWindow.gameObject.SetActive(true);
                ChangeAlphaValueForTeleportPlayerFromCloseZoneWindowToBlack();

                await UniTask.Delay(timerAfterTeleportCloseZone, cancellationToken: this.GetCancellationTokenOnDestroy());
                ChangeAlphaValueForTeleportPlayerFromCloseZoneToWhite();

                await UniTask.Delay(timerDisableTeleporCloseZoneWindow, cancellationToken: this.GetCancellationTokenOnDestroy());
                teleportPlayerTransitionClozeZoneWindow.gameObject.SetActive(false);

            }
            catch (Exception ex) { Debug.LogError(ex); }
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