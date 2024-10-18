using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD

public class CoinSpawner : MonoBehaviour
{
    public GameObject Coin;  
    public int coinAmount = 10;
    public Vector3 spawnArea = new Vector3(10, 1, 10);
    public Vector3 centerPosition = new Vector3(-23.77426f, 15.975f, -31.79031f);

=======
public class CoinSpawner : MonoBehaviour
{
    public GameObject Coin;  
    public int coinAmount = 10; 
    public Vector3 spawnArea = new Vector3(10, 1, 10); 
    public Vector3 centerPosition = new Vector3(-23.77426f, 15.975f, -31.79031f); 
>>>>>>> main

    void Start()
    {
        StartCoroutine(SpawnCoins());
    }

<<<<<<< HEAD

    public IEnumerator SpawnCoins() // Ensure this method is public
    {
=======
    public IEnumerator SpawnCoins()
    {
        int attempts = 0;
>>>>>>> main
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 randomPosition;
            bool positionValid = false;
<<<<<<< HEAD
            int attempts = 0; // Move attempts counter inside the loop

=======
>>>>>>> main

            // Retry up to 10 times to avoid overlapping coins
            while (!positionValid && attempts < 10)
            {
<<<<<<< HEAD
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
=======
                // Calculate random position within the spawn area, centered around the specified location
                randomPosition = new Vector3(
                    Random.Range(centerPosition.x - spawnArea.x / 2, centerPosition.x + spawnArea.x / 2), 
                    centerPosition.y, // Y position is fixed at the center's Y level
                    Random.Range(centerPosition.z - spawnArea.z / 2, centerPosition.z + spawnArea.z / 2)
                );

                // Check if the position is already occupied by another object
                if (!Physics.CheckSphere(randomPosition, 0.5f)) // Adjust radius accordingly
>>>>>>> main
                {
                    Instantiate(Coin, randomPosition, Quaternion.identity);
                    positionValid = true;
                }
<<<<<<< HEAD
                else
                {
                    Debug.Log($"Position occupied: {randomPosition}");
                }

=======
>>>>>>> main

                attempts++;
            }

<<<<<<< HEAD

            if (!positionValid)
            {
                Debug.LogWarning("Failed to find valid position for coin after 10 attempts");
            }


=======
            if (attempts >= 10)
            {
                Debug.LogWarning("Failed to find valid position for coin");
            }

>>>>>>> main
            yield return new WaitForSeconds(0.1f); // Wait a short time between spawns
        }
    }
}
<<<<<<< HEAD

=======
>>>>>>> main
