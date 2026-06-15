using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public ushort damageToDeal = 1;
    public float knockbackStrength = 1f;
    public float upwardKnockback = 1f;
    public bool isEnemy = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isEnemy == false)
        {
            GameObject otherObject = other.gameObject;

            if (otherObject.CompareTag("Enemy"))
            {
                Debug.Log($"{gameObject.name} hit {other.name}");
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
        else if (isEnemy == true)
        {
            GameObject otherObject = other.gameObject;

            if (otherObject.CompareTag("Player"))
            {
                Debug.Log($"{gameObject.name} hit {other.name}");
                PlayerHealth playerHealth = otherObject.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.health -= damageToDeal;
                }

                Rigidbody playerRb = otherObject.GetComponent<Rigidbody>();

                if (playerRb != null)
                {
                    Vector3 horizontalDirection =
                        (otherObject.transform.position - transform.position).normalized;

                    horizontalDirection.y = 0;
                    horizontalDirection.Normalize();
                    playerRb.AddForce(Vector3.up * upwardKnockback, ForceMode.Impulse);
                    playerRb.AddForce(horizontalDirection * knockbackStrength, ForceMode.Impulse);
                }
            }
        }
    }
}