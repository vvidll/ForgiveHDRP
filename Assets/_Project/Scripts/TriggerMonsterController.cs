using _Project.Scripts.MissionsScripts;
using UnityEngine;

public class TriggerMonsterController : MonoBehaviour
{
    [SerializeField] GameObject treeFall, treeStand;

    [SerializeField] GameObject enemyWatcher;

    [SerializeField] ShowMissionsManager showMissionsManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && showMissionsManager.isNeedCarCheck == true) 
        {
            enemyWatcher.SetActive(true);
            treeStand.SetActive(false);
            treeFall.SetActive(true);
        }
    }
}
