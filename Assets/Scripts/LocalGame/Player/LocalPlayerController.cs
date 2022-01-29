using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalPlayerController : MonoBehaviour
{
    [SerializeField] int id;
    public int GetID() { return id; }

    [SerializeField] TextMeshPro playerName;
    public void SetName(string playerName) { this.playerName.text = playerName; }
    public string GetName() { return playerName.text; }

    bool isReady = false;
    public void SetIsReady(bool flag) { isReady = flag; }
    public bool IsReady() { return isReady; }

    LocalPlayerMovement localPlayerMovement;
    ControlsManager controlsManager;
    LocalPlayerKillController localPlayerKillController;

    public bool isImposter = false;

    int currentWeapons = 0;
    public int GetCurrentWeapons() { return currentWeapons; }

    [SerializeField] TextMeshProUGUI weaponsCountText;
    [SerializeField] StatePanelController statePanelController;

    private void Awake()
    {
        localPlayerMovement = GetComponent<LocalPlayerMovement>();
        controlsManager = FindObjectOfType<ControlsManager>();
        localPlayerKillController = GetComponent<LocalPlayerKillController>();

        localPlayerMovement.SetID(id);
        weaponsCountText.text = currentWeapons.ToString();
    }

    public void StartPlayer()
    {
        localPlayerMovement.MovePlayer();
    }
    public void StopPlayer()
    {
        localPlayerMovement.StopPlayer();
    }

    public void CollectCoin()
    {
        currentWeapons++;
        weaponsCountText.text = currentWeapons.ToString();
    }

    void Update()
    {
        if (isImposter)
            if (Input.GetKey(controlsManager.GetKey(id, ControlKeys.ActionKey)))
            {
                localPlayerKillController.Kill();
            }
    }

    public void Kill()
    {
        foreach (var v in localPlayerMovement.gameObject.GetComponentsInChildren<BoxCollider2D>())
            if (v.gameObject.tag == "Crewmate")
                v.gameObject.tag = "Trigger";

        localPlayerMovement.StopPlayer();
        GetComponentInChildren<PlayerAnimator>().PlayerDead();
    }

    public void SetIsImposter()
    {
        statePanelController.SetImposter();
        isImposter = true;
        localPlayerMovement.Speed = 10;

        localPlayerKillController.KillButton.SetActive(true);
    }

    public void SetIsCrewMate()
    {
        statePanelController.SetCrewmate();

        localPlayerKillController.KillButton.SetActive(false);
    }
}
