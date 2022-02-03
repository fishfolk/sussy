using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatePanelController : MonoBehaviour
{
    [SerializeField] Image panelImage;
    [SerializeField] TextMeshProUGUI panelText;
    [SerializeField] Color imposterColor;
    [SerializeField] Color crewmateColor;

    public void SetImposter()
    {
        panelImage.color = imposterColor;
        panelText.text = "Pirate";
    }

    public void SetCrewmate()
    {
        panelImage.color = crewmateColor;
        panelText.text = "Crewmate";
    }
}
