using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKillController : MonoBehaviour
{
    //The UI button with the Overlay color
    Button KillButton;
    Image KillButtonImage;
    Text KillButtonCoolDownText;

    float KillCoolDown = 5;
    bool canKill = true;
    string killableID;

    List<string> idKillList;

    void Awake()
    {
        GameObject killButton = FindObjectOfType<InGameUIController>().imposterKillButton;
        KillButton = killButton.GetComponent<Button>();
        KillButtonImage = killButton.GetComponent<Image>();
        KillButtonCoolDownText = killButton.GetComponentInChildren<Text>();
        KillButton.onClick.AddListener(() => Kill());

        idKillList = new List<string>();

        KillButtonCoolDownText.text = "";
        KillButtonImage.fillAmount = 1;
    }

    private void OnEnable()
    {
        idKillList = new List<string>();

        StopAllCoroutines();

        canKill = true;

        if (idKillList.Count > 0)
            KillButton.interactable = true;

        KillButtonCoolDownText.text = "";
        KillButtonImage.fillAmount = 1;
    }

    IEnumerator ResetKill()
    {
        //Reset the Variables
        canKill = false;
        KillButton.interactable = false;
        KillButtonCoolDownText.text = "" + KillCoolDown;
        KillButtonImage.fillAmount = 0;

        //Start the Timer
        float TimeLeft = KillCoolDown;
        while (TimeLeft != 0)
        {
            yield return new WaitForSeconds(1);
            TimeLeft--;

            //Change the Time Text and the Image Fill Amount
            KillButtonCoolDownText.text = "" + TimeLeft;
            KillButtonImage.fillAmount = 1 - TimeLeft / KillCoolDown;
        }

        if (idKillList.Count > 0)
            KillButton.interactable = true;

        //Set variables for the next kill
        KillButtonCoolDownText.text = "";
        KillButtonImage.fillAmount = 1;
        canKill = true;
    }

    //Called when the kill button is pressed
    public void Kill()
    {
        if (idKillList.Count > 0)
        {
            FindObjectOfType<GameConnectionManager>().KillTask(idKillList[0]);
            FindObjectOfType<PlayersManager>().KillPlayer(idKillList[0]);

            //Start the CoolDown
            StartCoroutine(ResetKill());

            idKillList.Remove(idKillList[0]);
        }
    }

    #region Triggers

    //Called when a player enter the player collider
    public void EnableKilling(string ID)
    {
        if (!idKillList.Contains(ID))
            idKillList.Add(ID);

        killableID = ID;
        //if the cooldown is 0
        if (canKill)
            KillButton.interactable = true;
    }

    //Called when a player exit the player collider
    public void DisableKilling(string ID)
    {
        if (idKillList.Contains(ID))
            idKillList.Remove(ID);

        KillButton.interactable = false;
    }

    #endregion
}
