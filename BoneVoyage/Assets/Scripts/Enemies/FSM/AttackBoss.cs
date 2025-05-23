using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBoss", menuName = "StatesSO/AttackBoss")]
public class AttackBoss : StateSO
{
    private Coroutine attackRoutine;
    public float attackColliderActiveTime = 0.5f;
    private AudioSource attackAudioSource;
    public override void OnStateEnter(EnemyController ec)
    {
        ec.canMove = false;
        attackRoutine = ec.StartCoroutine(AttackLoop(ec));
        attackAudioSource = ec.GetComponent<AudioSource>();
        
    }

    public override void OnStateExit(EnemyController ec)
    {
        ec.canMove = true;
        //ec.damageSourceL.BoxCollider.enabled = false;
        ec.damageSourceR.BoxCollider.enabled = false;
        if (attackRoutine != null)
        {
            ec.StopCoroutine(attackRoutine);
        }
    }

    public override void OnStateUpdate(EnemyController ec)
    {
        ec.transform.rotation = Quaternion.LookRotation(ec.target.transform.position - ec.transform.position);
        
    }

    private IEnumerator AttackLoop(EnemyController ec)
    {
        while (ec.OnAttackRange)
        {
            yield return RotateTowardsTarget(ec);
            ec.animator.SetTrigger("attack");
            ec.StartCoroutine(PlayAttackSound(ec));
            yield return new WaitForSeconds(0.5f);
            //ec.damageSourceL.BoxCollider.enabled = true;
            ec.damageSourceR.BoxCollider.enabled = true;
            yield return new WaitForSeconds(attackColliderActiveTime);
            //ec.damageSourceL.BoxCollider.enabled = false;
            ec.damageSourceR.BoxCollider.enabled = false;
            yield return new WaitForSeconds(1.6f - 0.5f - attackColliderActiveTime);
        }
    }

    private IEnumerator RotateTowardsTarget(EnemyController ec)
    {
        if (ec.target == null || !ec.canMove) yield break;

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
    public IEnumerator PlayAttackSound(EnemyController ec)
    {
        attackAudioSource.PlayOneShot(ec.attackSound);
        yield return new WaitForSeconds(0.5f);
    }
}