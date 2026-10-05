using System.Collections;
using Scripts;
using Scripts.TextScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.DialogueSystem.DialogueWithSalerScripts
{
    // скрипт отвечающий за диалог с продавцом
    public class DialogueWithSaler : CommonDialogueWithSaler
    {
        [SerializeField] AudioSource[] audiosDialogueSource;
        [SerializeField] AudioSource audioLeaveFromDialogue;

        [Header("References")]
        [SerializeField] DisableAndEnableMovementAndCursorController disableAndEnableMovementAndCursorController;
        [SerializeField] TypingText typingText;
        [SerializeField] Image imageInteract;

        [Header("Dialogue Window")]
        [SerializeField] GameObject dialogueWindow;
        [SerializeField] TMP_Text namePerson;
        [SerializeField] public TMP_Text dialogueText;

        [SerializeField] private ChoiseAnswersController choiseAnswers;
        
        [HideInInspector]
        public bool isStartDialogue = false;
        public static int currentDialogueIndex = 1;

        [HideInInspector]
        public bool hasBoughtItemsInShop = false;


        public void StartDialogue()
        {
            imageInteract.gameObject.SetActive(false);

            isStartDialogue = true;
            dialogueWindow.SetActive(true);

            disableAndEnableMovementAndCursorController.isDialogueWithSalerActive = true;
            disableAndEnableMovementAndCursorController.DisableMovementAndShowCursor();

            switch (currentDialogueIndex)
            {
                case 1:
                    FirstDialogue();
                    break;

                case 2:
                    SecondDialogue();
                    break;

                case 3:
                    ThirdDialogueStageFirst();
                    break;

                case 4:
                    FourthDialogue();
                    break;

                case 5:
                    FifthDialogue();
                    break;

                case 6:
                    SixthDialogue();
                    break;

                case 7:
                    SeventhDialogue();
                    break;
            }
        }

        public void FirstDialogue()
        {
            currentDialogueIndex = 1;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
    namePerson, "Здравствуй покупатель! Не ожидал что кто-то сюда приедет, в такое захолустье", dialogueText);

            audiosDialogueSource[0].Play();

            StartCoroutine(choiseAnswers.ChoiseFirstAnswerCoroutine());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public void SecondDialogue()
        {
            currentDialogueIndex = 2;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
    namePerson, "Ну в прямом, здесь месяц назад начали происходить странные вещи в лесу, уже у всех это на слуху, даже полиция отказывается выяснять что здесь случилось", 
    dialogueText);

            audiosDialogueSource[1].Play();

            StartCoroutine(choiseAnswers.ChoiseSecondAnswerCoroutine());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public void ThirdDialogueStageFirst()
        {
            currentDialogueIndex = 3;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
                namePerson, "Нууууу, это долгая история, ты просто знай, я тебя предупредил, а ты уже сам решай идти ли тебе туда или нет",
                dialogueText);

            audiosDialogueSource[2].Play();

            StartCoroutine(ThirdDialogueStageSecond());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public IEnumerator ThirdDialogueStageSecond() 
        {
            yield return new WaitForSeconds(6);

            currentDialogueIndex = 3;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
                namePerson, "хотя знаешь я могу тебе рассказать почему я тут еще работаю",
                dialogueText);

            audiosDialogueSource[3].Play();

            StartCoroutine(ThirdDialogueStageThird());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public IEnumerator ThirdDialogueStageThird()
        {
            yield return new WaitForSeconds(4);

            currentDialogueIndex = 3;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
                namePerson, "но с тебя денюжки дружок",
                dialogueText);

            audiosDialogueSource[4].Play();

            StartCoroutine(choiseAnswers.ChoiseThirdAnswerCoroutine());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public void FourthDialogue() 
        {
            currentDialogueIndex = 4;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
                            namePerson, "Не мало.",
                            dialogueText);

            audiosDialogueSource[5].Play();

            StartCoroutine(choiseAnswers.ChoiseFourthAnswerCoroutine());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public void FifthDialogue() 
        {
            currentDialogueIndex = 5;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
                            namePerson, "Нууу эм пятихатку давай и расскажу",
                            dialogueText);

            audiosDialogueSource[6].Play();

            StartCoroutine(choiseAnswers.ChoiseFifthAnswerCoroutine());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public void SixthDialogue() 
        {
            currentDialogueIndex = 6;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
                            namePerson, "Ну и все тогда, давай выкладывай все что взял и оплачивай",
                            dialogueText);

            audiosDialogueSource[7].Play();

            StartCoroutine(choiseAnswers.ChoiseSixthAnswerCoroutine());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public void SeventhDialogue()
        {
            hasBoughtItemsInShop = true;

            currentDialogueIndex = 7;

            CurrentDialogue(choiseAnswers, currentDialogueIndex, typingText, "SELLER: ",
                            namePerson, "Благодарю",
                            dialogueText);

            audiosDialogueSource[8].Play();

            StartCoroutine(choiseAnswers.StopAllDialogues());

            SaveCurrentDialogue(currentDialogueIndex);
        }

        public IEnumerator StopDialogueCoroutine()
        {
            namePerson.text = "SELLER:";
            dialogueText.text = "Уже уходишь? ну ладно...";

            audioLeaveFromDialogue.Play();

            typingText.UpdateText();

            if (dialogueWindow.activeSelf == true)
            {
                yield return new WaitForSeconds(2);
                StopDialogue();
            }

            

        }

        public void StopDialogue()
        {

            dialogueWindow.SetActive(false);

            isStartDialogue = false;

            disableAndEnableMovementAndCursorController.isDialogueWithSalerActive = false;
            disableAndEnableMovementAndCursorController.EnableMovementAndHideCursor();
        }

    }
}
