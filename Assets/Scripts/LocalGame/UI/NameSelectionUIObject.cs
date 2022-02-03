using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameSelectionUIObject : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] int objectID;

    private void OnEnable()
    {
        inputField.text = "Player " + (objectID + 1) + "...";
    }

    public void SelectName()
    {
        LocalPlayersManager.Instance.NamePlayer(objectID, inputField.text);
    }
}
