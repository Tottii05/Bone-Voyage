using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Player;

public class BuyItem : MonoBehaviour, IInteractActions
{
    private Player playerActions;
    public GameObject priceText;
    public float price = 3;
    public bool playerInRange = false;
    public CoinUI coinUI;
    public string playerPrefsKey;
    public int playerPrefsValue;

    public void Awake()
    {
        playerActions = new Player();
        playerActions.Interact.SetCallbacks(this);
        coinUI = GameObject.Find("CoinCounter").GetComponent<CoinUI>();
    }

    public void OnEnable()
    {
        playerActions.Enable();
    }

    public void OnDisable()
    {
        playerActions.Disable();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            priceText.SetActive(true);
            priceText.GetComponent<TMPro.TextMeshPro>().text = price.ToString();
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            priceText.SetActive(false);
            playerInRange = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && playerInRange)
        {
            int currentCoins = PlayerPrefs.GetInt("Coins", 0);
            if (currentCoins >= price)
            {
                PlayerPrefs.SetInt("Coins", currentCoins - (int)price);
                PlayerPrefs.SetInt(playerPrefsKey, playerPrefsValue);
                PlayerPrefs.Save();
                coinUI.UpdateCoinText();
                Destroy(gameObject);
                Destroy(priceText);
            }
        }
    }
}