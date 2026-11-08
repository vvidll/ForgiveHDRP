using UnityEngine;

enum EnemyType 
{
    Default,
    WatcherEnemy,
    AttackEnemy
}

namespace _Project.Scripts.AI
{
    public class Enemy : MonoCache
    {
        [SerializeField] internal EnemyType enemyType = EnemyType.Default;

        [SerializeField] protected Animator animatorEnemy;

        [SerializeField] protected Transform player;

        public virtual void LookAtPlayer() { }

        public override void OnTick()
        {
            LookAtPlayer();
        }
    }
}