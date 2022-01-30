using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class ConnectionHostManager : MonoBehaviour
{
    GameConnectionManager gameConnectionManager;
    GameConnectionUIManager gameConnectionUIManager;
    List<string> playersSessionIDs;

    Dictionary<string, string> playersStatesDictionary;

    int imposterCount = 0;
    int crewmateCount = 0;

    [SerializeField] int maxTasks = 10;
    int currentTasks = 0;

    void Awake()
    {
        gameConnectionManager = FindObjectOfType<GameConnectionManager>();
        gameConnectionUIManager = FindObjectOfType<GameConnectionUIManager>();
    }

    public void SetHost(List<string> playersSessionIDs)
    {
        this.playersSessionIDs = playersSessionIDs;
    }

    public async Task BuildPlayersRoles()
    {
        Debug.Log("BuildPlayersRoles");
        playersStatesDictionary = new Dictionary<string, string>();
        imposterCount = 0;
        crewmateCount = 0;
        currentTasks = 0;

        int imposterIndex = Random.Range(0, playersSessionIDs.Count);
        imposterCount++;

        string jsonState;

        for (int i = 0; i < playersSessionIDs.Count; i++)
        {
            if (gameConnectionManager.localUserSessionID == playersSessionIDs[i])
            {
                if (i == imposterIndex)
                {
                    FindObjectOfType<PlayerState>().SetPlayerState(PlayerState.imposterString);
                    playersStatesDictionary.Add(playersSessionIDs[i], PlayerState.imposterString);
                }
                else
                {
                    FindObjectOfType<PlayerState>().SetPlayerState(PlayerState.crewmateString);
                    playersStatesDictionary.Add(playersSessionIDs[i], PlayerState.crewmateString);
                    crewmateCount++;
                }
            }
            else
            {
                if (i == imposterIndex)
                {
                    jsonState = MatchDataJson.SetPlayerState(playersSessionIDs[i], PlayerState.imposterString);
                    playersStatesDictionary.Add(playersSessionIDs[i], PlayerState.imposterString);
                }
                else
                {
                    jsonState = MatchDataJson.SetPlayerState(playersSessionIDs[i], PlayerState.crewmateString);
                    playersStatesDictionary.Add(playersSessionIDs[i], PlayerState.crewmateString);
                    crewmateCount++;
                }

                await gameConnectionManager.SendMatchStateAsync(OpCodes.SetPlayerState, jsonState);
            }
        }
    }

    public async Task StartGame()
    {
        FindObjectOfType<InGameUIController>().SetTaskSlider(currentTasks, maxTasks);

        FindObjectOfType<PlayerState>().SetPlayerStateUI();

        FindObjectOfType<PlayersManager>().SpawnPlayers();

        string jsonState = MatchDataJson.SetStartGame(maxTasks);
        await gameConnectionManager.SendMatchStateAsync(OpCodes.StartGame, jsonState);

        jsonState = MatchDataJson.SetPlayerName(gameConnectionManager.localUserSessionID, gameConnectionUIManager.GetPlayerName());
        gameConnectionManager.SendMatchState(OpCodes.PlayerNameChange, jsonState);

        FindObjectOfType<PlayersManager>().AddPlayerName(gameConnectionManager.localUserSessionID, gameConnectionUIManager.GetPlayerName());
    }

    public async void CompleteTask()
    {
        currentTasks++;

        await CheckGameEnd();

        SpwanCoin();
    }
    public async Task KillTask(string killableID)
    {
        if (playersStatesDictionary[killableID] != PlayerState.dead)
        {
            if (gameConnectionManager.localUserSessionID == killableID)
                FindObjectOfType<PlayersManager>().KillLocalPlayer(killableID);
            else
                FindObjectOfType<PlayersManager>().KillPlayer(killableID);

            if (playersStatesDictionary[killableID] == PlayerState.crewmateString)
                crewmateCount--;
            else
                imposterCount--;

            playersStatesDictionary[killableID] = PlayerState.dead;

            string jsonState = MatchDataJson.SetUserID(killableID);
            await gameConnectionManager.SendMatchStateAsync(OpCodes.Dead, jsonState);

            await CheckGameEnd();
        }
    }

    public async void PlayerRemoved(string userSessionID)
    {
        if (playersStatesDictionary.ContainsKey(userSessionID))
        {
            if (playersStatesDictionary[userSessionID] == PlayerState.imposterString)
                imposterCount--;
            else
                crewmateCount--;

            playersStatesDictionary.Remove(userSessionID);
        }

        await CheckGameEnd();
    }

    public async Task CheckGameEnd()
    {
        if (imposterCount == 0 || currentTasks == maxTasks)
        {
            FindObjectOfType<InGameUIController>().ActivateCrewMateWonUI();
            gameConnectionManager.SendMatchState(OpCodes.CrewmateWon, "");

            await Task.Delay(2500);
            await gameConnectionManager.LeaveMatch();
        }

        if (crewmateCount == 0)
        {
            FindObjectOfType<InGameUIController>().ActivateImpostersWonUI();
            gameConnectionManager.SendMatchState(OpCodes.ImposterWon, "");

            await Task.Delay(2500);
            await gameConnectionManager.LeaveMatch();
        }
    }

    #region Coins

    CoinsSpawnerManager coinsSpawnerManager;
    List<int> coinsIDList;
    int currentCoinListIndex = 0;

    int CoinsToSpawn = 5;

    public void SpawnCoins()
    {
        coinsSpawnerManager = FindObjectOfType<CoinsSpawnerManager>();
        currentCoinListIndex = 0;
        coinsIDList = new List<int>();

        for (int i = 0; i < coinsSpawnerManager.spawnPoints.Count; i++)
        {
            coinsIDList.Add(i);
        }
        Randomizer.Randomize(coinsIDList);

        for (int i = 0; i < CoinsToSpawn; i++)
        {
            gameConnectionManager.SpawnCoin(coinsIDList[currentCoinListIndex]);
            currentCoinListIndex++;
        }
    }

    void SpwanCoin()
    {
        currentCoinListIndex++;

        if (currentCoinListIndex >= coinsIDList.Count)
            currentCoinListIndex = 0;

        gameConnectionManager.SpawnCoin(coinsIDList[currentCoinListIndex]);
    }

    #endregion

    #region Meeting System

    Dictionary<string, int> playersVotingList;
    int meetingPlayerCount;

    public void StartMeeting()
    {
        List<string> playersIDs = FindObjectOfType<PlayersManager>().GetPlayersID();
        playersVotingList = new Dictionary<string, int>();
        meetingPlayerCount = 0;

        foreach (var v in playersIDs)
        {
            if (playersStatesDictionary[v] != PlayerState.dead)
            {
                meetingPlayerCount++;
                playersVotingList.Add(v, 0);
            }
        }

        StartCoroutine(CountDown());
    }

    public void PlayerVoted(string id)
    {
        playersVotingList[id]++;

        meetingPlayerCount--;
        if (meetingPlayerCount == 0)
        {
            CalculateMeeting();
        }
    }

    void CalculateMeeting()
    {
        StopAllCoroutines();

        int maxVote = 0;
        foreach (var v in playersVotingList.Values)
        {
            if (v > maxVote)
                maxVote = v;
        }

        int voteTie = 0;
        foreach (var v in playersVotingList.Values)
        {
            if (v == maxVote)
            {
                voteTie++;
            }
        }
        if (voteTie == 1) //There is no tie
        {
            string KillID = "";
            foreach (var v in playersVotingList.Keys)
            {
                if (maxVote == playersVotingList[v])
                {
                    KillID = v;
                    break;
                }
            }
            FindObjectOfType<PlayersManager>().ActivatePlayers();
            currentKillID = KillID;
            gameConnectionManager.ShowMeetingResult(KillID);

            Invoke("KillTaskMeeting", 4);
        }
        else
            gameConnectionManager.ShowMeetingResult("");

        Invoke("EndMeeting", 5);
    }
    string currentKillID;
    void KillTaskMeeting()
    {
        KillTask(currentKillID);
    }

    void EndMeeting()
    {
        gameConnectionManager.EndMeeting();
    }

    IEnumerator CountDown()
    {
        WaitForSeconds delay = new WaitForSeconds(FindObjectOfType<MeetingsManager>().counter - 1);
        yield return delay;

        CalculateMeeting();
    }

    #endregion
}
