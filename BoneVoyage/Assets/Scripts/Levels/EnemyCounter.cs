using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public float enemiesToKill = 0f;
    public float enemyKilled = 0f;
    public GameObject itemToDestroy;
    public int currentLevelIndex;

    void Start()
    {
        EnemyController.OnEnemyDeath += IncrementKillCount;
    }

    void OnDestroy()
    {
        EnemyController.OnEnemyDeath -= IncrementKillCount;
    }

    private void IncrementKillCount()
    {
        enemyKilled++;
        if (enemyKilled >= enemiesToKill)
        {
            GameManagerScript.instance.MarkLevelCompleted(currentLevelIndex);
            if (gameObject.name == "FinalWall" || gameObject.name == "FinalFence")
            {
                if (gameObject.name == "FinalWall")
                {
                    LevelPickerBehaviour levelPicker = FindObjectOfType<LevelPickerBehaviour>();
                    levelPicker.canBeShown = true;
                }
            }
            DestroyItem();
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