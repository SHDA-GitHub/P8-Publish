using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    public int DamageToDeal = 1;
    public float speed = 10f;
    [SerializeField] private float desTime = 0.1f;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, 2);
    }

    private void Update()
    {
        Destroy(gameObject, desTime);
    }

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
        Destroy(gameObject);
    }
}
    