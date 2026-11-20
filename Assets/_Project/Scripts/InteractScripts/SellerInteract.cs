using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using _Project.Scripts.TextScripts;
using DialogueEditor;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.InteractScripts
{
    // скрипт отвечающий за взаимодействие с продавцом
    public class SellerInteract : MonoBehaviour, IInteractable
    {
        [Header("Dialogue")]

        [SerializeField] NPCConversation npcConversation;

        [SerializeField] DialogueWithSeller dialogue;
        [SerializeField] TypingText typingText;
        
        [SerializeField] TMP_Text tmpTextTalkWithSaler;

        //[SerializeField] DisableAndEnableMovementAndCursorController disableAndEnableMovementAndCursorController;

        public int countStartDialogue = 0;

        private void Awake()
        {
            //if (PlayerPrefs.HasKey(CommonDialogueWithSeller.CurrentDialogueIndexKey))
              //  DialogueWithSeller.currentDialogueIndex = CommonDialogueWithSeller.LoadCurrentDialogue(DialogueWithSeller.currentDialogueIndex);
        }
        public void Interact()
        {
            // Dialogue system
            dialogue.StartDialogue();

            //ConversationManager.Instance.StartConversation(npcConversation);
            //disableAndEnableMovementAndCursorController.DisableMovementAndShowCursor();

            Debug.Log("Interact with seller");
        }

        public void Description() 
        {
            tmpTextTalkWithSaler.text = "Talk with seller";
            //return tmpTextTalkWithSaler.text;
        }

        /*public void ChangeLayerInteractionToDefault() 
        {
            if (this.gameObject.layer == LayerMask.NameToLayer("Interaction"))
                this.gameObject.layer = LayerMask.NameToLayer("Default");
        }*/
    }
}

