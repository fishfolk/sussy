using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player Classes and Components
    PlayerAnimator playerAnimator;
    SpriteRenderer playerSpriteRenderer;
    Rigidbody2D rigidBody2D;

    public void StartPlayer()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        // playerAudioController = GetComponent<PlayerAudioController>();

        MovePlayer();
    }

    #region Movement
    [SerializeField] float Speed = 5f;
    float currentSpeed = 0;

    bool CanMove = false;
    bool isMoving = false;

    public void FixedUpdateMovement(float inputHor, float inputVer)
    {
        if (CanMove)
        {
            ChangeSpeed(inputHor, inputVer);

            rigidBody2D.velocity = new Vector2(inputHor * currentSpeed, inputVer * currentSpeed);

            UpdatePlayerCompnments(inputHor, inputVer);
        }
    }

    public void UpdatePlayerCompnments(float inputHor, float inputVer)
    {
        FlipSprite(inputHor);

        isMoving = inputHor != 0 || inputVer != 0;
        if (isMoving)
        {
            playerAnimator.MovePlayer();
        }
        else
        {
            playerAnimator.StopPlayer();
        }
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

    #endregion


    #region Change Player Properties
    public void MovePlayer()
    {
        CanMove = true;
    }
    public void StopPlayer()
    {
        CanMove = false;
    }
    public void KillPlayer()
    {
        CanMove = false;
        playerAnimator.PlayerDead();
    }
    #endregion
}
