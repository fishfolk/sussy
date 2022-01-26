using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class will manage,spawn and remove all players in the client

public class PlayersManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject serverPlayer;

    //These Contains all the Active alive Players in the current Client
    private Dictionary<string, GameObject> playersDictionary;
    private Dictionary<string, string> playersNames;

    [SerializeField] List<Vector3> spwanPositionsList;
    int spwanPositionsListIndex = 0;
    bool isSpawned = false;

    GameConnectionManager gameConnectionManager;

    void Awake()
    {
        gameConnectionManager = FindObjectOfType<GameConnectionManager>();
        playersNames = new Dictionary<string, string>();
        playersDictionary = new Dictionary<string, GameObject>();
    }

    #region Spawning
    public void SpawnPlayers()
    {
        if (!isSpawned)
        {
            List<string> playersSessionIDs = gameConnectionManager.playersSessionIDs;
            string localUserSessionID = gameConnectionManager.localUserSessionID;

            foreach (string id in playersSessionIDs)
            {
                GameObject player = null;

                if (id == localUserSessionID)
                    player = InstantiatePlayer(id);
                else
                    player = InstantiateServerPlayer(id);

                playersDictionary.Add(id, player);

                spwanPositionsListIndex++;
                if (spwanPositionsListIndex >= spwanPositionsList.Count)
                    spwanPositionsListIndex = 0;
            }
            isSpawned = true;
        }
    }

    //Create the main Player
    public GameObject InstantiatePlayer(string id)
    {
        GameObject p = Instantiate(player, Vector3.zero, Quaternion.identity);

        PlayerController playerController = p.GetComponent<PlayerController>();
        playerController.Initiate(id, spwanPositionsList[spwanPositionsListIndex]);

        return p;
    }

    //Create the Server Players
    public GameObject InstantiateServerPlayer(string id)
    {
        GameObject p = Instantiate(serverPlayer, Vector3.zero, Quaternion.identity);

        PlayerServerController playerController = p.GetComponent<PlayerServerController>();
        playerController.Initiate(id, spwanPositionsList[spwanPositionsListIndex]);

        return p;
    }
    #endregion

    #region Meetings
    bool isPlayerDeactivated = false;
    public void DeactivatePlayers()
    {
        foreach (string id in playersDictionary.Keys)
        {
            playersDictionary[id].SetActive(false);
        }
        isPlayerDeactivated = true;
    }
    public void ActivatePlayers()
    {
        if (isPlayerDeactivated)
        {
            isPlayerDeactivated = false;
            foreach (string id in playersDictionary.Keys)
            {
                playersDictionary[id].SetActive(true);
                if (playersDictionary[id].GetComponent<PlayerController>() != null)
                    playersDictionary[id].GetComponent<PlayerController>().SetPos(spwanPositionsList[spwanPositionsListIndex]);
                else
                    playersDictionary[id].GetComponent<PlayerServerController>().SetPos(spwanPositionsList[spwanPositionsListIndex]);

                spwanPositionsListIndex++;
                if (spwanPositionsListIndex >= spwanPositionsList.Count)
                    spwanPositionsListIndex = 0;
            }
        }
    }
    #endregion

    #region Actions Called from Server
    public void AddPlayerName(Dictionary<string, string> state)
    {
        string id = state["userSessionID"];

        playersNames.Add(id, state["playerName"]);

        if (playersDictionary.ContainsKey(id))
        {
            PlayerServerController playerController = playersDictionary[id].GetComponent<PlayerServerController>();
            playerController.SetPlayerName(playersNames[id]);
        }
    }
    public void AddPlayerName(string id, string name)
    {
        playersNames.Add(id, name);
        if (playersDictionary.ContainsKey(id))
        {
            PlayerServerController playerController = playersDictionary[id].GetComponent<PlayerServerController>();
            if (playerController != null)
                playerController.SetPlayerName(playersNames[id]);
        }
    }
    public void MovePlayer(Dictionary<string, string> state)
    {
        string id = state["userSessionID"];
        if (playersDictionary.ContainsKey(id))
        {
            PlayerServerController playerController = playersDictionary[id].GetComponent<PlayerServerController>();
            playerController.SetPlayerState(state);
        }
    }
    public void SetInputPlayer(Dictionary<string, string> state)
    {
        string id = state["userSessionID"];
        if (playersDictionary.ContainsKey(id))
        {
            PlayerServerController playerController = playersDictionary[id].GetComponent<PlayerServerController>();
            playerController.SetInputs(state);
        }
    }

    #region Removing Players
    //Remove all players
    public void KillPlayers()
    {
        if (playersDictionary != null)
        {
            spwanPositionsListIndex = 0;
            foreach (string id in playersDictionary.Keys)
            {
                Destroy(playersDictionary[id]);
            }

            playersDictionary = new Dictionary<string, GameObject>();
            playersNames = new Dictionary<string, string>();

            isSpawned = false;
        }

        foreach (var v in FindObjectsOfType<PlayerMovement>())
        {
            Destroy(v.gameObject);
        }
    }
    public void KillLocalPlayer(string localUserSessionID)
    {
        FindObjectOfType<PlayerState>().Dead();
        FindObjectOfType<PlayerController>().KillPlayer();

        if (playersDictionary.ContainsKey(localUserSessionID))
        {
            playersDictionary.Remove(localUserSessionID);
        }
    }
    public void KillPlayer(Dictionary<string, string> state)
    {
        string id = state["userSessionID"];
        if (playersDictionary.ContainsKey(id))
        {
            PlayerServerController playerController = playersDictionary[id].GetComponent<PlayerServerController>();
            playerController.KillPlayer();

            playersDictionary.Remove(id);
        }
    }
    public void KillPlayer(string id)
    {
        if (playersDictionary.ContainsKey(id))
        {
            PlayerServerController playerController = playersDictionary[id].GetComponent<PlayerServerController>();
            playerController.KillPlayer();

            playersDictionary.Remove(id);
        }
    }
    public void PlayerLeft(Dictionary<string, string> state)
    {
        string id = state["userSessionID"];

        if (playersDictionary.ContainsKey(id))
        {
            if (playersDictionary[id] != null)
                Destroy(playersDictionary[id]);

            playersDictionary.Remove(id);
        }
    }
    #endregion

    #endregion

    public List<string> GetPlayersID()
    {
        List<string> playersIDs = new List<string>();

        foreach (var v in playersDictionary.Keys)
            playersIDs.Add(v);

        return playersIDs;
    }
    public string GetPlayerName(string id)
    {
        if (playersNames.ContainsKey(id))
            return playersNames[id];
        else
            return id;
    }
}
