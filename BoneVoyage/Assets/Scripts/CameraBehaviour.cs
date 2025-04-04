using UnityEngine;
using System.Collections.Generic;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0f, 12.3f, -9.63f);
    private List<Material> originalPlayerMaterials = new List<Material>();
    private Material glowMaterial;
    private Renderer[] playerRenderers;
    private float fadeSpeed = 2f;
    private List<ParticleSystem> fireballs = new List<ParticleSystem>();
    public Dictionary<ParticleSystem, Material> originalFireballMaterials = new Dictionary<ParticleSystem, Material>();

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

    public void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        HandlePlayerGlow();
        HandleFireballGlow();
    }

    private void HandlePlayerGlow()
    {
        for (int i = 0; i < playerRenderers.Length; i++)
        {
            Renderer renderer = playerRenderers[i];
            if (renderer == null) continue;
            bool isPartObscured = false;
            Vector3 rendererCenter = renderer.bounds.center;
            Vector3 direction = (rendererCenter - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, rendererCenter);
            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != player && !hit.collider.transform.IsChildOf(player.transform))
                {
                    isPartObscured = true;
                }
            }
            if (isPartObscured && renderer.material != glowMaterial)
            {
                renderer.material = glowMaterial;
            }
            else if (!isPartObscured && renderer.material != originalPlayerMaterials[i])
            {
                renderer.material = originalPlayerMaterials[i];
            }
        }
    }

    private void HandleFireballGlow()
    {
        foreach (ParticleSystem fireball in fireballs)
        {
            if (fireball == null) continue;
            bool isFireballObscured = false;
            RaycastHit[] hits = Physics.RaycastAll(transform.position,
                (fireball.transform.position - transform.position).normalized,
                Vector3.Distance(transform.position, fireball.transform.position));
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != fireball.gameObject && hit.collider.gameObject != player && !hit.collider.transform.IsChildOf(player.transform))
                {
                    isFireballObscured = true;
                }
            }
            ParticleSystemRenderer fireballRenderer = fireball.GetComponent<ParticleSystemRenderer>();
            if (fireballRenderer != null)
            {
                if (isFireballObscured && fireballRenderer.material != glowMaterial)
                {
                    fireballRenderer.material = glowMaterial;
                }
                else if (!isFireballObscured && fireballRenderer.material != originalFireballMaterials[fireball])
                {
                    fireballRenderer.material = originalFireballMaterials[fireball];
                }
            }
        }
    }
}