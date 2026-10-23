using _Project.Scripts.MissionsScripts;
using UnityEngine;

public class TriggerCheckCar : MonoBehaviour
{
    [SerializeField] GameObject treeFall, treeStand;

    [SerializeField] EnemyWatcherController enemyWatcher;

    [SerializeField] ShowMissionsManager showMissionsManager;

    bool isActiveTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && showMissionsManager.isNeedCarCheck == true && isActiveTrigger == false) 
        {
            enemyWatcher.gameObject.SetActive(true);
            enemyWatcher.isActiveEnemyWatcher = true;

            treeStand.SetActive(false);
            treeFall.SetActive(true);

            isActiveTrigger = true;
        }
    }
}
