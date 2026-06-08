using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public ushort DamageToDeal = 1;

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;

        if (otherObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = otherObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.health -= DamageToDeal;
            }
        }
    }
}