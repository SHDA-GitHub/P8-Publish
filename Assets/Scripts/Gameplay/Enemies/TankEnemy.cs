using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : BaseEnemy
{
    [SerializeField] private NavMeshAgent _agent;
    private Transform _targetObject;

    [Header("Tank Attack")]
    [SerializeField] private GameObject slashCollider;
    [SerializeField] private bool slashActive = false;
    [SerializeField] private Animator animator; 
    public bool attacking = false;
    public float knockbackStrength = 1f;
    public float slashDuration = 1f;
    public float introDuration = 1f;
    public float slashHitbox = 1f;
    public float meleeDMG = 12f;

    void Start()
    {
        Transform target = Player.instance.transform;
        _agent = GetComponent<NavMeshAgent>();
        _targetObject = target;

        MeleeCollision melee = slashCollider.GetComponent<MeleeCollision>();

        if (melee != null)
        {
            melee.damageToDeal = (ushort)meleeDMG;
        }
    }

    private void Update()
    {
        if (attacking == false)
        {
            _agent.isStopped = false;
            animator.SetBool("isWalking", true);
            _agent.SetDestination(_targetObject.position);
        }
        else if (attacking == true)
        {
            _agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(introDuration);

        if (slashActive)
            yield break;

        slashActive = true;

        slashCollider.SetActive(true);

        yield return new WaitForSeconds(slashDuration);

        slashCollider.SetActive(false);

        slashActive = false;
        attacking = false;
    }
}
