using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class RangedProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject _attackExplosion;

    [SerializeField] private Vector3 _lowestPosition;

    [Header("Stats")]
    [SerializeField] private float _destroyTime;
    public float _attackDamage;

    private void Awake()
    {
        _target = Player.instance.transform;
        StartCoroutine(DestroyProjectile());
        _attackExplosion.SetActive(false);
    }

    private void Update()
    {
        if (transform.position.y <= _lowestPosition.y)
        {
            StartCoroutine(AttackCoroutine());

        }
    }

    private void DestroySpline()
    {
        Destroy(gameObject.GetComponent<SplineAnimate>().Container);
        Destroy(gameObject.GetComponent<SplineAnimate>());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        _renderer.enabled = false;
        _attackExplosion.SetActive(true); ;
    }

    private void OnDestroy()
    {
        DestroySpline();
    }
}
