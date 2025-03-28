using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyPathFinding : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public GameObject destination;
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Chase()
    {
        animator.SetBool("run", true);
        StopAllCoroutines();
        agent.SetDestination(target.transform.position);
    }

    public void StopChase()
    {
        animator.SetBool("run", false);
        agent.SetDestination(transform.position);
        return;
    }

    public void StopEscape()
    {
        animator.SetBool("run", false);
        return;
    }

    public void Escape()
    {
        animator.SetBool("run", true);
        Vector3 directionAway = (transform.position - target.transform.position).normalized;
        float fleeDistance = 10f;
        Vector3 newTargetPosition = transform.position + (directionAway * fleeDistance);
        agent.SetDestination(newTargetPosition);
    }
}
