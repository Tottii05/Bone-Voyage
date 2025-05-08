using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Kight : ACharacter
{
    public List<GameObject> weapons;
    public GameObject currentWeapon;

    public List<GameObject> shields;
    public GameObject currentShield;

    public bool reduceDamage = false;
    public void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        supportText = GameObject.Find("supportText").GetComponent<TextMeshProUGUI>();
        specialText = GameObject.Find("specialText").GetComponent<TextMeshProUGUI>();
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        foreach (GameObject shield in shields)
        {
            shield.SetActive(false);
        }

        currentWeapon = weapons[PlayerPrefs.GetInt("KnightCurrentWeapon")];
        currentWeapon.SetActive(true);
        currentShield = shields[PlayerPrefs.GetInt("KnightCurrentShield")];
        currentShield.SetActive(true);
        supportTimer = 0f;
        specialTimer = 0f;
        UpdateSupportUI();
        UpdateSpecialUI();
    }
    public void Update()
    {
        if (!supportReady)
        {
            supportTimer -= Time.deltaTime;
            UpdateSupportUI();
        }
        if (!specialReady)
        {
            specialTimer -= Time.deltaTime;
            UpdateSpecialUI();
        }
    }
    public override void Attack()
    {
        StartCoroutine(PerformAttack());
    }
    public override void Support()
    {
        reduceDamage = !reduceDamage;
        if (reduceDamage)
        {
            if (supportReady)
            {
                characterBehaviour.isWaiting = false;
                animator.SetTrigger("startBlocking");
                animator.SetBool("blocking", true);
                supportReady = false;
                supportTimer = supportCooldown;
                UpdateSupportUI();
                StartCoroutine(SupportCooldown());
            }
        }
        else
        {
            animator.SetBool("blocking", false);
            characterBehaviour.isWaiting = true;
        }
    }
    public IEnumerator SupportCooldown()
    {
        yield return new WaitForSeconds(supportCooldown);
        supportReady = true;
        supportTimer = 0f;
        UpdateSupportUI();
    }
    public override void Special()
    {
        if (specialReady)
        {
            characterBehaviour.usingSpecial = true;
            animator.SetBool("special", characterBehaviour.usingSpecial);
            animator.SetTrigger("useSpecial");
            specialReady = false;
            specialTimer = specialCooldown;
            UpdateSpecialUI();
            currentWeapon.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(PerformSpecial());
        }
    }
    public IEnumerator PerformAttack()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.2f);
        currentWeapon.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        currentWeapon.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        characterBehaviour.isWaiting = true;
    }
    public IEnumerator PerformSpecial()
    {
        StartCoroutine(SpecialCooldown());
        yield return new WaitForSeconds(5f);
        characterBehaviour.usingSpecial = false;
        animator.SetBool("special", characterBehaviour.usingSpecial);
        currentWeapon.GetComponent<BoxCollider>().enabled = false;
    }
    public IEnumerator SpecialCooldown()
    {
        yield return new WaitForSeconds(specialCooldown);
        specialReady = true;
        specialTimer = 0f;
        UpdateSpecialUI();
    }
    private void UpdateSupportUI()
    {
        if (supportTimer >= 0)
        {
            supportText.text = Mathf.Ceil(supportTimer).ToString();
        }
    }

    private void UpdateSpecialUI()
    {
        if (specialTimer >= 0)
        {
            specialText.text = Mathf.Ceil(specialTimer).ToString();
        }
    }
    public override void TakeDamage(float damage)
    {
        if (reduceDamage)
        {
            damage *= currentShield.GetComponent<Shield>().damageReduction;
            animator.SetTrigger("blockHit");
        }
        else
        {
            animator.SetTrigger("hit");
        }
        health -= damage;
        healthBar.value = health / 100;
        if (health <= 0)
        {
            Die();
        }
    }
}
