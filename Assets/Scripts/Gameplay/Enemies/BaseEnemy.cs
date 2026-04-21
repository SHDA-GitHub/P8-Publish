using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    protected enum EnemyState
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Dead
    }

    protected EnemyState currentState = EnemyState.Idle;



}
