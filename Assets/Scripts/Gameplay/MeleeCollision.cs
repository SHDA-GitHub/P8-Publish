using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public ushort damageToDeal = 1;
    public float knockbackStrength = 1f;
    public float upwardKnockback = 1f;

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;

        if (otherObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = otherObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.health -= damageToDeal;
            }

            Rigidbody enemyRb = otherObject.GetComponent<Rigidbody>();

            if (enemyRb != null)
            {
                Vector3 horizontalDirection =
                    (otherObject.transform.position - transform.position).normalized;

                horizontalDirection.y = 0;
                horizontalDirection.Normalize();
                enemyRb.AddForce(Vector3.up * upwardKnockback, ForceMode.Impulse);
                enemyRb.AddForce(horizontalDirection * knockbackStrength, ForceMode.Impulse);
            }
        }
    }
}