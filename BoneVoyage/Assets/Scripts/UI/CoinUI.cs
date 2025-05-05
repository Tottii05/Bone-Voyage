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

    private void UpdateCoinText()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = coins.ToString();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.DeleteKey("Coins");
        }
    }
}