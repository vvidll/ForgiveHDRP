using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Scripts.TextScripts
{
    public class TypingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private int speedTyping = 50; // 0.005 in seconds

        internal string isFullText;
        public bool isTyping = false;
        bool isCoroutineStopping = false;

        public void UpdateText()
        {
            try
            {
                isFullText = dialogueText.text;
                dialogueText.text = "";

                isTyping = true;
                TypingTextCoroutine().Forget();
            }
            catch (Exception ex) { Debug.LogError(ex); }

        }

        public async UniTaskVoid TypingTextCoroutine()
        {

            for (int i = 0; i < isFullText.Length && isCoroutineStopping == false; i++)
            {
                try
                {
                    dialogueText.text += isFullText[i];
                    await UniTask.Delay(speedTyping, cancellationToken: this.GetCancellationTokenOnDestroy());
                }

                catch (Exception ex) { Debug.LogError(ex); }

            }

            isTyping = false;
            Debug.Log("Печать закончилась");

        }
        


    }
}
