using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class to spawn the correct Player Sprite/Object Prefab

public class PlayerSpriteSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> playersPrefabList;
    [SerializeField] GameObject ImposterPrefab;

    GameObject currentPlayer;

    public void SpawnPlayer()
    {
        if (FindObjectOfType<PlayerState>().isCrewMate)
            StartGameCrewmate();
        else
            StartGameImposter();
    }

    public void StartGameCrewmate()
    {
        currentPlayer = Instantiate(playersPrefabList[Random.Range(0, playersPrefabList.Count)], this.transform);
    }

    void StartGameImposter()
    {
        currentPlayer = Instantiate(ImposterPrefab, this.transform);
    }
}
