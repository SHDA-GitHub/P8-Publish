using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    [SerializeField] float DamageToDeal = 1;
    [SerializeField] float speed = 10f;
    [SerializeField] float desTime = 0.1f;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision other)
    {
        //GameObject otherObject = other.gameObject;

        //if (otherObject.CompareTag("Enemy"))
        //{
        //    EnemyHealth enemyHealth = otherObject.GetComponent<EnemyHealth>();

        //    if (enemyHealth != null)
        //    {
        //        enemyHealth.DamageEnemy(DamageToDeal);
        //    }
        //}

        Destroy(gameObject, desTime);
    }
}
