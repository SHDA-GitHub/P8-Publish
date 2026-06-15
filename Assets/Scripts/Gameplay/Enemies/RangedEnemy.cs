using System.Collections;
using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class RangedEnemy : BaseEnemy
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private SplineContainer _attackSpline;
    [SerializeField] private GameObject _projectilePrefab;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Stats")]
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;

    [SerializeField] private bool _canAttack;

    private void Awake()
    {
        _canAttack = true;
    }
    private void Update()
    {
        if (Vector3.Distance(_target.position, pointA.position) >= _attackRange || !_canAttack) return;

        StartCoroutine(AttackEnemy());
    }
    private IEnumerator AttackEnemy()
    {
        SplineContainer attackSplineCopy = gameObject.AddComponent<SplineContainer>();
        attackSplineCopy.Spline.Add(new BezierKnot());
        attackSplineCopy.Spline.Add(new BezierKnot());

        BezierKnot knot0 = attackSplineCopy.Spline[0];
        BezierKnot knot1 = attackSplineCopy.Spline[1];

        knot0.TangentIn = new Vector3(0, 0, -50);
        knot0.TangentOut = new Vector3(0, 0, 50);
        knot0.Rotation = Quaternion.Euler(270, 0, 0);

        knot1.Position.x = pointA.position.x - pointB.position.x;
        knot1.Position.y = 3;
        knot1.Position.z = pointA.position.z - pointB.position.z;
        knot1.Position *= -1;

        attackSplineCopy.Spline[0] = knot0;
        attackSplineCopy.Spline[1] = knot1;

        _canAttack = false;
        GameObject projectile = Instantiate(_projectilePrefab, transform);
        SplineAnimate splineAnimate = projectile.GetComponent<SplineAnimate>();
        splineAnimate.Container = attackSplineCopy;
        splineAnimate.Play();

        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}
