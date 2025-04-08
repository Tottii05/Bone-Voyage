using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "StatesSO/Attack")]
public class Attack : StateSO
{
    private Coroutine attackRoutine;

    public override void OnStateEnter(EnemyController ec)
    {
        ec.canMove = false;
        attackRoutine = ec.StartCoroutine(AttackLoop(ec));
    }

    public override void OnStateExit(EnemyController ec)
    {
        ec.canMove = true;
        if (attackRoutine != null)
        {
            ec.StopCoroutine(attackRoutine);
        }
    }

    public override void OnStateUpdate(EnemyController ec)
    {
    }

    private IEnumerator AttackLoop(EnemyController ec)
    {
        while (ec.OnAttackRange)
        {
            yield return RotateTowardsTarget(ec);
            ec.animator.SetTrigger("attack");
            ec.damageSourceL.BoxCollider.enabled = true;
            ec.damageSourceR.BoxCollider.enabled = true;
            yield return new WaitForSeconds(1.6f);
            ec.damageSourceL.BoxCollider.enabled = false;
            ec.damageSourceR.BoxCollider.enabled = false;
        }
    }

    private IEnumerator RotateTowardsTarget(EnemyController ec)
    {
        if (ec.target == null) yield break;

        float rotationSpeed = 5f;
        Vector3 direction = (ec.target.transform.position - ec.transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(ec.transform.rotation, targetRotation) > 5f)
        {
            ec.transform.rotation = Quaternion.Slerp(ec.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
    }
}
