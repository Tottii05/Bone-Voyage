using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Die", menuName = "StatesSO/Die")]
public class Die : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
        ec.animator.SetTrigger("die");
        ec.Die();
    }

    public override void OnStateExit(EnemyController ec)
    {

    }

    public override void OnStateUpdate(EnemyController ec)
    {

    }
}