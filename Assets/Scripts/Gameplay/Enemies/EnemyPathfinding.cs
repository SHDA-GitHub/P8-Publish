using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : BaseEnemy
{

    [SerializeField] private NavMeshAgent _agent;
    private Transform _targetObject;
    void Start()
    {
        Transform target = Player.instance.transform;
        print(target);
        _agent = GetComponent<NavMeshAgent>();
        _targetObject = target;
    }
    private void Update()
    {
        _agent.SetDestination(_targetObject.position);
    }
}
