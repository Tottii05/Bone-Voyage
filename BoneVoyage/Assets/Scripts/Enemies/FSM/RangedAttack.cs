using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "StatesSO/RangedAttack")]
public class RangedAttack : StateSO
{
    private Coroutine attackRoutine;
    public GameObject bulletPrefab;
    public Stack<GameObject> bulletStack = new Stack<GameObject>();
    public int bulletPoolSize = 3;
    public float bulletLifeTime = 2f;
    private CameraBehaviour cameraBehaviour;

    public override void OnStateEnter(EnemyController ec)
    {
        if (cameraBehaviour == null)
        {
            cameraBehaviour = FindObjectOfType<CameraBehaviour>();
        }

        if (bulletStack.Count == 0)
        {
            CreateBulletPool();
        }
        ec.animator.SetTrigger("aim");
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
        ec.animator.SetBool("inRange", true);
    }

    public override void OnStateUpdate(EnemyController ec)
    {
        ec.animator.SetBool("inRange", true);
        if (ec.target == null || ec.animator.GetCurrentAnimatorStateInfo(0).IsName("Skeletons_Inactive_Floor_Pose")) return;
        ec.transform.rotation = Quaternion.LookRotation(ec.target.transform.position - ec.transform.position);
    }
    
    private IEnumerator AttackLoop(EnemyController ec)
    {
        while (ec.OnAttackRange)
        {
            if (ec.animator.GetCurrentAnimatorStateInfo(0).IsName("2H_Ranged_Aiming"))
            {
                Debug.Log("Shooting");
                yield return new WaitForSeconds(0.2f);
                ec.animator.SetTrigger("shoot");
                if (bulletStack.Count > 0)
                {
                    Vector3 direction = ec.target.transform.position - ec.spawnPoint.transform.position;

                    Quaternion rotation = Quaternion.LookRotation(direction);
                    GameObject bullet = bulletStack.Pop();
                    bullet.SetActive(true);
                    bullet.transform.position = ec.spawnPoint.transform.position;
                    bullet.transform.rotation = rotation;
                    bullet.GetComponent<BulletBehaviour>().Initialize(this);
                    if (cameraBehaviour != null)
                    {
                        cameraBehaviour.RegisterArrow(bullet);
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void CreateBulletPool()
    {
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletStack.Push(bullet);
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        if (cameraBehaviour != null)
        {
            cameraBehaviour.UnregisterArrow(bullet);
        }

        bullet.SetActive(false);
        bulletStack.Push(bullet);
    }
}