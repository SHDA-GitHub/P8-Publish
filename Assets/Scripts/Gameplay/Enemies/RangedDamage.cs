using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class RangedDamage : BaseEnemy
{
    [Header("References")]
    [SerializeField] private RangedProjectile _rangedProjectileScript;

    [Header("Stats")]
    [SerializeField] private float _attackDamage;

    private void Awake()
    {
        _attackDamage = _rangedProjectileScript._attackDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            Attack(_attackDamage, playerHealth);
        }
    }

    protected override void Attack(float damage, PlayerHealth playerHealth)
    {

        base.Attack(damage, playerHealth);

    }


}
