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
    public Slider musicSlider;
    public Slider sfxSlider;
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
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void Update()
    {
        if (Input.anyKeyDown && !hasOpenedMenu)
        {
            ButtonPress();
        }
    }
    public void ButtonPress()
    {
        textTitle.GetComponent<Animator>().SetTrigger("Start");
        hasOpenedMenu = true;
        buttonEnter.SetActive(false);
        panelOptions.SetActive(true);

    }
    
    public void onSliderMusicChange(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        Debug.Log("Music volume set to: " + volume);
    }

    public void onSliderSFXChange(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        Debug.Log("SFX volume set to: " + volume);
    }
}
