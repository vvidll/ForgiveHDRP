using UnityEngine;

enum EnemyType 
{
    Default,
    WatcherEnemy,
    AttackEnemy
}

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
