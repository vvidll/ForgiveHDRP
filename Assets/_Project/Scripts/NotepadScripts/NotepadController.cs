using UnityEngine;

namespace _Project.Scripts.NotepadScripts
{
    public class NotepadController : DisableAndEnableMovementAndCursorController
    {
        [Header("Player Animation")]
        [SerializeField] PlayerAnimation playerAnimation;

        [SerializeField] Animator animator;
        int keyPress = 0;

        public override void OnTick()
        {
            OpenAndCloseNotepad();
        }

        public void OpenAndCloseNotepad()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                switch (keyPress)
                {
                    case 0:
                        keyPress = 1;
                        animator.SetTrigger("isOpenNotepad");

                        DisableScripts();
                        ShowCursor();

                        playerAnimation.CallIdleAnimation();
                        break;

                    case 1:
                        keyPress = 0;
                        animator.SetTrigger("isCloseNotepad");

                        EnableScripts();
                        HideCursor();

                        playerAnimation.CallIdleAnimation();
                        break;
                }
            }
        }
    }
}
