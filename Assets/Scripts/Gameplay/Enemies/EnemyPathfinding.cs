using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : BaseEnemy
{

    [SerializeField] private NavMeshAgent _agent;
    public Transform targetObject;
    void Awake()
    {

        _agent = GetComponent<NavMeshAgent>();
        targetObject = Player.instance.transform;
    }
    private void Update()
    {
        _agent.SetDestination(targetObject.position);
    }
}
