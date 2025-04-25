using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethods : MonoBehaviour
{
    public void LoadWorldMap()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManagerScript>().playerWorldMap != null)
        {
            SceneManager.LoadScene("WorldMap");
        }
    }
    public void LoadScene(string name)
    {
        if (name == "CharacterSelection")
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().playerWorldMap = null;
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().player = null;
        }
        SceneManager.LoadScene(name);
    }
}
