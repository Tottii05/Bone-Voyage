using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinSoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCoinSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource component not found on the GameObject.");
        }
    }
}
