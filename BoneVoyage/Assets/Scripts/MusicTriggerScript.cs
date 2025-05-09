using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerScript : MonoBehaviour
{
    public BackgroundMusicLoaderBehaviour musicSource; 
    // Start is called before the first frame update
    void Start()
    {
        musicSource = GameObject.Find("BGMLoader").GetComponent<BackgroundMusicLoaderBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        if (other.CompareTag("Player"))
        {
            musicSource.StartClip();
        }
        
        StartCoroutine(destroyer());
    }

    private IEnumerator destroyer()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
