using System;
using System.Collections;
using _Project.Scripts.MissionsScripts;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InteractScripts.CarInteractScripts
{
    // скрипт отвечающий за анализ и починку машины
    public class CarInteractRepair : MonoCache, IInteractable
    {
        [SerializeField] ShowMissionsManager showMissionsManager;
        [SerializeField] TMP_Text interactWithCarText;
        [SerializeField] Slider barProgressionForWriteInNotepad;
        [SerializeField] TMP_Text textForSuccessfulAnalysis;

        bool isHideSlider = false;
        bool isStartProgressAnalysis = false;

        int countStep = 0;
        int countPressF = 0;

        public override void OnTick()
        {
            if (barProgressionForWriteInNotepad.value == 50 && isHideSlider == false)
            {
                barProgressionForWriteInNotepad.gameObject.SetActive(false);
                isHideSlider = true;
                isStartProgressAnalysis = false;

                ShowTextForAnalysisSuccessful().Forget();

                Debug.Log("Анализ закончился");
            }

            if (isStartProgressAnalysis == true && countStep == 0)
                IncreasedProgressAnalysisCororutineStepFirst().Forget();

            if (countStep == 1)
                IncreasedProgressAnalysisCororutineStepSecond().Forget();

            if (countStep == 2)
                IncreasedProgressAnalysisCororutineStepThird().Forget();

        }

        public void Interact()
        {
            // !!! ON when need repair of the car !!!

            if (showMissionsManager.isNeedCarCheck == true)
            {
                switch (countPressF)
                {
                    case 0:
                        countPressF = 1;
                        isStartProgressAnalysis = true;
                        barProgressionForWriteInNotepad.gameObject.SetActive(true);
                        break;

                    case 1:
                        countPressF = 2;
                        barProgressionForWriteInNotepad.gameObject.SetActive(false);
                        break;

                }
            }

        }

        public void Description()
        {
            // !!! ON when need repair of the car !!!

            if (showMissionsManager.isNeedCarCheck == true)
            {
                interactWithCarText.text = "Start analysis for repair a car";
            }
        }

        async UniTaskVoid ShowTextForAnalysisSuccessful()
        {
            try 
            { 
                textForSuccessfulAnalysis.gameObject.SetActive(true);
                textForSuccessfulAnalysis.text = "Analysis successful!";

                isStartProgressAnalysis = false;
                showMissionsManager.isNeedCarCheck = false;

                await UniTask.Delay(2000, cancellationToken: this.GetCancellationTokenOnDestroy());
                textForSuccessfulAnalysis.gameObject.SetActive(false);

                showMissionsManager.ShowMissionFindItemsForCarRepair();
            }
            catch (Exception ex) { Debug.LogError(ex); }
        }

        async UniTaskVoid IncreasedProgressAnalysisCororutineStepFirst()
        {
            try 
            { 
                await UniTask.Delay(2000, cancellationToken: this.GetCancellationTokenOnDestroy());
                barProgressionForWriteInNotepad.value = 10;

                countStep = 1;
            }
            catch (Exception ex) { Debug.LogError(ex); }
        }

        async UniTaskVoid IncreasedProgressAnalysisCororutineStepSecond()
        {
            try 
            { 
                await UniTask.Delay(2000, cancellationToken: this.GetCancellationTokenOnDestroy());
                barProgressionForWriteInNotepad.value = 30;

                countStep = 2;
            }
            catch (Exception ex) { Debug.LogError(ex); }
        }

        async UniTaskVoid IncreasedProgressAnalysisCororutineStepThird()
        {
            try 
            { 
                await UniTask.Delay(3000, cancellationToken: this.GetCancellationTokenOnDestroy());
                barProgressionForWriteInNotepad.value = 50;

                countStep = 3;
            }
            catch (Exception ex) { Debug.LogError(ex); }
        }
    }
}