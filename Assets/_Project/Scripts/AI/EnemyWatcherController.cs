using System;
using _Project.Scripts.PlacedItemsScripts;
using _Project.Scripts.PlayerScripts;
using UnityEngine;

public class EnemyWatcherController : Enemy
{
    [SerializeField] internal SleepingBagController sleepingBagController;

    [SerializeField] Transform playerTransform;
    [SerializeField] PlayerMovement playerMovement;

    private void Update()
    {
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        if (playerMovement.isHasKeyboardInput == true)
        {
            animatorEnemy.SetBool("isWalking", true);
            transform.LookAt(playerTransform);
        }

        else
        {
            animatorEnemy.SetBool("isWalking", false);
        }
    }
}
