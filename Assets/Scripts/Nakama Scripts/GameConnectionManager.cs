using Nakama;
using Nakama.TinyJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//This Class will connect with the Nakama Connections Class
//it'll be also responsable to getting all messages and proccess them

public class GameConnectionManager : MonoBehaviour
{
    NakamaConnection nakamaConnection;
    GameConnectionUIManager gameConnectionUIManager;
    ConnectionHostManager connectionHostManager;
    PlayersManager playersManager;
    CoinsSpawnerManager coinsSpawnerManager;
    MeetingsManager meetingsManager;

    public List<string> playersSessionIDs;

    private IUserPresence localUser;
    [SerializeField] public string localUserSessionID;
    private IMatch currentMatch;

    public int minPlayers = 3;
    int maxPlayers;
    int playerCount = 0;

    bool isHost = false;
    bool isHostSet = false;

    #region Connecting
    async void Awake()
    {
        nakamaConnection = FindObjectOfType<NakamaConnection>();
        gameConnectionUIManager = FindObjectOfType<GameConnectionUIManager>();
        connectionHostManager = FindObjectOfType<ConnectionHostManager>();
        playersManager = FindObjectOfType<PlayersManager>();
        coinsSpawnerManager = FindObjectOfType<CoinsSpawnerManager>();
        meetingsManager = FindObjectOfType<MeetingsManager>();
        playersSessionIDs = new List<string>();

        maxPlayers = minPlayers;

        await Connect();
    }

    //Connect to Nakama Server
    public async Task Connect()
    {
        await nakamaConnection.Connect();

        RegisterEvents();

        gameConnectionUIManager.ActivateServerConnected();
    }

    //Register Nakama Server Events to functions
    void RegisterEvents()
    {
        UnityMainThreadDispatcher mainThread = UnityMainThreadDispatcher.Instance();
        nakamaConnection.GetSocket().ReceivedMatchmakerMatched += T => mainThread.Enqueue(() => OnReceivedMatchmakerMatched(T));
        nakamaConnection.GetSocket().ReceivedMatchPresence += T => mainThread.Enqueue(() => OnReceivedMatchPresence(T));
        nakamaConnection.GetSocket().ReceivedMatchState += m => mainThread.Enqueue(async () => await OnReceivedMatchState(m));
    }
    public async Task FindMatch()
    {
        await nakamaConnection.FindMatch(minPlayers, maxPlayers);
    }
    public async Task CanelMatchMacking()
    {
        await nakamaConnection.CancelMatchmaking();
    }
    public async Task LeaveMatch()
    {
        //Reset Client Classes and values
        gameConnectionUIManager.ResetUI();

        FindObjectOfType<InGameUIController>().ResetUI();
        FindObjectOfType<PlayersManager>().KillPlayers();

        playerCount = 0;
        playersSessionIDs = new List<string>();

        isHost = false;
        isHostSet = false;

        coinsSpawnerManager.RemoveAllCoins();

        //Send message that the player left
        string jsonState = MatchDataJson.SetUserID(localUserSessionID);
        await SendMatchStateAsync(OpCodes.PlayerLeft, jsonState);

        await nakamaConnection.LeaveMatch();
    }
    #endregion


    #region Events
    /// <summary>
    /// Called when a MatchmakerMatched event is received from the Nakama server.
    /// </summary>
    /// <param name="matched">The MatchmakerMatched data.</param>
    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        // Cache a reference to the local user.
        localUser = matched.Self.Presence;
        localUserSessionID = localUser.SessionId;

        IMatch match = await nakamaConnection.GetSocket().JoinMatchAsync(matched);

        foreach (IUserPresence user in match.Presences)
        {
            if (!playersSessionIDs.Contains(user.SessionId))
            {
                playersSessionIDs.Add(user.SessionId);
                playerCount++;
            }
        }

        // Join the match.
        gameConnectionUIManager.ActivateMatchFound();
        gameConnectionUIManager.SetPlayerCountText(playersSessionIDs.Count, maxPlayers);

        // Cache a reference to the current match.
        currentMatch = match;
        nakamaConnection.SetCurrentMatch(currentMatch);

