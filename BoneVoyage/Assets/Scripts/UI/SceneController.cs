using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetPanel(GameObject panel)
    {
        
        panel.SetActive(!panel.activeSelf);
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("FirstTime", 1);
        Application.Quit();
    }

    
}
