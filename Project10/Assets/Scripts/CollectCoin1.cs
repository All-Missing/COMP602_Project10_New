using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CollectCoin : MonoBehaviour
{
    private int coinCount = 0; // Rename to avoid confusion with property
    public TMPro.TextMeshProUGUI CoinText; 

    public int CoinCount => coinCount; // Public property to access coin count

    public void OnTriggerEnter(Collider other) // Change to public
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++; // Increment coinCount
            
            if (CoinText != null)
            {
                CoinText.text = "Coins: " + coinCount.ToString();
            }

            Debug.Log(coinCount);

            DestroyImmediate(other.gameObject); // Use DestroyImmediate instead of Destroy
        }
    }

    // Public method to add coins manually
    public void AddCoins(int amount)
    {
        coinCount += amount; // Increase the coin count by the given amount
        if (CoinText != null)
        {
            CoinText.text = "Coins: " + coinCount.ToString(); // Update the UI
        }
        Debug.Log("Added " + amount + " coins. Total coins: " + coinCount);
    }
}