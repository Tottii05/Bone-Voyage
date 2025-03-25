using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "StatesSO/Attack")]
public class Attack : StateSO
{
    public override void OnStateEnter(EnemyController ec)
    {
        Debug.Log("Dame el movil");
    }

    public override void OnStateExit(EnemyController ec)
    {
    }

    public override void OnStateUpdate(EnemyController ec)
    {

    }
}