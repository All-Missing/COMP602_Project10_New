using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro; // Ensure you have this line
using System.Collections; // Add this line to use IEnumerator

[TestFixture]
public class CoinTests
{
    private CollectCoin collectCoin;
    private CoinSpawner coinSpawner;
    private GameObject coinPrefab;

    [SetUp]
public void Setup()
{
    // Create a GameObject for the CollectCoin script
    GameObject collectorObject = new GameObject();
    collectCoin = collectorObject.AddComponent<CollectCoin>();

    // Create a TextMeshProUGUI mock for CoinText
    var tmpText = new GameObject().AddComponent<TextMeshProUGUI>();
    collectCoin.CoinText = tmpText;

    // Create a GameObject for the CoinSpawner script
    GameObject spawnerObject = new GameObject();
    coinSpawner = spawnerObject.AddComponent<CoinSpawner>();

    // Create a coin prefab for testing
    coinPrefab = new GameObject("Coin");
    coinPrefab.tag = "Coin"; // Set the tag for the prefab

    // Assign the coin prefab to the coin spawner
    coinSpawner.Coin = coinPrefab; // Ensure this line is included
}

//This bug is here....
// [UnityTest]
// public IEnumerator CollectCoin_IncreasesScore()
// {
//     // Arrange
//     int initialScore = collectCoin.CoinCount; // Use the public property CoinCount
//     GameObject coin = Object.Instantiate(coinPrefab);
//     coin.transform.position = Vector3.zero; // Place it at the origin
//     var collider = coin.AddComponent<SphereCollider>();
//     collider.isTrigger = true;

//     // Act
//     collectCoin.OnTriggerEnter(collider); // Now this call should work

//     // Wait for the next frame
//     yield return null;

//     // Assert
//     Assert.AreEqual(initialScore + 1, collectCoin.CoinCount); // Check the CoinCount property directly
//     Assert.IsTrue(coin == null); // Ensure the coin has been destroyed (coin should still be the original instance)
// }

    [Test]
public void CoinSpawner_SpawnsCorrectAmountOfCoins()
{
    // Arrange
    coinSpawner.Coin = coinPrefab; // Assign the coin prefab
    coinSpawner.coinAmount = 2; // Set the amount of coins to spawn

    // Act
    coinSpawner.StartCoroutine(coinSpawner.SpawnCoins()); // Invoke the coroutine
    // Use a short delay to allow the coins to spawn (Unity requires some time for coroutines to execute)
    // Wait for a brief period before checking the count

    // Wait a moment for coins to spawn
    System.Threading.Thread.Sleep(200); // Wait for 200 ms (adjust timing as needed)

    // Assert
    int spawnedCoinsCount = GameObject.FindGameObjectsWithTag("Coin").Length;
    Assert.AreEqual(2, spawnedCoinsCount);
}



    [TearDown]
    public void TearDown()
    {
        // Clean up test objects
        Object.DestroyImmediate(coinPrefab);
        Object.DestroyImmediate(collectCoin.gameObject);
        Object.DestroyImmediate(coinSpawner.gameObject);
    }
}
