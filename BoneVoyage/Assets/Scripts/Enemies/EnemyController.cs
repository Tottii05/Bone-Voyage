using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour, IDamageable
{
    public int HP;
    public GameObject target;
    public bool OnAttackRange = false, escape = false, chase = false;
    public EnemyPathFinding _chaseB;
    public StateSO currentState;
    public List<StateSO> States;
    public float AttackRange = 2f;
    public Vector3 lastPlayerPosition;
    public EnemyPathFinding Pathfinding;
    public Animator animator;

    void Start()
    {
        Pathfinding = GetComponent<EnemyPathFinding>();
        _chaseB = GetComponent<EnemyPathFinding>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnAttackRange = true;
            CheckEndingConditions();
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnAttackRange = false;
            CheckEndingConditions();
        }
    }

    private void Update()
    {
        if (target != null)
        {
            OnAttackRange = Vector3.Distance(transform.position, target.transform.position) < AttackRange;
        }
        else
        {
            OnAttackRange = false;
        }
        currentState.OnStateUpdate(this);
    }

    public void CheckEndingConditions()
    {
        foreach (ConditionSO condition in currentState.EndConditions)
        {
            if (condition.CheckCondition(this) == condition.answer) ExitCurrentNode();
        }
    }

    public void ExitCurrentNode()
    {
        foreach (StateSO stateSO in States)
        {
            if (stateSO.StartCondition == null || stateSO.StartCondition.CheckCondition(this) == stateSO.StartCondition.answer)
            {
                EnterNewState(stateSO);
                break;
            }
        }
        currentState.OnStateEnter(this);
    }

    private void EnterNewState(StateSO state)
    {
        currentState.OnStateExit(this);
        currentState = state;
        currentState.OnStateEnter(this);
    }

    public void TakeDamage(float damage)
    {
        HP -= (int)damage;
        if (HP > 0)
        {
            animator.SetTrigger("hit");
        }
        if (HP <= 25)
        {
            escape = true;
        }
        CheckEndingConditions();
    }


    public IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }
}
