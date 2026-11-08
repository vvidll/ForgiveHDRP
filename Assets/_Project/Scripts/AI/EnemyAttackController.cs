using _Project.Scripts.TriggerScripts;
using UnityEngine;

namespace _Project.Scripts.AI
{
    public class EnemyAttackController : Enemy
    {
        [SerializeField] TriggerAttackMonster triggerAttackMonster;

        float speedFollowing = .5f;

        public override void LookAtPlayer()
        {
            if (triggerAttackMonster.isHasFollowing == true)
            {
                transform.LookAt(player);

                animatorEnemy.SetTrigger("TriggerWalking");

                transform.position = Vector3.Lerp(transform.position, player.position, speedFollowing * Time.deltaTime);
            }
        }
    }
}