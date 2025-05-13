using System.Collections;
using UnityEngine;

public class growlerScript : MonoBehaviour
{
    public AudioSource growlAudio;

    void Start()
    {
        growlAudio = GetComponent<AudioSource>();
        StartCoroutine(GrowlLoop());
    }

    IEnumerator GrowlLoop()
    {
        while (true)
        {
            int rngTime = Random.Range(5, 10);
            yield return new WaitForSeconds(rngTime);

            if (growlAudio != null && growlAudio.clip != null)
            {
                growlAudio.Play();
                yield return new WaitForSeconds(growlAudio.clip.length);
            }
            else
            {
                Debug.LogWarning("No se ha asignado un AudioClip al AudioSource.");
                yield return new WaitForSeconds(1f); // Espera un poco antes de intentar de nuevo
            }
        }
    }
}
