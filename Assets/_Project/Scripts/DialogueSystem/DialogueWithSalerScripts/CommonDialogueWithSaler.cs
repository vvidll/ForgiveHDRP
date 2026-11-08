using _Project.Scripts.TextScripts;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.DialogueSystem.DialogueWithSalerScripts
{
    // скрипт отвечающий за диалог с продавцом(выведен как отдельный скрипт для одинаковых методов, сделано для удобства)
    public class CommonDialogueWithSaler : MonoBehaviour
    {
        public const string CurrentDialogueIndexKey = "current_dialogue_index";

        [ContextMenu("Delete Key Dialogue (Польз.)")]
        protected void DeleteKeysDialogue() => PlayerPrefs.DeleteKey(CurrentDialogueIndexKey);

        protected void CurrentDialogue(ChoiseAnswersController choiseAnswers, int currentDialogueIndex, 
            TypingText typingText, string namePerson, TMP_Text tmpNamePerson, string dialogueText, TMP_Text tmpDialogue)
        {
            DisableButtonsForAnswers(choiseAnswers, currentDialogueIndex);

            Debug.Log(currentDialogueIndex);

            tmpNamePerson.text = namePerson;
            tmpDialogue.text = dialogueText;

            typingText.UpdateText();
        }

        public void SaveCurrentDialogue(int currentDialogueIndex)
        {
            PlayerPrefs.SetInt(CurrentDialogueIndexKey, currentDialogueIndex);
        }

        public static int LoadCurrentDialogue(int currentDialogueIndex)
        {
            return PlayerPrefs.GetInt(CurrentDialogueIndexKey, currentDialogueIndex);
        }

        public void DisableButtonsForAnswers(ChoiseAnswersController choiseAnswers, int currentDialogueIndex)
        {
            for (int i = 0; i < currentDialogueIndex; i++)
            {
                switch (currentDialogueIndex)
                {
                    case 1:
                    case 2:
                        choiseAnswers.windowChoiseAnswer.SetActive(false);
                        choiseAnswers.answersButtons[0].gameObject.SetActive(false);
                        break;

                    case 3:
                        choiseAnswers.windowChoiseAnswer.SetActive(false);

                        choiseAnswers.answersButtons[1].gameObject.SetActive(false);
                        break;

                    case 4:
                        choiseAnswers.windowChoiseAnswer.SetActive(false);

                        choiseAnswers.answersButtons[2].gameObject.SetActive(false);
                        break;

                    case 5:
                        choiseAnswers.windowChoiseAnswer.SetActive(false);

                        choiseAnswers.answersButtons[3].gameObject.SetActive(false);
                        break;

                    case 6:
                        choiseAnswers.windowChoiseAnswer.SetActive(false);

                        choiseAnswers.answersButtons[4].gameObject.SetActive(false);
                        break;

                    case 7:
                        choiseAnswers.windowChoiseAnswer.SetActive(false);
                        break;
                }

            }
        }

    }
}