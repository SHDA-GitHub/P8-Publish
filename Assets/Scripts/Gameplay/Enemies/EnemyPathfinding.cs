using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : BaseEnemy
{
    [SerializeField] private NavMeshAgent agent;
    public Transform targetObject;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    protected override void Update()
    {
        if(currentState != EnemyState.Chasing)
        {
            return;
        }
        agent.SetDestination(targetObject.position);
    }
}
