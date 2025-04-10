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
    public GameObject panelSettings;
    public GameObject buttonEnter;
    public TextMeshProUGUI textTitle;
    public Transform titleTransform;
    private static bool hasOpenedMenu = false;
    public float timeLapse = 2f;
    void Start()
    {
        if (!hasOpenedMenu)
        {
            
            panelOptions.SetActive(false);
            buttonEnter.SetActive(true);
        }
        else
        {
            textTitle.GetComponent<Animator>().SetTrigger("Start");
            panelOptions.SetActive(true);
            buttonEnter.SetActive(false);
        }
        panelSettings.SetActive(false);
    }
    public void ButtonPress()
    {
        textTitle.GetComponent<Animator>().SetTrigger("Start");
        hasOpenedMenu = true;
        buttonEnter.SetActive(false);
        panelOptions.SetActive(true);

    }



}
