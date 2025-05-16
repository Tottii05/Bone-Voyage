using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rogue : ACharacter
{
    [Header("Prefabs and projectiles spawnpoint")]
    [Header("----------------------------------------")]
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject specialAura;
    public Transform spawnPoint;
    public List<GameObject> weapons = new List<GameObject>();
    public GameObject activeWeapon;
    [Header("Combat variables and CDs")]
    [Header("----------------------------------------")]
    private bool usingSpecial = false;
    public float specialDuration;
    public float arrowCD;
    public float grenadeCD;
    [Header("Pool variables")]
    [Header("----------------------------------------")]
    public Stack<GameObject> arrowStack = new Stack<GameObject>();
    public int arrowPoolSize = 3;
    public float arrowLifeTime = 2f;
    public Stack<GameObject> grenadeStack = new Stack<GameObject>();
    public int grenadePoolSize = 5;
    public float grenadeLifeTime = 3f;


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        supportText = GameObject.Find("supportText").GetComponent<TextMeshProUGUI>();
        specialText = GameObject.Find("specialText").GetComponent<TextMeshProUGUI>();
        supportImage = GameObject.Find("SupportCDColor");
        specialImage = GameObject.Find("UltCDColor");
        specialAura.SetActive(false);
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;
        CreateArrowPool();
        CreateGrenadePool();

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        activeWeapon = weapons[PlayerPrefs.GetInt("RogueCurrentWeapon")];
        activeWeapon.SetActive(true);
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
        if (supportReady) StartCoroutine(PerformRoll());
    }
    public override void Special()
    {
        if (specialReady)
        {
            StartCoroutine(useEspecial());
        }
    }

    public void CreateArrowPool()
    {
        for (int i = 0; i < arrowPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            arrowStack.Push(bullet);
        }
    }

    public void CreateGrenadePool()
    {
        for (int i = 0; i < grenadePoolSize; i++)
        {
            GameObject grenade = Instantiate(grenadePrefab);
            grenade.SetActive(false);
            grenadeStack.Push(grenade);
        }
    }

    public void ReturnArrowToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        arrowStack.Push(bullet);
    }

    public void ReturnGrenadeToPool(GameObject grenade)
    {
        grenade.SetActive(false);
        grenadeStack.Push(grenade);
    }
    public IEnumerator PerformAttack()
    {         
        if (!usingSpecial)
        {
            if (arrowStack.Count > 0)
            {
                characterBehaviour.isWaiting = false;
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                if (bulletPrefab != null)
                {
                    GameObject bullet = arrowStack.Pop();
                    bullet.SetActive(true);
                    bullet.transform.position = spawnPoint.position;
                    bullet.transform.rotation = spawnPoint.rotation;
                    bullet.GetComponent<ArrowBehaviour>().Initialize(this);
                }
                yield return new WaitForSeconds(arrowCD);
            }            
        } else
        {
            characterBehaviour.isWaiting = false;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);
            if (grenadePrefab != null)
            {
                GameObject grenade = grenadeStack.Pop();
                grenade.SetActive(true);
                grenade.transform.position = spawnPoint.position;
                grenade.transform.rotation = spawnPoint.rotation;
                grenade.GetComponent<GrenadeBehaviour>().Initialize(this);
            }
            yield return new WaitForSeconds(grenadeCD);
        }      
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator PerformRoll()
    {
        characterBehaviour.isWaiting = false;
        supportReady = false;
        supportTimer = supportCooldown;
        UpdateSupportUI();
        animator.SetTrigger("Dash");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.detectCollisions = false;
        rb.velocity = Vector3.zero;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + transform.forward * 4f; // Ajusta el valor de +Xf para la distancia del roll.

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rb.MovePosition(Vector3.Lerp(startPos, targetPos, elapsedTime / duration));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        rb.MovePosition(targetPos);
        rb.detectCollisions = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        characterBehaviour.isWaiting = true;
        StartCoroutine(SupportCooldown());
        characterBehaviour.isWaiting = true;

    }

    public IEnumerator SupportCooldown()
    {
        yield return new WaitForSeconds(supportCooldown);
        supportReady = true;
        supportTimer = 0f;
        UpdateSupportUI();
    }

    public IEnumerator useEspecial()
    {
        specialReady = false;
        specialTimer = specialCooldown;
        UpdateSpecialUI();
        specialAura.SetActive(true);
        usingSpecial = true;
        yield return new WaitForSeconds(specialDuration);
        specialAura.SetActive(false);
        usingSpecial = false;
        StartCoroutine(SpecialCooldown());
    }
    public IEnumerator SpecialCooldown()
    {
        yield return new WaitForSeconds(specialCooldown);
        specialReady = true;
        specialTimer = 0f;
        UpdateSpecialUI();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(Hit());
        healthBar.value = health / 100;
    }
    public IEnumerator Hit()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("hit");

        yield return new WaitForSeconds(0.3f);

        characterBehaviour.isWaiting = true;
    }
    private void UpdateSupportUI()
    {
        if (supportTimer >= 0)
        {
            if (supportTimer != 0)
            {
                supportImage.GetComponent<Image>().enabled = true;
                supportText.text = Mathf.Ceil(supportTimer).ToString();
            }
            else
            {
                supportImage.GetComponent<Image>().enabled = false;
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
                specialImage.GetComponent<Image>().enabled = true;
                specialText.text = Mathf.Ceil(specialTimer).ToString();
            }
            else
            {
                specialImage.GetComponent<Image>().enabled = false;
                specialText.text = "";
            }
        }
    }

    public IEnumerator playDashSound()
    {
        audioSource.PlayOneShot(supportSound);
        yield return new WaitForSeconds(0.5f);
    }
}
