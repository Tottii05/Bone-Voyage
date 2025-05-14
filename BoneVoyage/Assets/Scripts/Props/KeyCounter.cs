using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCounter : MonoBehaviour
{
    public int keys = 0;
    public int keysObtained = 0;
    public int currentLevelIndex;
    private LevelPickerBehaviour levelPicker;       
    public void Start()
    {
        keys = GameObject.FindGameObjectsWithTag("Key").Length;
        levelPicker = FindObjectOfType<LevelPickerBehaviour>();
    }
    public void IncrementKeysObtained()
    {
        keysObtained++;
        if (keys <= keysObtained)
        {
            levelPicker.canBeShown = true;
            GameManagerScript.instance.MarkLevelCompleted(currentLevelIndex);
            Destroy(gameObject);
        }
    }
}