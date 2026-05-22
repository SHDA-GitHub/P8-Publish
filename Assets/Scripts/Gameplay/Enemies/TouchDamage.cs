using UnityEngine;

public class TouchDamage : BaseEnemy
{
    public float _attackDamage = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            Attack(_attackDamage, playerHealth);
        }
    }
    protected override void Attack(float damage, PlayerHealth enemyHealth)
    {
        base.Attack(damage, enemyHealth);
        enemyHealth.health -= damage;
    }
}
