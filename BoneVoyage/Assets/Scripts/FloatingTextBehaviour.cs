using UnityEngine;

public class FloatingTextBehaviour : MonoBehaviour
{
    public float DestroyTime = 0.1f;

    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
}
