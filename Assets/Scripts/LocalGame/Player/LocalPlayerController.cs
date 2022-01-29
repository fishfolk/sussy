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
    LocalPlayerInventory localPlayerInventory;

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
        localPlayerInventory = GetComponent<LocalPlayerInventory>();

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
        if (Input.GetKey(controlsManager.GetKey(id, ControlKeys.ActionKey)))
        {
            if (isImposter)
                localPlayerKillController.Kill();

            if (!isImposter)
                localPlayerInventory.PlantMine();
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

    public void Stun()
    {
        localPlayerMovement.StopPlayer();
        isImposter = false;

        GetComponentInChildren<PlayerAnimator>().PlayerDead();
        ExplosionsSpawner.Instance.Spawn(ExplosionsType.big, transform.position);
        Invoke("ReleaseStun", 5);
    }

    public void ReleaseStun()
    {
        localPlayerMovement.MovePlayer();
        isImposter = true;

        GetComponentInChildren<PlayerAnimator>().ReleaseStun();
    }

    public void SetIsImposter()
    {
        statePanelController.SetImposter();
        isImposter = true;
        localPlayerMovement.Speed = 10;

        localPlayerKillController.StartImposter();

        localPlayerInventory.MineButton.SetActive(false);

        playerName.color = Color.red;
    }

    public void SetIsCrewMate()
    {
        statePanelController.SetCrewmate();

        localPlayerInventory.StartCrewMate();

        localPlayerKillController.KillButton.SetActive(false);
    }
}
