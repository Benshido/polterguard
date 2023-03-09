using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float alertedTimer = 5f;
    [SerializeField] string state = "unaggroed"; //the state of the boss
    private int stateTimer = 0; //how many frames in the current state there are
    private string prevState = "unaggroed"; //last state
    [SerializeField] float meleeRange = 0f; //how far away the boss decides to melee you
    [SerializeField] bool hasEnraged = false; //Whether or not the boss has been below 50% hp
    private float maxHP;

    /// <summary>
    /// Those with highest range will be used first, otherwise first in list has highest priority
    /// </summary>
    [SerializeField] EnemyAttack[] attacks;
    [Tooltip("Make sure to use correct animation transition conditions")]
    private int currentAttackType = 0;
    private bool hasAvailableAtk = false;

    [Header("Hearing")]
    [SerializeField] public float baseHearingRange = 2f;
    [SerializeField] public float hearingAlertedStateIncrease = 8f;
    [SerializeField] float hearingRange = 0f;

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
    public GameObject meleeHitbox;
    public GameObject CDHitbox;
    public GameObject LaserHitbox;

    private int provokedCount = 0;
    private int laserdir = 0;
    private bool Provoked = false;
    private bool gotmaxhp = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hearingRange = baseHearingRange;
        visionRange = baseVisionRange;
        myLife = GetComponent<EnemyHP>();
        if(myLife == null )
        {
            myLife = GetComponentInChildren<EnemyHP>();
        }
        anim = GetComponentInChildren<Animator>();

        if (target == null) target = FindFirstObjectByType<PlayerHP>().transform;

        StopAgent();

        attacks.OrderByDescending(x => x.Range);

        foreach (EnemyAttack attack in attacks)
        {
            attack.FullAmmo();
        }
    }
    private void FixedUpdate()
    {
        if (!gotmaxhp)
        {
            gotmaxhp= true;
            maxHP = myLife.HitPoints;
            Debug.Log(maxHP);
        }
        stateTimer++;
        if(state != prevState)
        {
            stateTimer = 0;
        }
        if ((stateTimer > 55 && stateTimer < 140) && state == "laserAttack" && stateTimer%2 == 0)
        {
            if (Vector3.Cross(transform.forward, target.position - transform.position).y > 0)
            {
                laserdir = 1;
            }
            else { laserdir = 0; }
            CreateLaserHitbox();
            if (laserdir == 0)
            {
                transform.Rotate(0, -2f, 0);
            }
            else
            {
                transform.Rotate(0, 2f, 0);
            }
        }
        prevState = state;
    }
    void Update()
    {
        if (myLife != null && myLife.IsAlive)
        {
            distanceToTarg = Vector3.Distance(target.position, transform.position);

            if (distanceToTarg <= hearingRange || HasVisualOnPlayer()) Provoked = true;
            else Provoked = false;

            //switches between states based on conditions
            //boss moves toward player. If player is too far away it will either defend or use ranged attack


            //what all the states should do
            switch (state)
            {
                case "unaggroed":
                    anim.SetTrigger("Idle");
                    break;
                case "idle":
                    EnableHurtbox();
                    anim.SetTrigger("Idle");
                    anim.SetBool("Attack", false);
                    StopMoving();
                    if((myLife.HitPoints <= maxHP / 2) && !hasEnraged)
                    {
                        Debug.Log("half health");
                        state = "enrage";
                    }
                    if (stateTimer >= 12)
                    {
                        int random = Random.Range(0, 3);
                        switch (random)
                        {
                            case 0: state = "idle"; break; //state = idle
                            case 1: state = "movingToPlayer"; break;
                            case 2: state = "idle"; break;
                        }
                    }
                    break;
                case "movingToPlayer":
                    ChaseTarget();
                    //anim.SetTrigger("Move"); 
                    if (stateTimer >= 60)
                    {
                        if (distanceToTarg > meleeRange)
                        {
                            int random = Random.Range(0, 5);
                            if (random == 0)
                            {
                                state = "cdAttack";
                            }
                            else if (random < 4)
                            {
                                if (hasEnraged)
                                {
                                    if (random == 3){
                                        state = "laserAttack";
                                    }
                                    else stateTimer = 0;
                                }
                            }
                            else
                            {
                                state = "defending";
                            }
                        }
                        else
                        {
                            state = "meleeAttack";
                        }
                    }
                    break;
                case "cdAttack":
                    Debug.Log("cdAttack");
                    StopMoving();
                    if (stateTimer == 1) { FacePlayer(); }
                    //state = "idle"; 
                    anim.SetBool("Attack", true);
                    anim.SetInteger("AttackType", 2);
                    break;
                case "defending":
                    Debug.Log("defend");
                    StopMoving();
                    FacePlayer();
                    anim.SetBool("Attack", true);
                    anim.SetInteger("AttackType", 1);
                    //state = "idle";
                    break;
                case "meleeAttack":
                    Debug.Log("melee");
                    StopMoving();
                    anim.SetBool("Attack", true);
                    //state = "idle";
                    anim.SetInteger("AttackType", 0);
                    break;
                case "laserAttack":
                    StopMoving();
                    anim.SetBool("Attack", true);
                    anim.SetInteger("AttackType", 3);
                    if(stateTimer == 1)
                    {
                        FacePlayer();
                        //laserdir = Random.Range(0, 2);
                    }

                    break;
                case "enrage":
                    StopMoving();
                    DisableHurtbox();
                    anim.SetBool("Attack", true);
                    anim.SetInteger("AttackType", 4);
                    hasEnraged = true;
                    break;


                default: Debug.Log("bad state"); break;
            }

            /*if (Provoked)
            {
                StartCoroutine(AlertedStateTimer());
                SetAttackType();
                if (distanceToTarg <= agent.stoppingDistance)
                {
                    Attack();
                }
                else if (distanceToTarg <= hearingRange || HasVisualOnPlayer()) ChaseTarget();

            }
            else anim.SetTrigger("Idle");*/
        }
        else {
            anim.SetBool("Alive", false);
            StopMoving();
            Debug.Log("dood");
            DisableHurtbox();
        }
    }

    private void EnableHurtbox()
    {
        var hurtbox = GetComponentInChildren<SphereCollider>();
        hurtbox.enabled = true;
    }

    private void DisableHurtbox()
    {
        var hurtbox = GetComponentInChildren<SphereCollider>();
        hurtbox.enabled = false;
    }

    private void CreateMeleeHitbox() {
        GameObject hitbox = Instantiate(meleeHitbox);
        hitbox.transform.position = transform.position+transform.forward*1;
        hitbox.transform.rotation = transform.rotation;
    }
    private void CreateRangedHitbox()
    {
        GameObject hitbox = Instantiate(CDHitbox);
        hitbox.transform.position = transform.position + transform.forward * 1 +transform.right*-1+transform.up*3f; //ik this is bad practice but i will never use this code angain anyway
        hitbox.transform.LookAt(target.position+Vector3.up*1);
        //Debug.Log(hitbox.transform.rotation.eulerAngles);
        //Debug.Log(hitbox.transform.rotation.eulerAngles);
    }
    private void CreateLaserHitbox()
    {
        GameObject hitbox = Instantiate(LaserHitbox);
        hitbox.transform.position = transform.position + transform.forward * 0.1f+transform.up*1.3f;
        hitbox.transform.rotation = transform.rotation;
    }
    private void FacePlayer()
    {
        //Vector3 dirToTarg = (target.position - transform.position);
        transform.LookAt(target.position);
    }
    public void endAttack()
    {
        anim.SetBool("Attack", false);
        state = "idle";
    }
    public void performLaser()
    {
        state = "laserAttack";
    }
    private void StopMoving()
    {
        agent.SetDestination(transform.position);
    }
    private void RunAway()
    {
        
        agent.SetDestination(transform.position-transform.forward*10);
    }
    private bool HasVisualOnPlayer()
    {
        Collider[] overlap = Physics.OverlapSphere(transform.position, visionRange);
        if (overlap.Contains(target.gameObject.GetComponentInChildren<Collider>()))
        {
            Vector3 dirToTarg = (target.position - transform.position);

            if (Vector3.Angle(dirToTarg, transform.forward) < visionAngle &&
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

            anim.SetTrigger("Move");
        }
        anim.SetBool("Attack", false);
    }

    private void Attack()
    {
        if (hasAvailableAtk)
        {
            var lookat = target.position;
            lookat.y = transform.position.y;
            transform.LookAt(lookat);

            anim.SetBool("Attack", true);
            anim.SetInteger("AttackType", currentAttackType);
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// Set an event triger on the animation and put this method in there
    /// </summary>
    public void UseAmmoNoProjectile()
    {
        var atk = attacks[currentAttackType];
        if (atk.Ammo > 0 || atk.HasInfiniteAmmo)
        {
            atk.UseAmmo();
            StartCoroutine(atk.Reload());
            HitboxMeleeAtk(atk);
        }
    }

    public void UseAmmoWithProjectile()
    {
        var atk = attacks[currentAttackType];
        if (atk.Ammo > 0 || atk.HasInfiniteAmmo)
        {
            atk.UseAmmo();
            StartCoroutine(atk.Reload());
            FireProjectile(atk);
        }
    }

    public LayerMask AttackMask;
    private void FireProjectile(EnemyAttack atk)
    {
        if (atk.Projectile != null)
        {
            var obj = Instantiate(atk.Projectile, transform);
            obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
            obj.transform.SetParent(null);
            var projectile = obj.AddComponent<EnemyProjectile>();
            projectile.speed = atk.projectileSpeed;
            projectile.damage = atk.Damage;
            projectile.range = atk.Range;
            projectile.target = target.position;
            projectile.mask = AttackMask;
        }
    }

    private void HitboxMeleeAtk(EnemyAttack atk)
    {
        if (atk.MeleeHitArea != null)
        {
            atk.MeleeHitArea.enabled = true;
            var melee = atk.MeleeHitArea.GetComponent<EnemyMeleeAtk>();
            if (melee == null)
            {
                melee = atk.MeleeHitArea.AddComponent<EnemyMeleeAtk>();
                melee.damage = atk.Damage;
            }
        }
    }
    public void MeleeHitboxOff(int attackType)
    {
        var atk = attacks[attackType];
        if (atk.MeleeHitArea != null)
        {
            atk.MeleeHitArea.enabled = false;
            var melee = atk.MeleeHitArea.GetComponent<EnemyMeleeAtk>();
            melee.damage = atk.Damage;
            melee.ClearHPObjectsHit();
        }
    }

    private void SetStoppingDistance()
    {
        agent.stoppingDistance = attacks[currentAttackType].Range;
    }

    /// <summary>
    /// Can be called through animation triggers
    /// </summary>
    public void StartAgent()
    {
        agent.isStopped = false;
    }
    /// <summary>
    /// Can be called through animation triggers
    /// </summary>
    public void StopAgent()
    {
        agent.isStopped = true;
    }

    private void SetAttackType()
    {
        SetStoppingDistance();

        hasAvailableAtk = false;
        for (int i = 0; i < attacks.Length; i++)
        {
            var atk = attacks[i];
            if (atk.Ammo > 0)
            {
                currentAttackType = atk.AnimationIndex;
                hasAvailableAtk = true;
                break;
            }
            if (atk.HasInfiniteAmmo)
            {
                currentAttackType = atk.AnimationIndex;
                hasAvailableAtk = true;
            }
        }
    }

    /// <summary>
    /// leaves range altered for set amount of seconds and then resets to base range
    /// </summary>
    /// <returns></returns>
    private IEnumerator AlertedStateTimer()
    {
        if (provokedCount <= 0)
        {
            provokedCount++;
            yield return new WaitUntil(() => Provoked == false);
            provokedCount--;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            yield return new WaitForSeconds(alertedTimer);

            if (provokedCount == 0)
            {
                hearingRange = baseHearingRange;
                visionRange = baseVisionRange;
            }
        }
    }
}
