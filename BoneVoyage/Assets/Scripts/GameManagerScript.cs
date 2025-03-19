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
    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        spawn = GameObject.Find("SpawnPoint");
        if (SceneManager.GetActiveScene().name == "WorldMap")
        {
            if (playerWorldMap != null)
            {
                Destroy(playerWorldMap);
            }
            Instantiate(playerWorldMap, spawn.transform.position, spawn.transform.rotation);
        }
        if (spawn != null && SceneManager.GetActiveScene().name != "WorldMap")
        {
            Instantiate(player, spawn.transform.position, spawn.transform.rotation);
        }
    }
}
