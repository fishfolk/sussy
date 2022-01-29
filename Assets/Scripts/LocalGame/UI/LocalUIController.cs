using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalUIController : MonoBehaviour
{
    [SerializeField] GameObject nameSelectionPanel;
    [SerializeField] GameObject weaponsPanel;
    [SerializeField] GameObject timerPanel;
    [SerializeField] GameObject statePanel;
    [SerializeField] GameObject imposterPanel;
    [SerializeField] GameObject ImposterWinPanel;
    [SerializeField] GameObject CrewmateWinPanel;


    void Awake()
    {
        EventManager.StartGame += StartGame;
        EventManager.EndCollect += EndCollect;
        EventManager.StartKill += StartKill;
        EventManager.EndGame += EndGame;
    }

    void OnDestroy()
    {
        EventManager.StartGame -= StartGame;
        EventManager.EndCollect -= EndCollect;
        EventManager.StartKill -= StartKill;
        EventManager.EndGame -= EndGame;

    }

    public void StartGame()
    {
        nameSelectionPanel.SetActive(false);
        timerPanel.SetActive(true);
        weaponsPanel.SetActive(true);
    }

    public void EndCollect()
    {
        timerPanel.SetActive(false);
        weaponsPanel.SetActive(false);

        statePanel.SetActive(true);
    }

    public void StartKill()
    {
        statePanel.SetActive(false);
        imposterPanel.SetActive(true);
        timerPanel.SetActive(true);
    }

    public void EndGame()
    {
        timerPanel.SetActive(false);
        weaponsPanel.SetActive(false);
        statePanel.SetActive(false);
        imposterPanel.SetActive(false);
    }

    public void ImposterWon()
    {
        ImposterWinPanel.SetActive(true);
    }
    public void CrewmateWon()
    {
        CrewmateWinPanel.SetActive(true);
    }
}
