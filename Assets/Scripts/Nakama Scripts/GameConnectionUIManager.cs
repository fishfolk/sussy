using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameConnectionUIManager : MonoBehaviour
{
    [SerializeField] GameObject serverConnectingText;
    [SerializeField] GameObject serverConnectedText;
    [SerializeField] GameObject findMatchPanel;
    [SerializeField] GameObject CancelMatchButton;
    [SerializeField] GameObject findingMatchText;
    [SerializeField] GameObject isHostGameObject;
    [SerializeField] TextMeshProUGUI playerCountText;
    [SerializeField] TMP_InputField nameInputField;

    [Header("In Game Panel")]
    [SerializeField] GameObject inGameUIPanel;


    GameConnectionManager gameConnectionManager;

    void Awake()
    {
        gameConnectionManager = FindObjectOfType<GameConnectionManager>();

        serverConnectingText.SetActive(true);

        serverConnectedText.SetActive(false);
        findMatchPanel.SetActive(false);
        CancelMatchButton.SetActive(false);
        playerCountText.gameObject.SetActive(false);
        findingMatchText.SetActive(false);
        inGameUIPanel.SetActive(false);
        isHostGameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
    }

    public void ResetUI()
    {
        serverConnectedText.SetActive(true);
        findMatchPanel.SetActive(true);
        nameInputField.gameObject.SetActive(true);

        CancelMatchButton.SetActive(false);
        playerCountText.gameObject.SetActive(false);
        findingMatchText.SetActive(false);
        inGameUIPanel.SetActive(false);
        isHostGameObject.SetActive(false);

        FindObjectOfType<MeetingsManager>().ResetMeeting();
    }

    #region Buttons
    public async void FindMatch()
    {
        ActivateFindingMatchUI();

        await gameConnectionManager.FindMatch();
    }
    public async void LeaveMatch()
    {
        await gameConnectionManager.LeaveMatch();
    }
    public async void CancelMatchMaking()
    {
        await gameConnectionManager.CanelMatchMacking();

        ActivateCancelMatchMaking();
    }
    #endregion

    #region UI Manipulations
    public void ActivateServerConnected()
    {
        serverConnectingText.SetActive(false);
        serverConnectedText.SetActive(true);
        isHostGameObject.SetActive(false);

        findMatchPanel.SetActive(true);
        nameInputField.gameObject.SetActive(true);
    }

    public void ActivateFindingMatchUI()
    {
        findMatchPanel.SetActive(false);
        nameInputField.gameObject.SetActive(false);

        isHostGameObject.SetActive(false);

        findingMatchText.SetActive(true);
        CancelMatchButton.SetActive(true);
    }

    void ActivateCancelMatchMaking()
    {
        findMatchPanel.SetActive(true);
        nameInputField.gameObject.SetActive(true);

        isHostGameObject.SetActive(false);
        findingMatchText.SetActive(false);
        CancelMatchButton.SetActive(false);
        playerCountText.gameObject.SetActive(false);
    }

    public void ActivateMatchFound()
    {
        CancelMatchButton.SetActive(false);
        findingMatchText.SetActive(false);

        inGameUIPanel.SetActive(true);
    }
    public void SetPlayerCountText(int minPlayers, int maxPlayers)
    {
        //Deactivated for clients testing
        //playerCountText.gameObject.SetActive(true);
        playerCountText.text = minPlayers + "/" + maxPlayers;
    }

    public void ActivateIsHost(bool isHost)
    {
        isHostGameObject.SetActive(isHost);
    }

    public string GetPlayerName()
    {
        return nameInputField.text;
    }
    #endregion
}
