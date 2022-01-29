using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerKillController : MonoBehaviour
{
    //The UI button with the Overlay color
    [SerializeField] public GameObject KillButton;
    Image KillButtonImage;

    float KillCoolDown = 5;
    bool canKill = true;
    LocalPlayerController killableID;
    List<LocalPlayerController> idKillList;

    void Awake()
    {
        KillButtonImage = KillButton.GetComponent<Image>();
        canKill = true;

        idKillList = new List<LocalPlayerController>();

        KillButtonImage.fillAmount = 1;
    }

    IEnumerator ResetKill()
    {
        //Reset the Variables
        canKill = false;
        KillButtonImage.fillAmount = 0;

        //Start the Timer
        float TimeLeft = KillCoolDown;
        while (TimeLeft != 0)
        {
            yield return new WaitForSeconds(1);
            TimeLeft--;

            //Change the Time Text and the Image Fill Amount
            KillButtonImage.fillAmount = 1 - TimeLeft / KillCoolDown;
        }

        if (idKillList.Count > 0)
            KillButton.GetComponent<Animator>().SetBool("Kill", true);

        //Set variables for the next kill
        KillButtonImage.fillAmount = 1;
        canKill = true;
    }

    //Called when the kill button is pressed
    public void Kill()
    {
        if (canKill)
        {
            if (idKillList.Count > 0)
            {
                killableID.Kill();

                //Start the CoolDown
                StartCoroutine(ResetKill());

                idKillList.Remove(idKillList[0]);

                LocalPlayersManager.Instance.KillPlayer();
            }

            AudioController.Instance.PlaySFX(Clips.Kill);
            KillButton.GetComponent<Animator>().SetBool("Kill", false);
        }
    }

    #region Triggers

    //Called when a player enter the player collider
    public void EnableKilling(LocalPlayerController ID)
    {
        if (!idKillList.Contains(ID))
            idKillList.Add(ID);

        killableID = ID;
        //if the cooldown is 0
        if (canKill)
        {
            KillButton.GetComponent<Animator>().SetBool("Kill", true);
        }
    }

    //Called when a player exit the player collider
    public void DisableKilling(LocalPlayerController ID)
    {
        if (idKillList.Contains(ID))
            idKillList.Remove(ID);

        KillButton.GetComponent<Animator>().SetBool("Kill", false);
    }

    #endregion
}
