using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerMovement : MonoBehaviour
{
    //Important so we can get the correct Controls
    int id;
    public void SetID(int id) { this.id = id; }

    [SerializeField] public float Speed = 7.5f;
    float currentSpeed = 0;

    ControlsManager controlsManager;
    bool CanMove = false;
    bool isMoving;
    SpriteRenderer playerSpriteRenderer;
    Rigidbody2D rigidBody2D;
    PlayerAnimator playerAnimator;

    void Start()
    {
        //get the ControlManager in the scene
        playerAnimator = GetComponent<PlayerAnimator>();
        controlsManager = FindObjectOfType<ControlsManager>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (CanMove)
        {
            float inputHor = 0;
            float inputVer = 0;

            if (Input.GetKey(controlsManager.GetKey(id, ControlKeys.UpKey)))
                inputVer += 1;

            if (Input.GetKey(controlsManager.GetKey(id, ControlKeys.DownKey)))
                inputVer -= 1;

            if (Input.GetKey(controlsManager.GetKey(id, ControlKeys.LeftKey)))
                inputHor -= 1;

            if (Input.GetKey(controlsManager.GetKey(id, ControlKeys.RightKey)))
                inputHor += 1;

            FixedUpdateMovement(inputHor, inputVer);
        }
    }

    void FixedUpdateMovement(float inputHor, float inputVer)
    {
        ChangeSpeed(inputHor, inputVer);

        rigidBody2D.velocity = new Vector2(inputHor * currentSpeed, inputVer * currentSpeed);

        UpdatePlayerCompnments(inputHor, inputVer);
    }

    public void UpdatePlayerCompnments(float inputHor, float inputVer)
    {
        FlipSprite(inputHor);

        isMoving = inputHor != 0 || inputVer != 0;
        if (isMoving)
            playerAnimator.MovePlayer();
        else
            playerAnimator.StopPlayer();
    }

    void FlipSprite(float inputDirection)
    {
        if (inputDirection > 0)
            playerSpriteRenderer.flipX = false;
        else if (inputDirection < 0)
            playerSpriteRenderer.flipX = true;
    }

    //Ramp Speed
    void ChangeSpeed(float inputHorDirection, float inputVerDirection)
    {
        //if the movemont started
        if (inputHorDirection != 0 || inputVerDirection != 0)
        {
            //Start Ramping Speed until reach the limit
            currentSpeed += 0.75f;
            if (currentSpeed >= Speed)
                currentSpeed = Speed;
        }
        else
        {
            currentSpeed -= 0.5f;

            //Stop moving when the speed go to 0
            if (currentSpeed <= 0)
            {
                currentSpeed = 0;
            }
        }
    }

    #region Change Player Properties
    public void MovePlayer()
    {
        CanMove = true;
    }
    public void StopPlayer()
    {
        CanMove = false;
        FixedUpdateMovement(0, 0);
    }
    public void KillPlayer()
    {
        CanMove = false;
        playerAnimator.PlayerDead();
    }
    #endregion
}