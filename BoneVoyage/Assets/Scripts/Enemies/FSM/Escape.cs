using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Escape", menuName = "StatesSO/Escape")]
public class Escape : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {

    }

    public override void OnStateExit(EnemyController ec)
    {
        ec.gameObject.GetComponent<EnemyPathFinding>().StopEscape();
    }

    public override void OnStateUpdate(EnemyController ec)
    {
        ec.gameObject.GetComponent<EnemyPathFinding>().Escape();
    }
}