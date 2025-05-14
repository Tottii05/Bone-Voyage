using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBoss : MonoBehaviour, IDamageable
{
    public Animator animator;
    public int HP = 400; //HP
    public float damageRecieved;
    public GameObject floatingText;
    public int attackCD = 3;

    public int currentLevelIndex;

    public Transform spawnPoint;

    public Stack<GameObject> attackStack = new Stack<GameObject>();
    public int attackPoolSize = 3;
    public float attackLifeTime = 2f;
    public GameObject attackPrefab;

    public bool playerInRange = false;
    public void CreateAttackPool()
    {
        for (int i = 0; i < attackPoolSize; i++)
        {
            GameObject bullet = Instantiate(attackPrefab);
            bullet.SetActive(false);
            attackStack.Push(bullet);
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        attackStack.Push(bullet);
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
        CreateAttackPool();
    }

    public void TakeDamage(float damage)
    {
        HP -= (int)damage;
        ShowFloatingText();
        if (HP <= 0) {

            GameManagerScript.instance.MarkLevelCompleted(currentLevelIndex);
            if (gameObject.name == "FinalWall")
            {
                LevelPickerBehaviour levelPicker = FindObjectOfType<LevelPickerBehaviour>();
                levelPicker.canBeShown = true;
            }

            Die();
        }
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    public void ShowFloatingText()
    {
        var go = Instantiate(floatingText, transform.position + Vector3.up * 3.2f, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = damageRecieved.ToString();
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.fontSize = 30;
    }

    public IEnumerator DieCoroutine()
    {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private Coroutine attackCoroutine;
    private Coroutine lookCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("Idle", false);
            if (attackCoroutine == null)
            {
                Debug.Log("Starting attacking");
                attackCoroutine = StartCoroutine(attacking());
            }
            if (lookCoroutine == null)
                lookCoroutine = StartCoroutine(lookToPlayer(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("Idle", true);
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

            if (lookCoroutine != null)
            {
                StopCoroutine(lookCoroutine);
                lookCoroutine = null;
            }
        }
    }


    public IEnumerator lookToPlayer(Collider player)
    {
        float rotationSpeed = 5f;

        while (true)
        {
            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator attacking()
    {
        while (true)
        {
            if (attackStack.Count > 0)
            {
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);

                if (attackPrefab != null)
                {
                    GameObject bullet = attackStack.Pop();
                    bullet.SetActive(true);
                    bullet.transform.position = spawnPoint.position;
                    bullet.transform.rotation = spawnPoint.rotation;
                    bullet.GetComponent<VillageBossBullet>().Initialize(this);
                }

                yield return new WaitForSeconds(attackCD);
            }
            else
            {
                yield return null; 
            }
        }
    }

}
