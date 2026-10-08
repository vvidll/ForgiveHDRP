using UnityEngine;

public class TriggerMonsterController : MonoBehaviour
{
    [SerializeField] GameObject treeFall, treeStand;

    [SerializeField] GameObject enemyWatcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            enemyWatcher.SetActive(true);
            treeStand.SetActive(false);
            treeFall.SetActive(true);
        }
    }
}
