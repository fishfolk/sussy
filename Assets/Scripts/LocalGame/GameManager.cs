using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    CoinsSpawnerManager coinsSpawnerManager;
    LocalUIController localUIController;

    [SerializeField] int collectTime = 60;
    [SerializeField] int killingTime = 60;

    [SerializeField] TextMeshProUGUI timerText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        coinsSpawnerManager = FindObjectOfType<CoinsSpawnerManager>();
        localUIController = FindObjectOfType<LocalUIController>();
    }

    public void StartGame()
    {
        EventManager.ActivateEvent(EventTypes.StartGame);
        SpawnCoins();

        StartCoroutine(StartCollectingTimer());
    }

    public void EndCollect()
    {
        coinsSpawnerManager.RemoveAllCoins();

        EventManager.ActivateEvent(EventTypes.EndCollect);
    }

    public void StartKill()
    {
        Invoke("startKill", 3);
    }

    void startKill()
    {
        EventManager.ActivateEvent(EventTypes.StartKill);
        StartCoroutine(StartKillTimer());
    }

    public void EndGame(bool ImposterWon)
    {
        StopAllCoroutines();

        if (ImposterWon)
            localUIController.ImposterWon();
        else
            localUIController.CrewmateWon();

        EventManager.ActivateEvent(EventTypes.EndGame);

        Invoke("ResetGame", 3);
    }

    public void ResetGame()
    {
        FindObjectOfType<ScenesManager>().GoToLocalScene();
    }

    IEnumerator StartCollectingTimer()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        for (int i = collectTime; i >= 0; i--)
        {
            timerText.text = i.ToString();
            yield return delay;
        }

        EndCollect();
    }
    IEnumerator StartKillTimer()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        for (int i = killingTime; i >= 0; i--)
        {
            timerText.text = i.ToString();
            yield return delay;
        }

        EndGame(false);
    }

    public void SpawnCoins()
    {
        for (int i = 0; i < 5; i++)
            coinsSpawnerManager.SpawnCoin();
    }

    public void CollectCoin()
    {
        coinsSpawnerManager.SpawnCoin();
    }
}
