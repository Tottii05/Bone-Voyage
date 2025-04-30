using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCounter : MonoBehaviour
{
    public int keys = 0;
    public int keysObtained = 0;
    public int currentLevelIndex;

    public void Start()
    {
        keys = GameObject.FindGameObjectsWithTag("Key").Length;
    }
    public void IncrementKeysObtained()
    {
        keysObtained++;
        if (keys <= keysObtained)
        {
            Destroy(gameObject);
        }
    }
}