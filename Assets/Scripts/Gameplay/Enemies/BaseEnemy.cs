using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
