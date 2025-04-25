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
}
