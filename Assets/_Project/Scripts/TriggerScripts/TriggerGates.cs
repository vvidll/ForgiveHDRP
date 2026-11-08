using UnityEngine;

namespace _Project.Scripts.TriggerScripts
{
    public class TriggerGates : MonoBehaviour
    {
        [SerializeField] Animator playerAnimator;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerAnimator.SetBool("isCloseGates", true);
            }
        }
    }
}