        if (minPlayers == playerCount)
            await StartHost();

    }

    /// <summary>
    /// Called when a player/s joins or leaves the match.
    /// </summary>
    /// <param name="matchPresenceEvent">The MatchPresenceEvent data.</param>
    private async void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
    {
        // For each new user that joins, spawn a player for them.
        foreach (IUserPresence user in matchPresenceEvent.Joins)
        {
            if (!playersSessionIDs.Contains(user.SessionId))
            {
                playersSessionIDs.Add(user.SessionId);
                playerCount++;
            }
        }

        // For each player that leaves, despawn their player.
        foreach (var user in matchPresenceEvent.Leaves)
        {
            if (playersSessionIDs.Contains(user.SessionId))
            {
                playersSessionIDs.Remove(user.SessionId);
                playerCount--;
            }
        }

        gameConnectionUIManager.SetPlayerCountText(playersSessionIDs.Count, maxPlayers);
    }
    /// <summary>
    /// Called when new match state is received. aka messages through the server
    /// </summary>
    /// <param name="matchState">The MatchState data.</param>
    private async Task OnReceivedMatchState(IMatchState matchState)
    {
        string senderSessionId = matchState.UserPresence.SessionId;

        var state = matchState.State.Length > 0 ? System.Text.Encoding.UTF8.GetString(matchState.State).FromJson<Dictionary<string, string>>() : null;

        // Decide what to do based on the Operation Code as defined in OpCodes.
        switch (matchState.OpCode)
        {
            //Message Received: what is the state of this client Player Sent by Host
            case OpCodes.SetPlayerState:
                if (state["userSessionID"] == localUserSessionID) //is directed to this Player
                {
                    FindObjectOfType<PlayerState>().SetPlayerState(state["playerState"]);
                }
                break;

            //Message Received: sent by other players to set the Host on the client
            case OpCodes.StartHost:
                if (state["userSessionID"] == localUserSessionID) //is directed to this Player
                {
                    await StartHost();
                }
                break;

            //Message Received: Sent by Host to start Game
            case OpCodes.StartGame:
                FindObjectOfType<InGameUIController>().SetTaskSlider(0, int.Parse(state["maxTasks"]));
                FindObjectOfType<PlayerState>().SetPlayerStateUI();

                playersSessionIDs.Sort();

                playersManager.SpawnPlayers();
                playersManager.AddPlayerName(localUserSessionID, gameConnectionUIManager.GetPlayerName());

                string jsonState = MatchDataJson.SetPlayerName(localUserSessionID, gameConnectionUIManager.GetPlayerName());
                SendMatchState(OpCodes.PlayerNameChange, jsonState);
                break;

            //Message Received: Sent by Host that the Imposter won the game
            case OpCodes.ImposterWon:
                FindObjectOfType<InGameUIController>().ActivateImpostersWonUI();
                await Task.Delay(2500);
                await LeaveMatch();
                break;

            //Message Received: Sent by Host that the Crewmates won the game
            case OpCodes.CrewmateWon:
                FindObjectOfType<InGameUIController>().ActivateCrewMateWonUI();
                await Task.Delay(2500);
                await LeaveMatch();
                break;

            //Message Received: Sent by Crewmate Client that he completed a task
            case OpCodes.CompleteTask:
                FindObjectOfType<InGameUIController>().ProgressTaskSlider();
                coinsSpawnerManager.RemoveCoin(int.Parse(state["CoinID"]));

                if (isHost) //Let the Host Keep Score
                    connectionHostManager.CompleteTask();

                break;

            //Message Received: Sent by Host to Spawn a new Coin on Client
            case OpCodes.SpawnCoin:
                coinsSpawnerManager.SpawnCoin(int.Parse(state["CoinID"]));
                break;

            //Message Received: Sent by player Client that he died
            case OpCodes.Dead:
                playersManager.ActivatePlayers();

                if (state["userSessionID"] == localUserSessionID) //is directed to this Player
                    playersManager.KillLocalPlayer(localUserSessionID);
                else
                    playersManager.KillPlayer(state);

                break;

            //Message Received: Sent by Imposter Client that he eleminated a crewmate and received by the host
            case OpCodes.KillTask: //Not Final
                if (isHost)
                    await connectionHostManager.KillTask(state["userSessionID"]);
                break;

            //Message Received: Sent by Player Client with position information to move the server player on other clients
            case OpCodes.PlayerChange:
                playersManager.MovePlayer(state);
                break;

            //Message Received: Sent by Player Client when leaving the game
            case OpCodes.PlayerLeft:
                playersManager.PlayerLeft(state);
                break;

            //Message Received: Sent by Player Client with Input information to move the server player on other clients
            case OpCodes.Input:
                playersManager.SetInputPlayer(state);
                break;

            //Message Received: Sent by Player Client with Player Name to set his name on all cleints
            case OpCodes.PlayerNameChange:
                playersManager.AddPlayerName(state);
                break;

            //Message Received: Sent by crewmate Client to start a meeting
            case OpCodes.StartMeeting:
                if (isHost)
                    connectionHostManager.StartMeeting();

                meetingsManager.StartMeeting();
                break;

            //Message Received: Sent by player Client to cast a vote
            case OpCodes.VotePlayer:
                if (isHost)
                    connectionHostManager.PlayerVoted(state["userSessionID"]);

                VoteRecived(state["userSessionID"]);
                break;

            //Message Received: Sent by Host to end meeting
            case OpCodes.EndMeeting:
                meetingsManager.EndMeeting();
                break;
        }
    }

    //when we want to make sure the message is received
    public async Task SendMatchStateAsync(long opCode, string state)
    {
        await nakamaConnection.GetSocket().SendMatchStateAsync(currentMatch.Id, opCode, state);
    }

    public void SendMatchState(long opCode, string state)
    {
        nakamaConnection.GetSocket().SendMatchStateAsync(currentMatch.Id, opCode, state);
    }
    #endregion

    #region Host Function

    //Called when the match is completed to set the Host
    async Task StartHost()
    {
        SetHost();

        if (isHost && !isHostSet)
        {
            isHostSet = true;
            await BuildPlayersRoles();
            await connectionHostManager.StartGame();
            connectionHostManager.SpawnCoins();
        }

        if (!isHost)
        {
            string jsonState = MatchDataJson.SetUserID(playersSessionIDs[0]);
            SendMatchState(OpCodes.StartHost, jsonState);
        }
    }

    //sort all ID and get the Host ID
    void SetHost()
    {
        playersSessionIDs.Sort();

        isHost = localUserSessionID == playersSessionIDs[0];

        connectionHostManager.SetHost(playersSessionIDs);

        gameConnectionUIManager.ActivateIsHost(isHost);
    }
    async Task BuildPlayersRoles()
    {
        await connectionHostManager.BuildPlayersRoles();
    }
    #endregion


    #region Game Functions

    //Called when a coin is Collected
    public void CompleteTask(int ID)
    {
        if (isHost)
            connectionHostManager.CompleteTask();

        FindObjectOfType<InGameUIController>().ProgressTaskSlider();
        string jsonState = MatchDataJson.SetCoinID(ID);
        SendMatchState(OpCodes.CompleteTask, jsonState);
    }

    //Called to spawn a coin
    public void SpawnCoin(int ID)
    {
        coinsSpawnerManager.SpawnCoin(ID);

        string jsonState = MatchDataJson.SetCoinID(ID);
        SendMatchStateAsync(OpCodes.SpawnCoin, jsonState);
    }

    //Called to elimanate a crewmember
    public async void KillTask(string id)
    {
        if (isHost)
            await connectionHostManager.KillTask(id);
        else
        {
            string jsonState = MatchDataJson.SetUserID(id);
            await SendMatchStateAsync(OpCodes.KillTask, jsonState);
        }
    }

    #endregion

    #region Voting System
    public void StartMeeting()
    {
        if (isHost)
            connectionHostManager.StartMeeting();

        SendMatchState(OpCodes.StartMeeting, "");
    }
    public void VotePlayer(string ID)
    {
        if (isHost)
            connectionHostManager.PlayerVoted(ID);

        if (ID == localUserSessionID)
            meetingsManager.VoteReceived(ID);

        string jsonState = MatchDataJson.SetUserID(ID);
        SendMatchState(OpCodes.VotePlayer, jsonState);
    }

    public void VoteRecived(string ID)
    {
        meetingsManager.VoteReceived(ID);
    }

    public void EndMeeting()
    {
        meetingsManager.EndMeeting();
        SendMatchState(OpCodes.EndMeeting, "");
    }

    #endregion
}