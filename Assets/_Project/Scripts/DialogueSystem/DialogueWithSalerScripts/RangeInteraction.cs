using _Project.Scripts;
using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using DialogueEditor;
using UnityEngine;

namespace _ProjectScripts.DialogueSystem.DialogueWithSalerScripts
{
    // скрипт отвечающий за радиус выхода из круга, где игрок прекратит разговаривать с продавцом
    public class RangeInteraction : MonoBehaviour
    {
        [SerializeField] DialogueWithSeller dialogue;
        //[SerializeField] DisableAndEnableMovementAndCursorController disableAndEnableMovementAndCursorController;

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                dialogue.StopDialogue();

                //ConversationManager.Instance.EndConversation();
                //disableAndEnableMovementAndCursorController.EnableMovementAndHideCursor();
            }
        }
    }
}
