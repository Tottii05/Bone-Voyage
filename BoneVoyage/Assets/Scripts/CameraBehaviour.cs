using UnityEngine;
using System.Collections.Generic;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0f, 12.3f, -9.63f);
    private List<Material> originalPlayerMaterials = new List<Material>();
    private Dictionary<GameObject, List<Material>> originalEnemyMaterials = new Dictionary<GameObject, List<Material>>();
    private Material glowMaterial;
    private Renderer[] playerRenderers;
    private Dictionary<GameObject, Renderer[]> enemyRenderers = new Dictionary<GameObject, Renderer[]>();
    private float fadeSpeed = 2f;
    private List<ParticleSystem> fireballs = new List<ParticleSystem>();
    public Dictionary<ParticleSystem, Material> originalFireballMaterials = new Dictionary<ParticleSystem, Material>();
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        playerRenderers = player.GetComponentsInChildren<Renderer>();
        if (playerRenderers.Length > 0)
        {
            foreach (Renderer renderer in playerRenderers)
            {
                originalPlayerMaterials.Add(renderer.material);
            }
            Shader outlineShader = Shader.Find("Custom/GlowThroughWalls");
            if (outlineShader != null)
            {
                glowMaterial = new Material(outlineShader);
                glowMaterial.SetColor("_GlowColor", new Color(0.5f, 0.8f, 1f, 0.8f));
            }
        }
        FindEnemies();
    }

    public void RegisterFireball(ParticleSystem fireball)
    {
        if (!fireballs.Contains(fireball))
        {
            fireballs.Add(fireball);
            ParticleSystemRenderer fireballRenderer = fireball.GetComponent<ParticleSystemRenderer>();
            if (fireballRenderer != null && !originalFireballMaterials.ContainsKey(fireball))
            {
                originalFireballMaterials[fireball] = fireballRenderer.material;
            }
        }
    }

    public void UnregisterFireball(ParticleSystem fireball)
    {
        if (fireballs.Contains(fireball))
        {
            ParticleSystemRenderer fireballRenderer = fireball.GetComponent<ParticleSystemRenderer>();
            if (fireballRenderer != null && originalFireballMaterials.ContainsKey(fireball))
            {
                fireballRenderer.material = originalFireballMaterials[fireball];
            }
            fireballs.Remove(fireball);
            originalFireballMaterials.Remove(fireball);
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            Renderer[] renderers = enemy.GetComponentsInChildren<Renderer>();
            enemyRenderers[enemy] = renderers;
            List<Material> materials = new List<Material>();
            foreach (Renderer renderer in renderers)
            {
                materials.Add(renderer.material);
            }
            originalEnemyMaterials[enemy] = materials;
        }
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            enemyRenderers.Remove(enemy);
            originalEnemyMaterials.Remove(enemy);
        }
    }

    private void FindEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            RegisterEnemy(enemy);
        }
    }

    public void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        HandlePlayerGlow();
        HandleFireballGlow();
        HandleEnemyGlow();
    }

    private void HandlePlayerGlow()
    {
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            Renderer renderer = playerRenderers[i];
            if (renderer != null)
            {
                bool isPartObscured = false;
                Vector3 rendererCenter = renderer.bounds.center;
                Vector3 direction = rendererCenter - transform.position;
                float distance = direction.magnitude;
                direction = direction.normalized;
                RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject != player && !hit.collider.transform.IsChildOf(player.transform) && !enemies.Contains(hit.collider.gameObject))
                    {
                        isPartObscured = true;
                    }
                }
                if (isPartObscured)
                {
                    if (renderer.material != glowMaterial)
                    {
                        renderer.material = glowMaterial;
                    }
                }
                else
                {
                    if (renderer.material != originalPlayerMaterials[i])
                    {
                        renderer.material = originalPlayerMaterials[i];
                    }
                }
            }
        }
    }

    private void HandleFireballGlow()
    {
        foreach (ParticleSystem fireball in fireballs)
        {
            if (fireball != null)
            {
                bool isFireballObscured = false;
                Vector3 direction = fireball.transform.position - transform.position;
                float distance = direction.magnitude;
                direction = direction.normalized;
                RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject != fireball.gameObject && hit.collider.gameObject != player && !hit.collider.transform.IsChildOf(player.transform) && !enemies.Contains(hit.collider.gameObject))
                    {
                        isFireballObscured = true;
                    }
                }
                ParticleSystemRenderer fireballRenderer = fireball.GetComponent<ParticleSystemRenderer>();
                if (fireballRenderer != null)
                {
                    if (isFireballObscured)
                    {
                        if (fireballRenderer.material != glowMaterial)
                        {
                            fireballRenderer.material = glowMaterial;
                        }
                    }
                    else
                    {
                        if (fireballRenderer.material != originalFireballMaterials[fireball])
                        {
                            fireballRenderer.material = originalFireballMaterials[fireball];
                        }
                    }
                }
            }
        }
    }

    private void HandleEnemyGlow()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemyRenderers.ContainsKey(enemy) && originalEnemyMaterials.ContainsKey(enemy))
            {
                Renderer[] renderers = enemyRenderers[enemy];
                List<Material> originalMaterials = originalEnemyMaterials[enemy];
                for (int i = 0; i < renderers.Length; i++)
                {
                    Renderer renderer = renderers[i];
                    if (renderer != null && i < originalMaterials.Count)
                    {
                        bool isPartObscured = false;
                        Vector3 rendererCenter = renderer.bounds.center;
                        Vector3 direction = rendererCenter - transform.position;
                        float distance = direction.magnitude;
                        direction = direction.normalized;
                        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);
                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.collider.gameObject != enemy && !hit.collider.transform.IsChildOf(enemy.transform) &&
                                hit.collider.gameObject != player && !hit.collider.transform.IsChildOf(player.transform) &&
                                !enemies.Contains(hit.collider.gameObject))
                            {
                                isPartObscured = true;
                            }
                        }
                        if (isPartObscured)
                        {
                            if (renderer.material != glowMaterial)
                            {
                                renderer.material = glowMaterial;
                            }
                        }
                        else
                        {
                            if (renderer.material != originalMaterials[i])
                            {
                                renderer.material = originalMaterials[i];
                            }
                        }
                    }
                }
            }
        }
    }
}