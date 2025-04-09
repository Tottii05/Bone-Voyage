using UnityEngine;

public class FireLightEffect : MonoBehaviour
{
    Light fireLight;

    public float minIntensity = 1.4f;
    public float maxIntensity = 2.2f;
    public float smoothSpeed = 2.0f;
    public float changeInterval = 0.5f;

    private float targetIntensity;
    private float timer;

    void Start()
    {
        fireLight = GetComponent<Light>();
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timer = 0f;
        }

        fireLight.intensity = Mathf.Lerp(fireLight.intensity, targetIntensity, Time.deltaTime * smoothSpeed);
    }
}
