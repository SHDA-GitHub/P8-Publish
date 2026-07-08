using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class RangedProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject _attackExplosion;
    [SerializeField] private GameObject _particles;

    [SerializeField] private Vector3 _lowestPosition;

    [Header("Stats")]
    [SerializeField] private float _destroyTime;
    public float _attackDamage;
    private bool isExploding = false;

    private void Awake()
    {
        _target = Player.instance.transform;
        _attackExplosion.SetActive(false);
    }

    private void Update()
    {
        if (transform.position.y <= _lowestPosition.y)
        {
            StartCoroutine(AttackCoroutine());
            _attackExplosion.SetActive(true);
        }
    }

    private void DestroySpline()
    {
        Destroy(gameObject.GetComponent<SplineAnimate>().Container);
        Destroy(gameObject.GetComponent<SplineAnimate>());
    }

    private IEnumerator AttackCoroutine()
    {
        _renderer.enabled = false;

        GameObject _newParticles = Instantiate(_particles, gameObject.transform);
        _newParticles.AddComponent<RangedExplosionParticle>();

        yield return new WaitForSeconds(0.1f);

        _attackExplosion.SetActive(false);
        _newParticles.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        DestroySpline();
    }
}
