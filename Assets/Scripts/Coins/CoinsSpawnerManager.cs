using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    public List<Transform> spawnPoints;

    public void SpawnCoin(int id)
    {
        Coin c = Instantiate(coinPrefab, spawnPoints[id].position, Quaternion.identity).GetComponent<Coin>();
        c.ID = id;
    }

    public void RemoveCoin(int id)
    {
        foreach (var v in FindObjectsOfType<Coin>())
        {
            if (v.ID == id)
                v.RemoveCoin();
        }
    }

    public void RemoveAllCoins()
    {
        foreach (var v in FindObjectsOfType<Coin>())
            v.RemoveCoin();
    }

}
