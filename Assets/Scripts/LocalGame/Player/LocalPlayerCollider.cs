using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerCollider : MonoBehaviour
{
    //This is the collider controller class for the player
    LocalPlayerMovement playerMovement;
    LocalPlayerKillController localPlayerKillController;
    LocalPlayerController localPlayerController;

    int playersCount = 0;

    void Awake()
    {
        playerMovement = GetComponentInParent<LocalPlayerMovement>();
        localPlayerController = GetComponentInParent<LocalPlayerController>();
        localPlayerKillController = GetComponentInParent<LocalPlayerKillController>();
    }

    #region Triggers
    void OnTriggerEnter2D(Collider2D other)
    {
        //When the Imposter Enters a crewmember Trigger
        if (localPlayerController.isImposter)
        {
            if (other.gameObject.tag == "Crewmate")
            {
                localPlayerKillController.EnableKilling(other.GetComponentInParent<LocalPlayerController>());
                playersCount++;
            }
            if (other.gameObject.tag == "Mine")
            {
                Destroy(other.gameObject);
                localPlayerKillController.DisableKilling(other.GetComponentInParent<LocalPlayerController>());
                localPlayerController.Stun();
            }
        }

        if (other.gameObject.tag == "Coin")
        {
            other.GetComponent<Coin>().CollectLocalCoin();
            AudioController.Instance.PlaySFX(Clips.PickUp);
            localPlayerController.CollectCoin();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //When the Imposter Exit a crewmember Trigger
        if (localPlayerController.isImposter)
            if (other.gameObject.tag == "Crewmate")
            {
                playersCount--;

                if (playersCount < 0) playersCount = 0;

                if (playersCount == 0)
                {
                    localPlayerKillController.DisableKilling(other.GetComponentInParent<LocalPlayerController>());
                }
            }
    }
    #endregion 
}
