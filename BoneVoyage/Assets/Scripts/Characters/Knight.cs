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

    public bool invulnerable = false;
    public bool reduceDamage = false;

    public AudioClip shieldHit;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
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
            invulnerable = true;
            characterBehaviour.usingSpecial = true;
            animator.SetBool("special", characterBehaviour.usingSpecial);
            animator.SetTrigger("useSpecial");
            StartCoroutine(playSpecialSound());
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
        StartCoroutine(shieldUpSound());
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
        yield return new WaitForSeconds(3.5f);
        characterBehaviour.usingSpecial = false;
        animator.SetBool("special", characterBehaviour.usingSpecial);
        currentWeapon.GetComponent<BoxCollider>().enabled = false;
        invulnerable = false;
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
            if (supportTimer != 0)
            {
                supportText.text = Mathf.Ceil(supportTimer).ToString();
            }
            else
            {
                supportText.text = "";
            }
        }
    }

    private void UpdateSpecialUI()
    {
        if (specialTimer >= 0)
        {
            if (specialTimer != 0)
            {
                specialText.text = Mathf.Ceil(specialTimer).ToString();
            }
            else
            {
                specialText.text = "";
            }
        }
    }
    public override void TakeDamage(float damage)
    {
        if (invulnerable)
        {
            StartCoroutine(shieldHitSound());
            return;
        }
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

    public IEnumerator swingSound()
    {
        audioSource.PlayOneShot(attackSound);
        yield return new WaitForSeconds(0.1f);
    }
    public IEnumerator shieldUpSound()
    {
        audioSource.PlayOneShot(supportSound);
        yield return new WaitForSeconds(0.1f);
    }
    public IEnumerator shieldHitSound()
    {
        audioSource.PlayOneShot(shieldHit);
        yield return new WaitForSeconds(0.1f);
    }   
    public IEnumerator playSpecialSound()
    {
        audioSource.loop = true;
        audioSource.clip = specialSound;
        audioSource.pitch = 3f;
        audioSource.Play();
        yield return new WaitForSeconds(3.5f);
        audioSource.pitch = 1f;
        audioSource.loop = false;
        audioSource.Stop();
    }
}
