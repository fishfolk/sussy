using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingButton : MonoBehaviour
{
    [SerializeField] Text buttonNameText;
    [SerializeField] Text voteCounterText;

    public string UserID;

    int currentVotes = 0;

    public void SetButton(string userID, string userName)
    {
        UserID = userID;
        buttonNameText.text = userName;
        voteCounterText.text = currentVotes.ToString();
    }

    public void VoteReceived()
    {
        currentVotes++;
        voteCounterText.text = currentVotes.ToString();
    }

    public void Vote()
    {
        VoteReceived();
        FindObjectOfType<MeetingsManager>().Vote();
        FindObjectOfType<GameConnectionManager>().VotePlayer(UserID);
    }
}
