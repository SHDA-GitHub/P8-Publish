using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : BaseEnemy
{
    [SerializeField] private NavMeshAgent agent;
    public Transform targetObject;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetObject = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        agent.SetDestination(targetObject.position);
    }
}
