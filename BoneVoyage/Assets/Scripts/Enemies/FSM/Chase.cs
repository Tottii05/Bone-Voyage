using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Chase", menuName = "StatesSO/Chase")]
public class Chase : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
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