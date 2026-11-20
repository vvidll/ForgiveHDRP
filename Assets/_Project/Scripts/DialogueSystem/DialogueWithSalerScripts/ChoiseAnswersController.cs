using System.Collections;
using _Project.Scripts.InventoryScripts;
using _Project.Scripts.TextScripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.DialogueSystem.DialogueWithSalerScripts
{
    // скрипт отвечающий за общую работу всех выборов ответов
    public class ChoiseAnswersController : ChoiseAnswerContollerCommon
    {
        [Header("AUDIO")]
        [SerializeField] AudioSource[] audiosAnswerSource;
        [SerializeField] AudioSource audioLeaveFromDialogueAnswer;

        [Header("REFERENCES")]
        [SerializeField] CheckCompleteTasksNotepad checkCompleteTasksNotepad;
        [SerializeField] DialogueWithSeller dialogue;
        [SerializeField] DisableAndEnableMovementAndCursorController disableAndEnableMovementAndCursorController;
        [SerializeField] TypingText typingText;
        [SerializeField] public GameObject windowChoiseAnswer;
        
        [Header("BUTTONS FOR ANSWERS")]
        [SerializeField] public Button[] answersButtons;
        [SerializeField] public Button answerForExitDialogueBtn;

        float timeTalkPlayer = 6f;

        public void FirstAnswerClickButton() 
        {
            audiosAnswerSource[0].Play();
            StartCoroutine(WaitFirstTalkPlayerCoroutine());
        }

        IEnumerator WaitFirstTalkPlayerCoroutine() 
        {
            yield return new WaitForSeconds(2);
            dialogue.SecondDialogue();
        }

        public void SecondAnswerClickButton()
        {
            audiosAnswerSource[1].Play();
            StartCoroutine(WaitSecondTalkPlayerCoroutine());
        }

        IEnumerator WaitSecondTalkPlayerCoroutine()
        {
            yield return new WaitForSeconds(5);
            dialogue.ThirdDialogueStageFirst();
        }

        public void ThirdAnswerClickButton()
        {
            audiosAnswerSource[2].Play();
            StartCoroutine(WaitThirdTalkPlayerCoroutine());
        }

        IEnumerator WaitThirdTalkPlayerCoroutine()
        {
            yield return new WaitForSeconds(2);
            dialogue.FourthDialogue();
        }


        public void FourthAnswerClickButton()
        {
            audiosAnswerSource[3].Play();
            StartCoroutine(WaitFourthTalkPlayerCoroutine());
        }

        IEnumerator WaitFourthTalkPlayerCoroutine()
        {
            yield return new WaitForSeconds(4);
            dialogue.FifthDialogue();
        }

        public void FifthAnswerClickButton()
        {
            audiosAnswerSource[4].Play();
            StartCoroutine(WaitFifthTalkPlayerCoroutine());
        }

        IEnumerator WaitFifthTalkPlayerCoroutine()
        {
            yield return new WaitForSeconds(4);
            dialogue.SixthDialogue();
        }

        public void SixthAnswerClickButton()
        {
            if (checkCompleteTasksNotepad.completeMissionItemsForSurvival == false)
            {
                Debug.Log("Вы не взяли вещи!");
            }

            else 
            {
                audiosAnswerSource[5].Play();
                StartCoroutine(WaitSixthTalkPlayerCoroutine());
            }

        }

        IEnumerator WaitSixthTalkPlayerCoroutine()
        {
            yield return new WaitForSeconds(3);
            dialogue.SeventhDialogue();
        }

        public void ExitFromCurrentDialogueClickButton()
        {
            audioLeaveFromDialogueAnswer.Play();

            StartCoroutine(WaitTalkPlayerExitDialogueCoroutine());

            disableAndEnableMovementAndCursorController.isDialogueWithSalerActive = false;
            disableAndEnableMovementAndCursorController.DisableMovementAndShowCursor();
        }

        IEnumerator WaitTalkPlayerExitDialogueCoroutine()
        {
            yield return new WaitForSeconds(4);
            StartCoroutine(dialogue.StopDialogueCoroutine());
        }

        public IEnumerator ChoiseFirstAnswerCoroutine()
        {
            yield return new WaitForSeconds(5);
            ChoiseAnswer(windowChoiseAnswer, answersButtons, 0, answerForExitDialogueBtn);
        }
        
        public IEnumerator ChoiseSecondAnswerCoroutine()
        {
            yield return new WaitForSeconds(8);
            ChoiseAnswer(windowChoiseAnswer, answersButtons, 1, answerForExitDialogueBtn);
        }
        
        public IEnumerator ChoiseThirdAnswerCoroutine()
        {
            yield return new WaitForSeconds(3);
            ChoiseAnswer(windowChoiseAnswer, answersButtons, 2, answerForExitDialogueBtn);
        }

        public IEnumerator ChoiseFourthAnswerCoroutine() 
        {
            yield return new WaitForSeconds(2);
            ChoiseAnswer(windowChoiseAnswer, answersButtons, 3, answerForExitDialogueBtn);
        }

        public IEnumerator ChoiseFifthAnswerCoroutine() 
        {
            yield return new WaitForSeconds(4);
            ChoiseAnswer(windowChoiseAnswer, answersButtons, 4, answerForExitDialogueBtn);
        }

        public IEnumerator ChoiseSixthAnswerCoroutine() 
        {
            yield return new WaitForSeconds(4);
            ChoiseAnswer(windowChoiseAnswer, answersButtons, 5, answerForExitDialogueBtn);
        }

        public IEnumerator StopAllDialogues() 
        {
            yield return new WaitForSeconds(2);
            dialogue.StopDialogue();

            disableAndEnableMovementAndCursorController.isDialogueWithSalerActive = false;
            disableAndEnableMovementAndCursorController.EnableMovementAndHideCursor();
        }
    }
}
