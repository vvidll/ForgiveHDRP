using UnityEngine;

enum EnemyType 
{
    Default,
    WatcherEnemy,
    AttackEnemy
}

public class Enemy : MonoBehaviour
{
    [SerializeField] internal EnemyType enemyType = EnemyType.Default;

    [SerializeField] protected Animator animatorEnemy;

    [HideInInspector]
    public bool isActiveEnemy = false;
}
