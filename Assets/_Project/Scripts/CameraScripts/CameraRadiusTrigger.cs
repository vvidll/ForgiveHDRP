using _Project.Scripts.AI;
using UnityEngine;

public class CameraRadiusTrigger : MonoBehaviour
{
    [SerializeField] EnemyWatcherController enemyWatcherController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWatcher"))
        {
            Debug.Log("has contact!");
            enemyWatcherController.DisableEnemy();
        }
    }
}