using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawnerManager : MonoBehaviour
{
    [SerializeField] List<GameObject> itemsPrefab;
    public List<Transform> spawnPoints;

    public void SpawnCoin(int id)
    {
        Coin c = Instantiate(itemsPrefab[Random.Range(0, itemsPrefab.Count)], spawnPoints[id].position, Quaternion.identity).GetComponentInChildren<Coin>();
        c.ID = id;

        ExplosionsSpawner.Instance.Spawn(ExplosionsType.big, spawnPoints[id].position);
    }

    public void RemoveCoin(int id)
    {
        foreach (var v in FindObjectsOfType<Coin>())
        {
            if (v.ID == id)
            {
                v.RemoveCoin();
                ExplosionsSpawner.Instance.Spawn(ExplosionsType.small, v.transform.position);
            }
        }
    }

    public void RemoveAllCoins()
    {
        foreach (var v in FindObjectsOfType<Coin>())
            v.RemoveCoin();
    }

}
