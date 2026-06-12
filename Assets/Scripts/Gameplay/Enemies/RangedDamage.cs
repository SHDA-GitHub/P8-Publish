using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class RangedDamage : BaseEnemy
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private SphereCollider _hitCollider;
    [SerializeField] private SphereCollider _attackCollider;

    [Header("Stats")]
    [SerializeField] private float _attackDamage;

    private void Awake()
    {
        _target = Player.instance.transform;
        StartCoroutine(DestroyProjectile());
    }
    protected override void Attack(float damage, PlayerHealth playerHealth)
    {
        base.Attack(damage, playerHealth);
    }
    private void OnTriggerEnter(Collider collision)
    {

        _renderer.enabled = false;
        _hitCollider.enabled = false;
        _attackCollider.enabled = true;
        print("touched");

        
        if (collision.CompareTag("Player"))
        {
            Attack(_attackDamage, collision.gameObject.GetComponent<PlayerHealth>());
        }
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject.GetComponent<SplineAnimate>().Container);
        Destroy(gameObject);
    }
}
