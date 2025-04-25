using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject spawn;
    public GameObject player;
    public GameObject playerWorldMap;
    public static GameManagerScript instance;

    public List<bool> levelCompletionStatus = new List<bool>();
    public int totalLevels = 9;

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLevelCompletion(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLevelCompletion(bool resetOnStart)
    {
        levelCompletionStatus.Clear();

        for (int i = 0; i < totalLevels; i++)
        {
            string key = $"Level_{i}";
            bool isCompleted;

            if (resetOnStart)
            {
                isCompleted = false;
                PlayerPrefs.SetInt(key, 0);
            }
            else
            {
                isCompleted = PlayerPrefs.GetInt(key, 0) == 1;
            }

            levelCompletionStatus.Add(isCompleted);
        }

        if (resetOnStart)
        {
            PlayerPrefs.Save();
        }
    }

    public void MarkLevelCompleted(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelCompletionStatus.Count)
        {
            levelCompletionStatus[levelIndex] = true;
            string key = $"Level_{levelIndex}";
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
        }
    }

    public bool IsLevelCompleted(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelCompletionStatus.Count)
        {
            return levelCompletionStatus[levelIndex];
        }
        return false;
    }

    public int GetTotalLevels()
    {
        return totalLevels;
    }

    public void ResetLevelCompletion()
    {
        InitializeLevelCompletion(true);
    }

    public void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name != "WorldMap")
        {
            spawn = GameObject.Find("PlayerSpawn");
        }
        if (SceneManager.GetActiveScene().name == "WorldMap")
        {
            spawn = GameObject.Find("PlayerSpawn");
            if (playerWorldMap != null)
            {
                Instantiate(playerWorldMap, spawn.transform.position, spawn.transform.rotation);
            }
        }

        if (spawn != null && SceneManager.GetActiveScene().name != "WorldMap")
        {
            GameObject.Find("Main Camera").GetComponent<CameraBehaviour>().player = GameObject.FindGameObjectWithTag("Player");
            GameObject.Find("Main Camera").GetComponent<CameraBehaviour>().playerRenderers = player.GetComponentsInChildren<Renderer>();
            Instantiate(player, spawn.transform.position, spawn.transform.rotation);
        }
    }

    public IEnumerator Waiter(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}