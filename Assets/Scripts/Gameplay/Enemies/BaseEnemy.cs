using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected enum EnemyState
    {
        Chasing,
        Attacking,
        Dead
    }

    protected EnemyState currentState = EnemyState.Chasing;

    private void Start()
    {
        currentState = EnemyState.Chasing;
    }
    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Chasing:
                //when chase

                break;
            case EnemyState.Attacking:
                //when attack

                break;
            case EnemyState.Dead:
                //when dead
                    Death();
                break;
        }
    }
    protected virtual void Death()
    {
        currentState = EnemyState.Dead;
    }
}
