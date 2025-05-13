using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static event System.Action CoinCollected;
    public GameObject audioSource;
    public coinSoundScript coinSoundScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 1);
            PlayerPrefs.Save();
            CoinCollected?.Invoke();
            coinSoundScript = audioSource.GetComponent<coinSoundScript>();
            audioSource.gameObject.transform.SetParent(null);
            coinSoundScript.PlayCoinSound();
            Destroy(gameObject);
        }
    }
}