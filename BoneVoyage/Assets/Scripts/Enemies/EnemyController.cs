using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool canMove = true;
    public GameObject floatingText;
    public float damageRecieved;
    public DamageSource damageSourceL;
    public DamageSource damageSourceR;
    public GameObject spawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.name == "Mage")
        {
            float dmg = player.GetComponent<Mage>().bulletPrefab.GetComponent<BulletBehaviour>().damage;
            damageRecieved = dmg;
        }
        Pathfinding = GetComponent<EnemyPathFinding>();
        _chaseB = GetComponent<EnemyPathFinding>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
            OnAttackRange = true;
            CheckEndingConditions();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
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
        ShowFloatingText();
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
        escape = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void ShowFloatingText()
    {
        var go = Instantiate(floatingText, transform.position + Vector3.up * 3.2f, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = damageRecieved.ToString();
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.fontSize = 30; 
    }
}
