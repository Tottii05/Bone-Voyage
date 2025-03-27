using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "StatesSO/Attack")]
public class Attack : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
        ec.animator.SetTrigger("attack");
    }

    public override void OnStateExit(EnemyController ec)
    {
    }

    public override void OnStateUpdate(EnemyController ec)
    {

    }
}