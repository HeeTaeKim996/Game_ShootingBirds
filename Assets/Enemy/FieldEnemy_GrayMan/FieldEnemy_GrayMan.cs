using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldEnemy_GrayMan : FieldEnemy
{
    private Rigidbody enemyRigidbody;
    private NavMeshAgent navMeshAgent;
    private float health = 20f;
    private float movementSpeed;
    private EnemyAttackTarget enemyAttackTarget;
    private Coroutine attackableCheckCoroutine;
    private Coroutine enemyActionCoroutine;
    private Animator enemyAnimator;
    private string currentEnemyAction;

    public Transform headPivot;

    private const string Walk = "Walk";
    private const string Attack_SwingCross = "Attack_SwingCross";
    private const string Attack_Stab = "Attack_Stab";
    
    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        movementSpeed = Random.Range(4f, 6f);
        if(attackableCheckCoroutine != null)
        {
            StopCoroutine(attackableCheckCoroutine);
        }
        attackableCheckCoroutine = StartCoroutine(Attackablecheck());

        weaknessPivots.Add(new EnemyWeakInformation() { weakTransform = headPivot, weakDistance = 1.2f });
        arrowSpawnDistance = 0.5f;
    }
    public override void GetAttackTarget(EnemyAttackTarget enemyAttackTarget)
    {
        this.enemyAttackTarget = enemyAttackTarget;
    }
    public override void GetDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            UIManager.instance.GetScoreChanged(1);
        }
    }
    private IEnumerator Attackablecheck()
    {
        float checkInterval = 1f;
        while (true)
        {
            float distanceToTarget = Vector3.Distance(transform.position, enemyAttackTarget.transform.position);
            if(distanceToTarget > 3f)
            {
                if(distanceToTarget > 3.5f)
                {
                    if (currentEnemyAction != "Move")
                    {
                        if (enemyActionCoroutine != null)
                        {
                            StopCoroutine(enemyActionCoroutine);
                        }
                        enemyActionCoroutine = StartCoroutine(MoveTowardAttackTarget());
                        currentEnemyAction = "Move";
                    }
                }
                if (distanceToTarget > 6f)
                {
                    checkInterval = 1f;
                }
                else
                {
                    checkInterval = 0.04f;
                }
            }
            else 
            {

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemyAttackTarget.transform.position - transform.position), 3f * Time.deltaTime);
                if(distanceToTarget > 2.7f)
                {
                    transform.position = transform.position + transform.forward * 3f * Time.deltaTime;
                }

                if (currentEnemyAction != "Attack")
                {
                    if(enemyActionCoroutine != null)
                    {
                        StopCoroutine(enemyActionCoroutine);
                    }
                    enemyActionCoroutine = StartCoroutine(AttackTarget());
                    currentEnemyAction = "Attack";
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }
    private IEnumerator MoveTowardAttackTarget()
    {
        enemyAnimator.CrossFade(Walk, 0.2f);
        yield return null; // 무브먼트 스피드 등 세팅 기다렸다 하는게 좋을 것 같아서 한프레임 쉼
        if (!navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
        }
        navMeshAgent.speed = movementSpeed;
        navMeshAgent.SetDestination(enemyAttackTarget.transform.position);
        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator AttackTarget()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.enabled = false;
        }

        while (true)
        {
            int randomInt = Random.Range((int)0, (int)2);
            if (randomInt == 0)
            {
                enemyAnimator.CrossFade(Attack_SwingCross, 0.4f,0, 0f);
                float duration = 2f; float damagingTime = duration * 0.490f; float elapsedTime = 0f;
                bool didDamage = false;


                while(elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    
                    if(elapsedTime >= damagingTime && !didDamage)
                    {
                        enemyAttackTarget.GetDamage(35f);
                        didDamage = true;
                    }

                    yield return null;
                }
            }
            else
            {
                enemyAnimator.CrossFade(Attack_Stab, 0.4f,0, 0f);
                float elapsedTime = 0; float duration = 1.825f; float damagingTime = duration * 0.498f;
                bool didDamage = false;
                while(elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    if(elapsedTime >= damagingTime && !didDamage)
                    {
                        enemyAttackTarget.GetDamage(35f);
                        didDamage = true;
                    }

                    yield return null;
                }
            }
        }
    }
}
