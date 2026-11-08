using _Project.Scripts.MissionsScripts;
using UnityEngine;

namespace _Project.Scripts.TriggerScripts
{
    public class TriggerAttackMonster : MonoBehaviour
    {
        [SerializeField] ShowMissionsManager showMissionsManager;

        [SerializeField] Transform attackMonster;

        [SerializeField] Animator attackMonsterAnimator;

        //[HideInInspector]
        public bool isHasFollowing = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && showMissionsManager.isFindItemsForCarRepair == true)
            {
                attackMonster.gameObject.SetActive(true);

                isHasFollowing = true;

            }
        }
    }
}