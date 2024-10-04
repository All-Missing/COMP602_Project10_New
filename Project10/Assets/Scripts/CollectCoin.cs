using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CollectCoin : MonoBehaviour
{
    private int Coin = 0;
    public TMPro.TextMeshProUGUI CoinText; 

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin")) 
        {
            Coin++;
            
            if (CoinText != null)
            {
                CoinText.text = "Coins: " + Coin.ToString();
            }

           
            Debug.Log(Coin);

            
            Destroy(other.gameObject);
        }
    }
}
