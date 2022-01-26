using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This is the Server PlayerController class that recieve messages into the server
// one Instance per Player in Client

public class PlayerServerController : MonoBehaviour
{
    public string Id;

    Rigidbody2D rigidbody2D;
    PlayerMovement playerMovement;
    TextMeshPro playerName;

    bool isDead = false;


    public void Initiate(string Id, Vector3 pos)
    {
        this.Id = Id;
        GetComponent<PlayerSpriteSpawner>().StartGameCrewmate();
        rigidbody2D = GetComponentInChildren<Rigidbody2D>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerMovement.StartPlayer();

        playerMovement.gameObject.transform.localPosition = pos;

        if (FindObjectOfType<PlayerState>().isImposter)
        {
            foreach (var v in playerMovement.gameObject.GetComponentsInChildren<BoxCollider2D>())
                if (v.gameObject.tag == "Trigger")
                    v.gameObject.tag = "Crewmate";
        }
    }
    public void SetPos(Vector3 pos)
    {
        playerMovement.gameObject.transform.localPosition = pos;
    }

    public void SetPlayerName(string name)
    {
        playerName = GetComponentInChildren<TextMeshPro>();
        playerName.text = name;
    }

    public void SetPlayerState(Dictionary<string, string> state)
    {
        if (isDead)
            return;

        Vector2 pos = new Vector2(float.Parse(state["pos_x"]), float.Parse(state["pos_y"]));

        // Begin lerping to the corrected position.
        lerpFromPosition = playerMovement.gameObject.transform.localPosition;
        lerpToPosition = pos;
        lerpTimer = 0;
        lerpPosition = true;
    }

    public void SetInputs(Dictionary<string, string> state)
    {
        if (isDead)
            return;

        playerMovement.UpdatePlayerCompnments(float.Parse(state["hor_Input"]), float.Parse(state["ver_Input"]));
    }

    public void KillPlayer()
    {
        foreach (var v in playerMovement.gameObject.GetComponentsInChildren<BoxCollider2D>())
            if (v.gameObject.tag == "Crewmate")
                v.gameObject.tag = "Trigger";

        isDead = true;
        playerMovement.StopPlayer();
        GetComponentInChildren<PlayerAnimator>().PlayerDead();
    }

    #region Position Lerping

    public float LerpTime = 0.05f;
    private float lerpTimer;
    private Vector3 lerpFromPosition;
    private Vector3 lerpToPosition;
    private bool lerpPosition;

    private void FixedUpdate()
    {
        // If we aren't trying to interpolate the player's position then return early.
        if (!lerpPosition)
        {
            return;
        }

        // Interpolate the player's position based on the lerp timer progress.
        playerMovement.gameObject.transform.localPosition = Vector3.Lerp(lerpFromPosition, lerpToPosition, lerpTimer / LerpTime);
        lerpTimer += Time.deltaTime;

        // If we have reached the end of the lerp timer, explicitly force the player to the last known correct position.
        if (lerpTimer >= LerpTime)
        {
            playerMovement.gameObject.transform.localPosition = lerpToPosition;
            lerpPosition = false;
        }
    }

    #endregion
}
