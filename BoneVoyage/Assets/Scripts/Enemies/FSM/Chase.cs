using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(fileName = "Chase", menuName = "StatesSO/Chase")]
public class Chase : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
        ec.GetComponent<CapsuleCollider>().enabled = true;
        ec.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public override void OnStateExit(EnemyController ec)
    {
        ec.gameObject.GetComponent<EnemyPathFinding>().StopChase();
    }

    public override void OnStateUpdate(EnemyController ec)
    {
        ec.gameObject.GetComponent<EnemyPathFinding>().Chase();
    }
}