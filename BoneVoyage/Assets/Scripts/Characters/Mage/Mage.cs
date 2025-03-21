using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : ACharacter
{
    [Header("Prefabs references")]
    [Header("----------------------------------------")]
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public GameObject UltVFXPrefab;
    public GameObject shieldVFXPrefab;
    [Header("Pool variables")]
    [Header("----------------------------------------")]
    public Stack<GameObject> bulletStack = new Stack<GameObject>();
    public int bulletPoolSize = 3;
    public float bulletLifeTime = 2f;
    [Header("Combat variables")]
    [Header("----------------------------------------")]
    public bool shielded = false;
    public List<GameObject> weapons = new List<GameObject>();
    public GameObject activeWeapon;

    public void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;
        CreateBulletPool();
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        activeWeapon = weapons[0];
        activeWeapon.SetActive(true);
    }

    public override void Attack()
    {
        StartCoroutine(PerformAttack());
    }

    public override void Support()
    {
        StartCoroutine(PerfomSupport());
    }

    public override void Special()
    {
        StartCoroutine(PerformSpecial());
    }

    public override void TakeDamage(float damage)
    {
        if (!shielded)
        {
            base.TakeDamage(damage);
        }
    }

    public IEnumerator PerformAttack()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.4f);
        Shoot();
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator PerfomSupport()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Support");
        yield return new WaitForSeconds(0.4f);
        shieldVFXPrefab.SetActive(true);
        StartCoroutine(WaitSupport());
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator WaitSupport()
    {
        yield return new WaitForSeconds(4f);
        shieldVFXPrefab.SetActive(false);
    }

    public IEnumerator PerformSpecial()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Ult");
        yield return new WaitForSeconds(0.7f);

        Vector3 mouseWorldPos = GetMouseWorldPosition();
        UltVFXPrefab.SetActive(true);
        UltVFXPrefab.transform.SetParent(null);
        UltVFXPrefab.transform.position = mouseWorldPos;

        StartCoroutine(WaitUlt());
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator WaitUlt()
    {
        yield return new WaitForSeconds(4.5f);
        UltVFXPrefab.SetActive(false);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return transform.position;
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

    public void Shoot()
    {
        if (bulletStack.Count > 0)
        {
            GameObject bullet = bulletStack.Pop();
            bullet.SetActive(true);
            bullet.transform.position = spawnPoint.position;
            bullet.transform.rotation = spawnPoint.rotation;
            bullet.GetComponent<BulletBehaviour>().Initialize(this);
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletStack.Push(bullet);
    }
}