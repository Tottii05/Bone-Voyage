using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour, IDamageable
{
    public int HP;
    public GameObject target;
    public bool OnVisionRange = false, OnAttackRange = false, escape = false;
    public EnemyPathFinding _chaseB;
    public StateSO currentState;
    public List<StateSO> States;
    public float AttackRange = 2f;
    public Vector3 lastPlayerPosition;
    public EnemyFOV EnemyFOV;
    public EnemyPathFinding Pathfinding;

    void Start()
    {
        EnemyFOV = GetComponent<EnemyFOV>();
        Pathfinding = GetComponent<EnemyPathFinding>();
        _chaseB = GetComponent<EnemyPathFinding>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            _chaseB.target = target;
            OnVisionRange = true;
            lastPlayerPosition = EnemyFOV.GetLastPlayerPosition(target);
            CheckEndingConditions();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (EnemyFOV.CheckPlayerInVision(other.gameObject))
            {
                lastPlayerPosition = EnemyFOV.GetLastPlayerPosition(other.gameObject);
                CheckEndingConditions();
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnVisionRange = false;
            OnAttackRange = false;
            CheckEndingConditions();
            if (HP < 100)
            {
                Pathfinding.RadiusPatrol(lastPlayerPosition);
            }
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
        if (HP <= 0)
        {
            Die();
        }
        if (HP <= 25)
        {
            escape = true;
        }
        CheckEndingConditions();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
