using UnityEngine;
using UnityEngine.AI;

public class SpawnerPlacement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _distanceFromPlayer = new Vector3(0,0,0);
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _player = Player.instance.transform;
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(_player.position + _distanceFromPlayer);
    }
}
