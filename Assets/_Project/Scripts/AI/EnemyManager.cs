using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyWatcherController enemyWatcher;
    
    private void Update()
    {
        if (enemyWatcher.enemyType == EnemyType.WatcherEnemy && enemyWatcher.sleepingBagController.isChangeTimeDay == true
            && enemyWatcher.isActiveEnemy == false) 
        {
            enemyWatcher.gameObject.SetActive(true);
            enemyWatcher.isActiveEnemy = true;
        }
            
        

    }
}
