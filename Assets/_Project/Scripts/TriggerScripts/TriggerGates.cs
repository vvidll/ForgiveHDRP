using UnityEngine;

public class TriggerGates : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            playerAnimator.SetBool("isCloseGates", true);
        }
    }
}
