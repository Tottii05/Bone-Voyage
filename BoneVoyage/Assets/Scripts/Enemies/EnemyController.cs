using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declare the delegate for the event
public delegate void EnemyDeathHandler();

public class EnemyController : MonoBehaviour, IDamageable
{
    // Add the event
    public static event EnemyDeathHandler OnEnemyDeath;

    public int HP;
    public GameObject target;
    public bool OnAttackRange = false, escape = false, chase = false;
    public EnemyPathFinding _chaseB;
    public StateSO currentState;
    public List<StateSO> States;
    public float AttackRange = 2f;
    public Vector3 lastPlayerPosition;
    public EnemyPathFinding Pathfinding;
    public Animator animator;
    public bool canMove = true;
    public GameObject floatingText;
    public float damageRecieved;
    public DamageSource damageSourceL;
    public DamageSource damageSourceR;
    public GameObject spawnPoint;
    public GameObject coinPrefab;
    public GameObject potionPrefab;

    [Header("Sound Effects")]
    [Header("----------------------------------------")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip wakeUpSound;
    public AudioClip dieSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Pathfinding = GetComponent<EnemyPathFinding>();
        _chaseB = GetComponent<EnemyPathFinding>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
            OnAttackRange = true;
            CheckEndingConditions();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            OnAttackRange = true;
            CheckEndingConditions();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnAttackRange = false;
            CheckEndingConditions();
        }
    }

    private void Update()
    {
        currentState.OnStateUpdate(this);
    }

    public void CheckEndingConditions()
    {
        foreach (ConditionSO condition in currentState.EndConditions)
        {
            if (condition.CheckCondition(this) == condition.answer) ExitCurrentNode();
        }
    }

    public void ExitCurrentNode()
    {
        foreach (StateSO stateSO in States)
        {
            if (stateSO.StartCondition == null || stateSO.StartCondition.CheckCondition(this) == stateSO.StartCondition.answer)
            {
                EnterNewState(stateSO);
                break;
            }
        }
    }

    private void EnterNewState(StateSO state)
    {
        currentState.OnStateExit(this);
        currentState = state;
        currentState.OnStateEnter(this);
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(hitSoundPlay());

        HP -= (int)damage;
        ShowFloatingText();
        if (HP > 0)
        {
            animator.SetTrigger("hit");
        }
        if (HP <= 25)
        {
            escape = true;
        }
        CheckEndingConditions();
    }

    public IEnumerator DieCoroutine()
    {
        escape = false;
        StartCoroutine(dieSoundPlay());
        yield return new WaitForSeconds(1.5f);
        // Trigger the death event before destroying
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void ShowFloatingText()
    {
        var go = Instantiate(floatingText, transform.position + Vector3.up * 3.2f, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = damageRecieved.ToString();
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.fontSize = 30;
    }

    public IEnumerator dieSoundPlay()
    {
        audioSource.PlayOneShot(dieSound);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator hitSoundPlay()
    {
        audioSource.PlayOneShot(hitSound);
        yield return new WaitForSeconds(0.5f);
    }
}