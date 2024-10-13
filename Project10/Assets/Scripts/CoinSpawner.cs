using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject Coin;  
    public int coinAmount = 10; 
    public Vector3 spawnArea = new Vector3(10, 1, 10); 
    public Vector3 centerPosition = new Vector3(-23.77426f, 15.975f, -31.79031f); 

    void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    IEnumerator SpawnCoins()
    {
        int attempts = 0;
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 randomPosition;
            bool positionValid = false;

            // Retry up to 10 times to avoid overlapping coins
            while (!positionValid && attempts < 10)
            {
                // Calculate random position within the spawn area, centered around the specified location
                randomPosition = new Vector3(
                    Random.Range(centerPosition.x - spawnArea.x / 2, centerPosition.x + spawnArea.x / 2), 
                    centerPosition.y, // Y position is fixed at the center's Y level
                    Random.Range(centerPosition.z - spawnArea.z / 2, centerPosition.z + spawnArea.z / 2)
                );

                // Check if the position is already occupied by another object
                if (!Physics.CheckSphere(randomPosition, 0.5f)) // Adjust radius accordingly
                {
                    Instantiate(Coin, randomPosition, Quaternion.identity);
                    positionValid = true;
                }

                attempts++;
            }

            if (attempts >= 10)
            {
                Debug.LogWarning("Failed to find valid position for coin");
            }

            yield return new WaitForSeconds(0.1f); // Wait a short time between spawns
        }
    }
}
