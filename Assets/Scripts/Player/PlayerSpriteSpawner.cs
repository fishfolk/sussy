using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class to spawn the correct Player Sprite/Object Prefab

public class PlayerSpriteSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> playersPrefabList;
    [SerializeField] GameObject ImposterPrefab;

    GameObject currentPlayer;

    public void SpawnPlayer(int playerSpriteIndex)
    {
        if (FindObjectOfType<PlayerState>().isCrewMate)
            StartGameCrewmate(playerSpriteIndex);
        else
            StartGameImposter();
    }

    public void StartGameCrewmate(int playerSpriteIndex)
    {
        int arrayIndex = playerSpriteIndex;
        if (playerSpriteIndex >= playersPrefabList.Count)
        {
            arrayIndex = playerSpriteIndex - (playerSpriteIndex / playersPrefabList.Count) * playersPrefabList.Count;
        }

        currentPlayer = Instantiate(playersPrefabList[arrayIndex], this.transform);
    }

    void StartGameImposter()
    {
        currentPlayer = Instantiate(ImposterPrefab, this.transform);
    }
}
