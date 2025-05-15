using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mage : ACharacter
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public GameObject UltVFXPrefab;
    public GameObject shieldVFXPrefab;
    public Stack<GameObject> bulletStack = new Stack<GameObject>();
    public int bulletPoolSize = 3;
    public float bulletLifeTime = 2f;
    public bool shielded = false;
    public List<GameObject> weapons = new List<GameObject>();
    public GameObject activeWeapon;
    private CameraBehaviour cameraBehaviour;

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
        cameraBehaviour = FindObjectOfType<CameraBehaviour>();
        CreateBulletPool();
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        activeWeapon = weapons[PlayerPrefs.GetInt("MageCurrentWeapon")];
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
        if (supportReady)
        {
            StartCoroutine(PerfomSupport());
        }
    }

    public override void Special()
    {
        if (specialReady)
        {
            StartCoroutine(PerformSpecial());
        }
    }

    public override void TakeDamage(float damage)
    {
        if (!shielded)
        {
            base.TakeDamage(damage);
            healthBar.value = health / 100;
        }
        else
        {
            StartCoroutine(shieldHitSound());
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
        supportReady = false;
        supportTimer = supportCooldown;
        UpdateSupportUI();
        yield return new WaitForSeconds(0.4f);
        shieldVFXPrefab.SetActive(true);
        shielded = true;
        StartCoroutine(WaitSupport());
        StartCoroutine(SupportCooldown());
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator WaitSupport()
    {
        yield return new WaitForSeconds(4f);
        shieldVFXPrefab.SetActive(false);
        shielded = false;
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
        animator.SetTrigger("Ult");
        specialReady = false;
        specialTimer = specialCooldown;
        UpdateSpecialUI();
        yield return new WaitForSeconds(0.7f);
        GameObject ultInstance = Instantiate(UltVFXPrefab, GetMouseWorldPosition(), Quaternion.identity);
        ultInstance.SetActive(true);
        StartCoroutine(WaitUlt(ultInstance));
        StartCoroutine(SpecialCooldown());
        characterBehaviour.isWaiting = true;
    }

    public IEnumerator WaitUlt(GameObject ultInstance)
    {
        yield return new WaitForSeconds(4.5f);
        Destroy(ultInstance);
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
            ParticleSystem fireball = bullet.GetComponent<ParticleSystem>();
            if (fireball != null)
            {
                ParticleSystemRenderer renderer = fireball.GetComponent<ParticleSystemRenderer>();
                if (renderer != null)
                {
                    renderer.material = renderer.material;
                }
            }
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
            ParticleSystem fireball = bullet.GetComponent<ParticleSystem>();
            if (fireball != null)
            {
                ParticleSystemRenderer renderer = fireball.GetComponent<ParticleSystemRenderer>();
                if (renderer != null)
                {
                    renderer.material = cameraBehaviour.originalFireballMaterials.ContainsKey(fireball) ? cameraBehaviour.originalFireballMaterials[fireball] : renderer.material;
                }
                cameraBehaviour.RegisterFireball(fireball);
            }
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        ParticleSystem fireball = bullet.GetComponent<ParticleSystem>();
        if (fireball != null)
        {
            ParticleSystemRenderer renderer = fireball.GetComponent<ParticleSystemRenderer>();
            if (renderer != null && cameraBehaviour.originalFireballMaterials.ContainsKey(fireball))
            {
                renderer.material = cameraBehaviour.originalFireballMaterials[fireball];
            }
            cameraBehaviour.UnregisterFireball(fireball);
        }
        bullet.SetActive(false);
        bulletStack.Push(bullet);
    }

    public IEnumerator shieldHitSound()
    {
        audioSource.PlayOneShot(supportSound);
        yield return new WaitForSeconds(0.1f);
    }
}