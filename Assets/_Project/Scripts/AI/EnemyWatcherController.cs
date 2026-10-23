using _Project.Scripts.CameraScripts;
using _Project.Scripts.PlacedItemsScripts;
using _Project.Scripts.PlayerScripts;
using UnityEngine;

public class EnemyWatcherController : Enemy
{
    [SerializeField] internal SleepingBagController sleepingBagController;

    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] CameraController cameraController;

    public bool isActiveEnemyWatcher = false;

    public override void LookAtPlayer()
    {
        if (playerMovement.isHasKeyboardInput == true)
        {
            animatorEnemy.SetBool("isWalking", true);
            transform.LookAt(player);
        }

        else
        {
            animatorEnemy.SetBool("isWalking", false);
        }
    }

    public void DisableEnemy() 
    {
        if (sleepingBagController.isChangeTimeDay == true
            && isActiveEnemyWatcher == true) 
        {
            this.gameObject.SetActive(false);
            isActiveEnemyWatcher = false;
        }
    }
}
