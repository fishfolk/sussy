using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeetingsManager : MonoBehaviour
{
    [SerializeField] GameObject meetingsPanel;
    [SerializeField] GameObject meetingsPanelLeft;
    [SerializeField] GameObject meetingsPanelRight;
    [SerializeField] TextMeshProUGUI meetingCounterText;

    [SerializeField] GameObject meetingButton;
    [SerializeField] GameObject callMeetingButton;

    [SerializeField] GameObject meetingResultPanel;
    [SerializeField] TextMeshProUGUI meetingResultPanelName;


    [SerializeField] public int counter = 60;

    PlayersManager playersManager;
    GameConnectionManager gameConnectionManager;

    List<string> playersIDs;

    private void Awake()
    {
        playersManager = FindObjectOfType<PlayersManager>();
        gameConnectionManager = FindObjectOfType<GameConnectionManager>();
    }

    public void StartMeeting()
    {
        callMeetingButton.GetComponent<Button>().interactable = false;

        playersIDs = playersManager.GetPlayersID();
        bool left = true;
        foreach (var v in playersIDs)
        {
            MeetingButton button;
            if (left)
                button = Instantiate(meetingButton, meetingsPanelLeft.transform).GetComponent<MeetingButton>();
            else
                button = Instantiate(meetingButton, meetingsPanelRight.transform).GetComponent<MeetingButton>();

            button.SetButton(v, playersManager.GetPlayerName(v));

            if (gameConnectionManager.localUserSessionID == v || FindObjectOfType<PlayerState>().isDead)
                button.GetComponent<Button>().interactable = false;

            left = !left;
        }
        meetingsPanel.SetActive(true);
        playersManager.DeactivatePlayers();

        StartCoroutine(CountDown());
    }

    public void EndMeeting()
    {
        callMeetingButton.GetComponent<Button>().interactable = true;
        StopAllCoroutines();

        foreach (var v in FindObjectsOfType<MeetingButton>())
        {
            Destroy(v.gameObject);
        }
        playersManager.ActivatePlayers();
        meetingsPanel.SetActive(false);
        meetingResultPanel.SetActive(false);
    }

    IEnumerator CountDown()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        for (int i = counter; i >= 0; i--)
        {
            meetingCounterText.text = i.ToString();
            yield return delay;
        }
    }

    public void Vote()
    {
        foreach (var v in FindObjectsOfType<MeetingButton>())
        {
            v.GetComponent<Button>().interactable = false;
        }
    }

    public void VoteReceived(string ID)
    {
        foreach (var v in FindObjectsOfType<MeetingButton>())
        {
            if (v.UserID == ID)
                v.VoteReceived();
        }
    }

    public void ShowCallMeetingButton()
    {
        callMeetingButton.gameObject.SetActive(true);
        callMeetingButton.GetComponent<Button>().interactable = true;
    }
    public void HideCallMeetingButton()
    {
        callMeetingButton.gameObject.SetActive(false);
        callMeetingButton.GetComponent<Button>().interactable = false;
    }
    public void ResetMeeting()
    {
        StopAllCoroutines();

        foreach (var v in FindObjectsOfType<MeetingButton>())
        {
            Destroy(v.gameObject);
        }

        meetingsPanel.SetActive(false);
    }
    public void ShowMeetingResult(string KillID)
    {
        meetingsPanel.SetActive(false);
        meetingResultPanel.SetActive(true);
        meetingResultPanelName.text = FindObjectOfType<PlayersManager>().GetPlayerName(KillID);
    }
}
