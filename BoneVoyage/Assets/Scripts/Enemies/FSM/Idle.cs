using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Idle", menuName = "StatesSO/Idle")]
public class Idle : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
        ec.GetComponent<CapsuleCollider>().enabled = false;
    }

    public override void OnStateExit(EnemyController ec)
    {

    }

    public override void OnStateUpdate(EnemyController ec)
    {

    }
}