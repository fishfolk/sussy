using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This is the local Playercontroller class that send messages into the server
// Only one Instance per client

public class PlayerController : MonoBehaviour
{
    public string Id;
    GameConnectionManager gameConnectionManager;
    PlayerControlsManager playerControlsManager;
    Rigidbody2D rigidbody2D;

    string previousDateState = "";
    string previousInputState = "";

    bool isDead = false;

    private void Awake()
    {
        playerControlsManager = GetComponent<PlayerControlsManager>();
    }

    public void Initiate(string Id, Vector3 pos)
    {
        this.Id = Id;
        GetComponent<PlayerSpriteSpawner>().SpawnPlayer();
        GetComponentInChildren<PlayerMovement>().StartPlayer();
        rigidbody2D = GetComponentInChildren<Rigidbody2D>();

        rigidbody2D.gameObject.transform.localPosition = pos;

        gameConnectionManager = FindObjectOfType<GameConnectionManager>();

        FindObjectOfType<CameraController>().Target = GetComponentInChildren<PlayerMovement>().gameObject;

        playerControlsManager.Initiate();

        //Set the Camera Size based on the type of the player
        if (FindObjectOfType<PlayerState>().isImposter)
        {
            this.gameObject.AddComponent<PlayerKillController>();
            FindObjectOfType<Camera>().orthographicSize = 5;
        }
        else
            FindObjectOfType<Camera>().orthographicSize = 4;

        GetComponentInChildren<PlayerMovement>().gameObject.AddComponent<PlayerCollider>();
        SetPlayerName(FindObjectOfType<GameConnectionUIManager>().GetPlayerName());

        startSync = true;
    }

    public void SetPos(Vector3 pos)
    {
        rigidbody2D.gameObject.transform.localPosition = pos;
    }

    public void SetPlayerName(string name)
    {
        GetComponentInChildren<TextMeshPro>().text = name;

        if (FindObjectOfType<PlayerState>().isImposter)
            GetComponentInChildren<TextMeshPro>().color = Color.red;
    }

    public void KillPlayer()
    {
        isDead = true;
        GetComponentInChildren<PlayerAnimator>().PlayerDead();
        GetComponentInChildren<PlayerMovement>().StopPlayer();
    }

    #region Network Sync
    //Sending the Data to the server

    float StateFrequency = 0.05f;
    private float stateSyncTimer;
    bool startSync = false;

    private void LateUpdate()
    {
        if (isDead)
            return;

        if (startSync)
        {
            if (stateSyncTimer <= 0)
            {
                string jsonState = MatchDataJson.SetPlayerData(Id, rigidbody2D.gameObject.transform.localPosition, rigidbody2D.velocity);
                if (previousDateState != jsonState)
                {
                    previousDateState = jsonState;
                    gameConnectionManager.SendMatchState(OpCodes.PlayerChange, jsonState);
                }

                jsonState = MatchDataJson.SetInput(Id, playerControlsManager._inputHor, playerControlsManager._inputVer);
                if (previousInputState != jsonState)
                {
                    previousInputState = jsonState;
                    gameConnectionManager.SendMatchState(OpCodes.Input, jsonState);
                }

                stateSyncTimer = StateFrequency;
            }
            stateSyncTimer -= Time.deltaTime;
        }
    }
    #endregion
}
