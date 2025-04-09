using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : SceneController
{
    public GameObject panelOptions;
    public GameObject buttonEnter;
    public TextMeshProUGUI textTitle;
    void Start()
    {
        if(PlayerPrefs.GetInt("FirstTime",0) == 0)
        {
            panelOptions.SetActive(true);
            buttonEnter.SetActive(true);
        }
        else
        {
            panelOptions.SetActive(false);
            buttonEnter.SetActive(false);
        }
    }
    public void ButtonPress()
    {
        this.gameObject.SetActive(false);
    }
    public void MoveTitle()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
