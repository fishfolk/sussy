using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCountUIController : MonoBehaviour
{
    [SerializeField] List<Button> countButtons;

    public void ActivateAllButtons()
    {
        foreach (var v in countButtons)
            v.interactable = true;
    }
}
