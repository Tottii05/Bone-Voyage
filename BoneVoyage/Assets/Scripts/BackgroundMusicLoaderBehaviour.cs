using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicLoaderBehaviour : MonoBehaviour
{
    public AudioClip BackgroundMusic;
    public AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();

        AudioSource.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        AudioSource.clip = BackgroundMusic;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartClip()
    {
        Debug.Log("Starting Clip");
        AudioSource.Play();
    }

    public void StopClip()
    {
        Debug.Log("Stopping Clip");
        AudioSource.Stop();
    }
}
