using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Patrol", menuName = "StatesSO/Patrol")]
public class Patrol : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
        ec.gameObject.GetComponent<EnemyPathFinding>().destination = GameObject.Find("BPoint");
    }

    public override void OnStateExit(EnemyController ec)
    {

    }

    public override void OnStateUpdate(EnemyController ec)
    {
        ec.gameObject.GetComponent<EnemyPathFinding>().Patrol();
    }
}