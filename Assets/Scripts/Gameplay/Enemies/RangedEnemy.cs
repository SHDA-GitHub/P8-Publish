using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class RangedEnemy : BaseEnemy
{
    public enum State
    {
        Attacking,
        Chasing,
    }
    public State currentState;

    [Header("References")]
    private Player player;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _projectilePrefab;

    [SerializeField] private Transform pointA;


    [SerializeField] private NavMeshAgent _navMeshAgent;

    [SerializeField] private List<GameObject> _projectiles;

    [Header("Stats")]
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;

    [SerializeField] private bool _canAttack;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
        _target = player.transform;
        _canAttack = true;
    }
    private void Update()
    {

        _navMeshAgent.SetDestination(_target.position);

        for (byte i = 0; i < _projectiles.Count; i++)
        {
            if (_projectiles[i] == null)
            {
                _projectiles.RemoveAt(i);
                i--;
            }
        }

        switch (currentState)
        {
            case State.Chasing:

                if (_projectiles.Count == 0)
                {
                    _navMeshAgent.enabled = true;
                }

                break;
            case State.Attacking:

                _navMeshAgent.enabled = false;
                if (!_canAttack) return;
                StartCoroutine(AttackEnemy());

                break;
        }

        if (Vector3.Distance(_target.position, transform.position) <= _attackRange)
        {
            currentState = State.Attacking;
        }

        if(Vector3.Distance(_target.position, transform.position) >= _attackRange)
        {
            currentState = State.Chasing;
        }

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

        knot1.Position.x = pointA.position.x - _target.position.x;
        knot1.Position.z = pointA.position.z - _target.position.z;
        knot1.Position *= -1;
        knot1.Position.y = 0;

        attackSplineCopy.Spline[0] = knot0;
        attackSplineCopy.Spline[1] = knot1;

        _canAttack = false;
        GameObject projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
        SplineAnimate splineAnimate = projectile.GetComponent<SplineAnimate>();
        splineAnimate.Container = attackSplineCopy;
        splineAnimate.Play();

        _projectiles.Add(projectile);

        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}
