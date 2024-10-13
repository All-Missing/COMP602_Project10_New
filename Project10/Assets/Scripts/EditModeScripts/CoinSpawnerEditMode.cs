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

    public IEnumerator SpawnCoins() // Ensure this method is public
    {
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 randomPosition;
            bool positionValid = false;
            int attempts = 0; // Move attempts counter inside the loop

            // Retry up to 10 times to avoid overlapping coins
            while (!positionValid && attempts < 10)
            {
                // Calculate random position within the spawn area
                randomPosition = new Vector3(
                    Random.Range(centerPosition.x - spawnArea.x / 2, centerPosition.x + spawnArea.x / 2), 
                    centerPosition.y, 
                    Random.Range(centerPosition.z - spawnArea.z / 2, centerPosition.z + spawnArea.z / 2)
                );

                // Log the attempted position
                Debug.Log($"Attempting to spawn coin at: {randomPosition}");

                // Check if the position is already occupied by another object
                if (!Physics.CheckSphere(randomPosition, 0.5f))
                {
                    Instantiate(Coin, randomPosition, Quaternion.identity);
                    positionValid = true;
                }
                else
                {
                    Debug.Log($"Position occupied: {randomPosition}");
                }

                attempts++;
            }

            if (!positionValid)
            {
                Debug.LogWarning("Failed to find valid position for coin after 10 attempts");
            }

            yield return new WaitForSeconds(0.1f); // Wait a short time between spawns
        }
    }
}
