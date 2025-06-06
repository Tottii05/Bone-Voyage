    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Barbarian : ACharacter
{
    public BoxCollider attackCollider;
    //objetos
    public GameObject mug;
    public List<GameObject> weaponsLeft ;
    public List<GameObject> weaponsRight;
    public GameObject currentWeaponLeft;
    public GameObject currentWeaponRight;
    public Transform slashPointLeft;
    public Transform slashPointRight;
    //variables
    public int damageAugment = 5;
    public int speedAugment = 2;
    public int heal=20;
    public float mugBuffDuration = 5f;
    public float damageBuffDuration = 5f;
    public bool specialActive=false;
    //VFX
    public GameObject slashVFX;      
    public GameObject healVFX;
    public GameObject buffVFX;
    public GameObject lfire;
    public GameObject rfire;

    public void Start()
    {
        foreach (GameObject weapon in weaponsLeft)
        {
            weapon.SetActive(false);
        }

        foreach (GameObject weapon in weaponsRight)
        {
            weapon.SetActive(false);
        }

        currentWeaponLeft = weaponsLeft[PlayerPrefs.GetInt("KnightCurrentWeapon")];
        currentWeaponRight = weaponsRight[PlayerPrefs.GetInt("KnightCurrentWeapon")];
        currentWeaponLeft.SetActive(true);
        currentWeaponRight.SetActive(true);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        supportText = GameObject.Find("supportText").GetComponent<TextMeshProUGUI>();
        specialText = GameObject.Find("specialText").GetComponent<TextMeshProUGUI>();
        supportImage = GameObject.Find("SupportCDColor");
        specialImage = GameObject.Find("UltCDColor");
        characterBehaviour = GetComponent<CharacterBehaviour>();
        animator = GetComponent<Animator>();
        checkPoint = transform.position;
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
        if (supportReady)
        {
            StartCoroutine(PerformSupport());
        }
    }
    public override void Special()
    {
        if (specialReady)
        {
            StartCoroutine(PerformSpecial());
        }
    }
    public IEnumerator PerformAttack()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Attack");
        StartCoroutine(Combo());
        if(specialActive)
        {
            StartCoroutine(playAttackSoundAlt());
            StartCoroutine(SpecialEffect());
        }
        else
        {
            StartCoroutine(playAttackSound());
        }

        yield return new WaitForSeconds(0.7f);

        characterBehaviour.isWaiting = true;
    }

    public IEnumerator PerformSupport()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Support");
        supportReady = false;
        supportTimer = supportCooldown;
        UpdateSupportUI();
        mug.SetActive(true);
        currentWeaponLeft.SetActive(false);
        currentWeaponRight.SetActive(false);
        healVFX.SetActive(true);
        buffVFX.SetActive(true);
        StartCoroutine(playMugSound()); //play mug sound
        yield return new WaitForSeconds(2f);
        healVFX.SetActive(false);
        buffVFX.SetActive(false);
        StartCoroutine(MugBuff());
        mug.SetActive(false);
        currentWeaponLeft.SetActive(true);
        currentWeaponRight.SetActive(true);
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
    public IEnumerator PerformSpecial()
    {
        characterBehaviour.isWaiting = false;
        animator.SetTrigger("Special");
        specialReady = false;
        specialTimer = specialCooldown;
        UpdateSpecialUI();
        StartCoroutine(DamageBuff());
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(SpecialCooldown());
        characterBehaviour.isWaiting = true;
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
    public IEnumerator Combo()
    {
        int num=animator.GetInteger("AttackNum");
        switch (num)
        {
            case 0:
                animator.SetInteger("AttackNum", 1);
                
                break;
            case 1:
                animator.SetInteger("AttackNum", 2);
                
                
                break;
            case 2:
                animator.SetInteger("AttackNum", 3);
               
                break;
            case 3:
                animator.SetInteger("AttackNum", 1);
                break;
        }
        yield return new WaitForSeconds(0.35f);
        currentWeaponLeft.GetComponent<BoxCollider>().enabled = true;
        currentWeaponRight.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        currentWeaponLeft.GetComponent<BoxCollider>().enabled = false;
        currentWeaponRight.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(1.5f);
        if(num+1== animator.GetInteger("AttackNum"))
        {
            animator.SetInteger("AttackNum", 0);
        }
        
    }
    public IEnumerator MugBuff()
    {
        health += heal;
        speed += speedAugment;
        healthBar.value = health / 100;
        yield return new WaitForSeconds(mugBuffDuration);
        speed -= speedAugment;
    }
    public IEnumerator DamageBuff()
    {
        rfire.SetActive(true);
        lfire.SetActive(true);
        specialActive = true;
        currentWeaponLeft.GetComponent<DamageSource>().damage += damageAugment;
        currentWeaponRight.GetComponent<DamageSource>().damage += damageAugment;
        Vector3 origL = currentWeaponLeft.GetComponent<BoxCollider>().size;
        Vector3 origR = currentWeaponRight.GetComponent<BoxCollider>().size;
        currentWeaponLeft.GetComponent<BoxCollider>().size = origL * 3f;
        currentWeaponRight.GetComponent<BoxCollider>().size = origR * 3f;
        yield return new WaitForSeconds(damageBuffDuration);
        currentWeaponLeft.GetComponent<DamageSource>().damage -= damageAugment;
        currentWeaponRight.GetComponent<DamageSource>().damage -= damageAugment;
        currentWeaponLeft.GetComponent<BoxCollider>().size = origL;
        currentWeaponRight.GetComponent<BoxCollider>().size = origR;
        lfire.SetActive(false);
        rfire.SetActive(false);
        specialActive = false;
    }
    public IEnumerator SpecialEffect()
    {
        int num = animator.GetInteger("AttackNum");
        
        //GameObject slashLeft = Instantiate(slashVFX, slashPointLeft.position, slashPointLeft.rotation, slashPointLeft);
        //GameObject slashRight = Instantiate(slashVFX, slashPointRight.position, slashPointRight.rotation, slashPointRight);
        switch (num)
        {
            case 1:
                yield return new WaitForSeconds(0.4f);
                Destroy(Instantiate(slashVFX, slashPointRight.position, slashPointRight.rotation, slashPointRight),0.2f);
                break;
            case 2:
                yield return new WaitForSeconds(0.5f);
                Destroy(Instantiate(slashVFX, slashPointRight.position, slashPointRight.rotation, slashPointRight), 0.2f);
                yield return new WaitForSeconds(0.4f);
                Destroy(Instantiate(slashVFX, slashPointLeft.position, slashPointLeft.rotation, slashPointLeft),0.2f);
                break;
            case 3:
                yield return new WaitForSeconds(0.6f);
                GameObject slashRight = Instantiate(slashVFX, slashPointRight.position, slashPointRight.rotation, slashPointRight);
                Destroy(slashRight, 0.2f);
                GameObject slashLeft = Instantiate(slashVFX, slashPointLeft.position, slashPointLeft.rotation, slashPointLeft);
                Destroy(slashLeft, 0.2f);
                slashLeft.transform.localScale= slashLeft.transform.localScale* 1.5f;   
                slashRight.transform.localScale = slashRight.transform.localScale * 1.5f;
                break;
        }
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator playMugSound()
    {
        audioSource.clip = supportSound;
        audioSource.loop = true;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.clip = null;
    }

    public IEnumerator playAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator playAttackSoundAlt()
    {
        audioSource.PlayOneShot(specialSound);
        yield return new WaitForSeconds(0.5f);
    }
}
