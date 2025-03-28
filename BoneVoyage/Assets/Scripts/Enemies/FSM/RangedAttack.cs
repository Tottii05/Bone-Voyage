using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "StatesSO/RangedAttack")]
public class RangedAttack : StateSO
{
    private Coroutine attackRoutine;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public Stack<GameObject> bulletStack = new Stack<GameObject>();
    public int bulletPoolSize = 3;
    public float bulletLifeTime = 2f;

    public override void OnStateEnter(EnemyController ec)
    {
        if (bulletStack.Count == 0)
        {
            CreateBulletPool();
        }
        GameObject skeletonRogue = GameObject.Find("Skeleton_Rogue");
        if (skeletonRogue != null)
        {
            Transform foundSpawnPoint = skeletonRogue.transform.Find("SpawnPoint");
            if (foundSpawnPoint != null)
            {
                spawnPoint = foundSpawnPoint;
            }
            else
            {
                Debug.LogError("SpawnPoint not found in Skeleton_Rogue!");
            }
        }
        else
        {
            Debug.LogError("Skeleton_Rogue not found in the scene!");
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
    }

    public override void OnStateUpdate(EnemyController ec)
    {
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
                    Vector3 direction = ec.target.transform.position - spawnPoint.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    GameObject bullet = bulletStack.Pop();
                    bullet.SetActive(true);
                    bullet.transform.position = spawnPoint.position;
                    bullet.transform.rotation = rotation;
                    bullet.GetComponent<BulletBehaviour>().Initialize(this);
                }
            }
            yield return new WaitForSeconds(0f);
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
        bullet.SetActive(false);
        bulletStack.Push(bullet);
    }
}
