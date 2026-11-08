using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using _Project.Scripts.TextScripts;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.InteractScripts
{
    // скрипт отвечающий за взаимодействие с продавцом
    public class SellerInteract : MonoBehaviour, IInteractable
    {
        [Header("Dialogue")]

        [SerializeField] DialogueWithSaler dialogue;
        [SerializeField] TypingText typingText;
        
        [SerializeField] TMP_Text tmpTextTalkWithSaler;

        private void Awake()
        {
            if (PlayerPrefs.HasKey(CommonDialogueWithSaler.CurrentDialogueIndexKey))
                DialogueWithSaler.currentDialogueIndex = CommonDialogueWithSaler.LoadCurrentDialogue(DialogueWithSaler.currentDialogueIndex);
        }

        public void Interact()
        {
            // Dialogue system
            dialogue.StartDialogue();

            Debug.Log("Interact with seller");
        }

        public void Description() 
        {
            tmpTextTalkWithSaler.text = "Talk with seller";
            //return tmpTextTalkWithSaler.text;
        } 
    }
}

