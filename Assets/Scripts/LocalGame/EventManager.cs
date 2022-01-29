using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypes
{
    None,

    StartGame,
    EndCollect,
    StartKill,
    EndGame
};

public class EventManager : MonoBehaviour
{
    public delegate void Event();
    public static event Event
    StartGame,
    EndGame,
    EndCollect,
    StartKill
    ;

    // used to make calling events easier from anywhere
    public static void ActivateEvent(EventTypes x)
    {
        switch (x)
        {
            case EventTypes.StartGame: StartGame?.Invoke(); break;
            case EventTypes.EndGame: EndGame?.Invoke(); break;
            case EventTypes.EndCollect: EndCollect?.Invoke(); break;
            case EventTypes.StartKill: StartKill?.Invoke(); break;
        }
    }
}