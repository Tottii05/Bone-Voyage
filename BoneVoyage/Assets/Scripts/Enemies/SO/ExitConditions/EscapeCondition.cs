using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "EscapeCondition", menuName = "ConditionSO/Escape")]
public class EscapeCondition : ConditionSO
{
    public override bool CheckCondition(EnemyController ec)
    {
        if (ec.HP <= 25)
        {
            return ec.escape == true;
        }
        return false;
    }
}