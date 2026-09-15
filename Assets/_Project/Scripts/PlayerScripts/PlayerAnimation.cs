using _Project.Scripts.InterfaceScripts;
using _Project.Scripts.CameraScripts;
using UnityEngine;
using _Project.Scripts.PlayerScripts;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] StaminaSliderController staminaSliderController;
    [SerializeField] CameraController cameraController;
    [SerializeField] PlayerMovement playerMovement;

    float smoothTime = 0.15f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationWalk(float verticalDirection, float horizontalDirection)
    {
        if(verticalDirection == 0 && horizontalDirection == 0)
            CallIdleAnimation();

        if (verticalDirection != 0)
        {
            if (verticalDirection > 0)
                animator.SetFloat("y", verticalDirection = 1f, smoothTime, Time.deltaTime); 

            else
                animator.SetFloat("y", verticalDirection = -1f, smoothTime, Time.deltaTime);
        }

        else
            animator.SetFloat("y", 0f, smoothTime, Time.deltaTime);

        if (horizontalDirection != 0)
        {
            if (horizontalDirection > 0)
                animator.SetFloat("x", horizontalDirection = 1f, smoothTime, Time.deltaTime);

            else
                animator.SetFloat("x", horizontalDirection = -1f, smoothTime, Time.deltaTime);
        }

        else
            animator.SetFloat("x", 0f, smoothTime, Time.deltaTime);
    }

    public void ChangeAnimationRun(float verticalDirection)
    {
        if(verticalDirection > 0 && Input.GetKey(KeyCode.LeftShift))
            animator.SetFloat("y", 1.5f, smoothTime, Time.deltaTime);
    }

    public void ChangeTurnAnimation()
    {
        if (cameraController.mouseX != 0 && 
            playerMovement.inputKeyboard.x == 0 && 
            playerMovement.inputKeyboard.z == 0)
        {
            animator.SetBool("isTurn", true);

            if (cameraController.mouseX > 0)
            {
                animator.SetFloat("xTurn", 1f, smoothTime, Time.deltaTime);
            }

            else
            {
                animator.SetFloat("xTurn", -1f, smoothTime, Time.deltaTime);
            }
        }

        else
        {
            animator.SetBool("isTurn", false);
        }
    }

    public void CallIdleAnimation() 
    {
        animator.SetFloat("y", 0f);
        animator.SetFloat("x", 0f);

        staminaSliderController.IncreasedStaminaIdle();
        
        playerMovement.isHasKeyboardInput = false;
    }
}
