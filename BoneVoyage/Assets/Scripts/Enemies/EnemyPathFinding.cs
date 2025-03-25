using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyPathFinding : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject Apoint, Bpoint;
    public GameObject target;
    public GameObject destination;
    public bool patrullanding = true;
    public float lastTargetRadius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        destination = Bpoint;   
        Patrol();
    }

    public void Patrol()
    {
        while (patrullanding)
        {
            agent.SetDestination(destination.transform.position);
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                return;
            }
            destination = destination == Apoint ? Bpoint : Apoint;
        }
    }
    public void Chase()
    {
        StopAllCoroutines();
        agent.SetDestination(target.transform.position);
    }

    public void StopChase()
    {
        return;
    }

    public void StopEscape()
    {
        return;
    }

    public void Escape()
    {
        Vector3 directionAway = (transform.position - target.transform.position).normalized;
        float fleeDistance = 10f;
        Vector3 newTargetPosition = transform.position + (directionAway * fleeDistance);
        agent.SetDestination(newTargetPosition);
    }

    public void RadiusPatrol(Vector3 center)
    {
        float firstX = Random.Range(center.x - lastTargetRadius, center.x + lastTargetRadius);
        float firstZ = Random.Range(center.z - lastTargetRadius, center.z + lastTargetRadius);
        float secondX = Random.Range(center.x - lastTargetRadius, center.x + lastTargetRadius);
        float secondZ = Random.Range(center.z - lastTargetRadius, center.z + lastTargetRadius);

        Apoint.transform.position = new Vector3(firstX, center.y, firstZ);
        Bpoint.transform.position = new Vector3(secondX, center.y, secondZ);
        Patrol();
    }
}
