using _Project.Scripts.MissionsScripts;
using UnityEngine;

public class TriggerAttackMonster : MonoBehaviour
{
    [SerializeField] ShowMissionsManager showMissionsManager;

    [SerializeField] Transform attackMonster;

    [SerializeField] Animator attackMonsterAnimator;

    //[HideInInspector]
    public bool isHasFollowing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && showMissionsManager.isFindItemsForCarRepair == true) 
        {
            attackMonster.gameObject.SetActive(true);

            isHasFollowing = true;

        }
    }
}
