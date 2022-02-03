using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayersManager : MonoBehaviour
{
    public static LocalPlayersManager Instance;
    [SerializeField] int playersNumber = 4;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        EventManager.StartGame += StartGame;
        EventManager.EndCollect += EndCollect;
        EventManager.EndGame += EndGame;
        EventManager.StartKill += StartKill;

    }

    void OnDestroy()
    {
        EventManager.StartGame -= StartGame;
        EventManager.EndCollect -= EndCollect;
        EventManager.EndGame -= EndGame;
        EventManager.StartKill -= StartKill;
    }

    public void StartGame()
    {
        foreach (var v in playersList)
            v.StartPlayer();
    }
    public void EndCollect()
    {
        foreach (var v in playersList)
            v.StopPlayer();

        CalculateWinner();
        GameManager.Instance.StartKill();
    }

    public void StartKill()
    {
        foreach (var v in playersList)
            v.StartPlayer();
    }

    public void EndGame()
    {
        foreach (var v in playersList)
            v.StopPlayer();
    }

    [SerializeField] List<LocalPlayerController> playersList;

    public void NamePlayer(int id, string name)
    {
        playersList[id].SetName(name);
        playersList[id].SetIsReady(true);

        CheckPlayersIfReady();
    }

    public void KillPlayer()
    {
        playersNumber--;

        if (playersNumber == 1)
            GameManager.Instance.EndGame(true);
    }

    public void CheckPlayersIfReady()
    {
        foreach (var v in playersList)
            if (!v.IsReady()) return;

        GameManager.Instance.StartGame();
    }

    public void CalculateWinner()
    {
        LocalPlayerController winner = null;
        int max = -1;
        foreach (var v in playersList)
        {
            if (v.GetCurrentWeapons() > max)
            {
                winner = v;
                max = v.GetCurrentWeapons();
            }
        }

        winner.SetIsImposter();
        foreach (var v in playersList)
        {
            if (v != winner)
                v.SetIsCrewMate();
        }
    }
}
