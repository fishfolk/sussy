using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsManager : MonoBehaviour
{
    [HideInInspector] public float _inputHor = 0;
    [HideInInspector] public float _inputVer = 0;

    PlayerMovement playerMovement;

    public void Initiate()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
    }

    void FixedUpdate()
    {
        float inputHor = Input.GetAxisRaw("Horizontal");
        float inputVer = Input.GetAxisRaw("Vertical");

        _inputHor = inputHor;
        _inputVer = inputVer;

        if (playerMovement != null)
            playerMovement.FixedUpdateMovement(inputHor, inputVer);
    }
}
