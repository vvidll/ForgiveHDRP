using System.Collections;
using _Project.Scripts.MissionsScripts;
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

                StartCoroutine(ShowTextForAnalysisSuccessful());

                Debug.Log("Анализ закончился");
            }

            if (isStartProgressAnalysis == true && countStep == 0)
                StartCoroutine(IncreasedProgressAnalysisCororutineStepFirst());

            if (countStep == 1)
                StartCoroutine(IncreasedProgressAnalysisCororutineStepSecond());

            if (countStep == 2)
                StartCoroutine(IncreasedProgressAnalysisCororutineStepThird());

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
                interactWithCarText.text = "Start analysis for repair a car";
            
        }

        IEnumerator ShowTextForAnalysisSuccessful()
        {
            textForSuccessfulAnalysis.gameObject.SetActive(true);
            textForSuccessfulAnalysis.text = "Analysis successful!";

            isStartProgressAnalysis = false;
            showMissionsManager.isNeedCarCheck = false;

            yield return new WaitForSeconds(2);
            textForSuccessfulAnalysis.gameObject.SetActive(false);
        }

        IEnumerator IncreasedProgressAnalysisCororutineStepFirst()
        {
            yield return new WaitForSeconds(2);
            barProgressionForWriteInNotepad.value = 10;

            countStep = 1;
        }

        IEnumerator IncreasedProgressAnalysisCororutineStepSecond()
        {
            yield return new WaitForSeconds(2);
            barProgressionForWriteInNotepad.value = 30;

            countStep = 2;
        }

        IEnumerator IncreasedProgressAnalysisCororutineStepThird()
        {
            yield return new WaitForSeconds(3);
            barProgressionForWriteInNotepad.value = 50;

            countStep = 3;
        }
    }
}