using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    protected virtual void Attack(float damage, PlayerHealth playerHealth)
    {
        playerHealth.health -= damage;
    }
}
