using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    //This is the collider controller class for the player
    PlayerMovement playerMovement;
    PlayerKillController playerKillController;
    PlayerState playerState;

    int playersCount = 0;

    void Awake()
    {
        playerState = FindObjectOfType<PlayerState>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    #region Triggers
    void OnTriggerEnter2D(Collider2D other)
    {
        //When the Imposter Enters a crewmember Trigger
        if (playerState.isImposter)
            if (other.gameObject.tag == "Crewmate")
            {
                PlayerServerController psc = other.GetComponentInParent<PlayerServerController>();

                if (playerKillController == null)
                    playerKillController = GetComponentInParent<PlayerKillController>();

                playerKillController.EnableKilling(psc.Id);
                playersCount++;
            }

        if (playerState.isCrewMate)
        {
            if (other.gameObject.tag == "Coin")
            {
                other.GetComponent<Coin>().CollectCoin();
            }
            if (other.gameObject.tag == "CallMeeting")
            {
                FindObjectOfType<MeetingsManager>().ShowCallMeetingButton();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //When the Imposter Exit a crewmember Trigger
        if (playerState.isImposter)
            if (other.gameObject.tag == "Crewmate")
            {
                playersCount--;

                if (playersCount == 0)
                {
                    PlayerServerController psc = other.GetComponentInParent<PlayerServerController>();

                    if (playerKillController == null)
                        playerKillController = GetComponentInParent<PlayerKillController>();

                    playerKillController.DisableKilling(psc.Id);
                }
            }

        if (playerState.isCrewMate)
        {
            if (other.gameObject.tag == "CallMeeting")
            {
                FindObjectOfType<MeetingsManager>().HideCallMeetingButton();
            }
        }
    }
    #endregion 
}
