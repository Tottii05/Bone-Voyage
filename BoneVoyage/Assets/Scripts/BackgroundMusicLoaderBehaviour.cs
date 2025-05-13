using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicLoaderBehaviour : MonoBehaviour
{
    public AudioClip BackgroundMusic;
    public AudioClip BackgroundMusicAlt;
    public AudioSource AudioSource;

    private void Awake()
    {   
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        if (BackgroundMusicAlt == null) AudioSource.clip = BackgroundMusic;
        else
        {
            if (Random.Range(0, 100) > 1)
            {
                AudioSource.clip = BackgroundMusic;
            }
            else
            {
                AudioSource.clip = BackgroundMusicAlt;
            }
        }
        if (AudioSource.playOnAwake == true) this.StartClip();
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
