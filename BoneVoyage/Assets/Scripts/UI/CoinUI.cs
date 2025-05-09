using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        UpdateCoinText();
        Coin.CoinCollected += UpdateCoinText;
    }

    private void Start()
    {
        UpdateCoinText();
    }

    private void OnDestroy()
    {
        Coin.CoinCollected -= UpdateCoinText;
    }

    public void UpdateCoinText()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = coins.ToString();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.SetInt("Coins", 0);
            PlayerPrefs.Save();
            UpdateCoinText();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetInt("Coins", 5);
            PlayerPrefs.Save();
            UpdateCoinText();
        }
    }
}