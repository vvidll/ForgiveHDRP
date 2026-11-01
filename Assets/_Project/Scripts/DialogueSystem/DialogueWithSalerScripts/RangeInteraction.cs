using _Project.Scripts.DialogueSystem.DialogueWithSalerScripts;
using UnityEngine;

namespace _ProjectScripts.DialogueSystem.DialogueWithSalerScripts
{
    // скрипт отвечающий за радиус выхода из круга, где игрок прекратит разговаривать с продавцом
    public class RangeInteraction : MonoBehaviour
    {
        [SerializeField] DialogueWithSaler dialogue;

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                dialogue.StopDialogue();
            }
        }
    }
}
