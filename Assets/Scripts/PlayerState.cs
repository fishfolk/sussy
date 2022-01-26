using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will hold the current Client State

public class PlayerState : MonoBehaviour
{
    public static string crewmateString = "Crewmate";
    public static string imposterString = "Imposter";
    public static string dead = "dead";

    [SerializeField] public bool isImposter = false;
    [SerializeField] public bool isCrewMate = false;
    [SerializeField] public bool isDead = false;

    [SerializeField] GameObject crewmatePlayerStateUI;
    [SerializeField] GameObject imposterPlayerStateUI;

    public void SetPlayerState(string playerStateString)
    {
        if (playerStateString == crewmateString)
            SetAsCrewmate();
        else
            SetAsImposter();
    }

    public void SetAsCrewmate()
    {
        Debug.Log("SetAsCrewmate");
        isImposter = false;
        isCrewMate = true;
        isDead = false;

        crewmatePlayerStateUI.SetActive(true);
        Invoke("ClearPlayerStateUI", 2);
    }
    public void SetAsImposter()
    {
        Debug.Log("SetAsImposter");

        isImposter = true;
        isCrewMate = false;
        isDead = false;

        imposterPlayerStateUI.SetActive(true);
        Invoke("ClearPlayerStateUI", 2);
    }

    public void Dead()
    {
        Debug.Log("Dead");

        isImposter = false;
        isCrewMate = false;
        isDead = true;

        SetPlayerStateUI();
    }

    public void SetPlayerStateUI()
    {
        if (isImposter)
            FindObjectOfType<InGameUIController>().ActivateImposterPanel();

        if (isCrewMate)
            FindObjectOfType<InGameUIController>().ActivateCrewmatePanel();

        if (isDead)
            FindObjectOfType<InGameUIController>().ActivateDeadPanel();
    }

    void ClearPlayerStateUI()
    {
        imposterPlayerStateUI.SetActive(false);
        crewmatePlayerStateUI.SetActive(false);
    }

}
