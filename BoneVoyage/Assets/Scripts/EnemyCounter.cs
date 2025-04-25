using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public float enemyCount = 0f;
    public float enemyKilled = 0f;
    public GameObject itemToDestroy;
    public int currentLevelIndex;

    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;
        EnemyController.OnEnemyDeath += IncrementKillCount;
    }

    void OnDestroy()
    {
        EnemyController.OnEnemyDeath -= IncrementKillCount;
    }

    private void IncrementKillCount()
    {
        enemyKilled++;
        if (enemyKilled >= enemyCount)
        {
            GameManagerScript.instance.MarkLevelCompleted(currentLevelIndex);
            DestroyItem();
            LevelPickerBehaviour levelPicker = FindObjectOfType<LevelPickerBehaviour>();
            levelPicker.canBeShown = true;
        }
    }

    public void DestroyItem()
    {
        if (itemToDestroy != null)
        {
            Destroy(itemToDestroy);
        }
    }
}