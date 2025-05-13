using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolumeAdjuster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
