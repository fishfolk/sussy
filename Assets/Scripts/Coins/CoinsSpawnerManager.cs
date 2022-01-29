using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawnerManager : MonoBehaviour
{
    [SerializeField] List<Sprite> itemsSprites;
    [SerializeField] GameObject coinObject;

    public List<Transform> spawnPoints;

    private void Awake()
    {
        Randomizer.Randomize(spawnPoints);
    }

    public void SpawnCoin(int id)
    {
        Coin c = Instantiate(coinObject, spawnPoints[id].position, Quaternion.identity).GetComponentInChildren<Coin>();
        c.ID = id;
        c.SetCoinSprite(itemsSprites[Random.Range(0, itemsSprites.Count)]);
    }

    int currentCoinListIndex = -1;
    public void SpawnCoin()
    {
        currentCoinListIndex++;

        if (currentCoinListIndex >= spawnPoints.Count)
            currentCoinListIndex = 0;

        Coin c = Instantiate(coinObject, spawnPoints[currentCoinListIndex].position, Quaternion.identity).GetComponentInChildren<Coin>();
        c.SetCoinSprite(itemsSprites[Random.Range(0, itemsSprites.Count)]);
    }


    public void RemoveCoin(int id)
    {
        foreach (var v in FindObjectsOfType<Coin>())
        {
            if (v.ID == id)
            {
                v.RemoveCoin();
            }
        }
    }

    public void RemoveAllCoins()
    {
        foreach (var v in FindObjectsOfType<Coin>())
            v.RemoveCoin();
    }

}
