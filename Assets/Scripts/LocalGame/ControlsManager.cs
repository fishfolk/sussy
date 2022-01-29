using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we can list all the controls that the game would contain.
public enum ControlKeys
{
    UpKey,
    DownKey,
    LeftKey,
    RightKey,
    ActionKey
};

//This Class will control the input for all Players
public class ControlsManager : MonoBehaviour
{
    //The Player Controls Class for each Player
    [Serializable]
    private class PlayerControl
    {
        public List<keyCode> keyCodes;

        //Return the correct KeyCode is is slow when you have more than 5 Controls use Switch instead
        public KeyCode GetKeyCode(ControlKeys controlKey)
        {
            foreach (keyCode k in keyCodes)
                if (k.controlKey == controlKey)
                    return k.key;

            return KeyCode.None;
        }
    }

    [Serializable]
    private class keyCode
    {
        public ControlKeys controlKey;
        public KeyCode key;
    }

    //The Global List for Player Controls
    [SerializeField]
    List<PlayerControl> playerControls;

    //This Class will be used by the Player to request KeyCode.
    public KeyCode GetKey(int PlayerID, ControlKeys controlKeys)
    {
        return playerControls[PlayerID].GetKeyCode(controlKeys);
    }
}