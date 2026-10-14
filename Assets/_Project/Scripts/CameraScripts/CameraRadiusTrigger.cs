using UnityEngine;

public class CameraRadiusTrigger : MonoBehaviour
{
    [SerializeField] EnemyWatcherController enemyWatcherController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyWatcher")
        {
            Debug.Log("has contact!");
            enemyWatcherController.DisableEnemy();
        }
    }
}