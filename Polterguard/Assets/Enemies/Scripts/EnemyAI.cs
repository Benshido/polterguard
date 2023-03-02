using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] public float baseHearingRange = 2f;
    [SerializeField] public float hearingAlertedStateIncrease = 8f;
    [SerializeField] float hearingRange = 0f;
    [SerializeField] float alertedTimer = 5f;

    [Header("Vision")]
    [SerializeField] public float baseVisionRange = 10f;
    [SerializeField] public float visionAlertedStateIncrease = 5f;
    [SerializeField] float visionRange = 0f;
    [Range(0, 180)][SerializeField] public float visionAngle = 10f;
    [SerializeField] LayerMask ObstructionLayers;

    private EnemyHP myLife;

    private float distanceToTarg = 0;
    private NavMeshAgent agent;
    private Animator anim;

    private int provokedCount = 0;
    private bool Provoked = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hearingRange = baseHearingRange;
        visionRange = baseVisionRange;
        myLife = GetComponent<EnemyHP>();
        anim = GetComponentInChildren<Animator>();

        agent.isStopped = true;
    }

    void Update()
    {
        if (myLife != null && myLife.IsAlive)
        {
            distanceToTarg = Vector3.Distance(target.position, transform.position);

            if (distanceToTarg <= hearingRange || HasVisualOnPlayer()) Provoked = true;
            else Provoked = false;

            if (Provoked)
            {
                StartCoroutine(AlertedStateTimer());
                if (distanceToTarg <= agent.stoppingDistance)
                {
                    BaseAttack();
                }
                else ChaseTarget();
            }
            else anim.SetTrigger("Idle");
        }
    }

    private bool HasVisualOnPlayer()
    {
        Collider[] overlap = Physics.OverlapSphere(transform.position, visionRange);
        if (overlap.Contains(target.gameObject.GetComponentInChildren<Collider>()))
        {
            Vector3 dirToTarg = (target.position - transform.position);
            if ((Vector3.Angle(transform.position, dirToTarg) - transform.eulerAngles.y) < visionAngle &&
                !Physics.Raycast(transform.position, dirToTarg, distanceToTarg, ObstructionLayers))
            {
                return true;
            }
        }
        return false;
    }

    private void ChaseTarget()
    {
        if (!anim.GetBool("Attack"))
        {
            hearingRange = baseHearingRange + hearingAlertedStateIncrease;
            visionRange = baseVisionRange + visionAlertedStateIncrease;
            agent.SetDestination(target.position);
            agent.isStopped = false;

            anim.SetTrigger("Move");
        }
        anim.SetBool("Attack", false);

    }

    private void BaseAttack()
    {
        var lookat = target.position;
        lookat.y = transform.position.y;
        transform.LookAt(lookat);

        anim.SetBool("Attack", true);
    }

    private IEnumerator AlertedStateTimer()
    {
        provokedCount++;
        yield return new WaitUntil(() => Provoked == false);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        yield return new WaitForSeconds(alertedTimer);
        if (provokedCount == 1)
        {
            provokedCount = 0;
            hearingRange = baseHearingRange;
            visionRange = baseVisionRange;
        }
    }
}


