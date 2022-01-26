using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] GameObject crewmatePanel;
    [SerializeField] GameObject imposterPanel;
    public GameObject imposterKillButton;

    [SerializeField] GameObject imposterWonPanel;
    [SerializeField] GameObject crewmateWonPanel;

    [SerializeField] Slider taskSliders;

    public void ResetUI()
    {
        taskSliders.gameObject.SetActive(false);
        crewmatePanel.SetActive(false);
        imposterPanel.SetActive(false);
        imposterWonPanel.SetActive(false);
        crewmateWonPanel.SetActive(false);
    }
    public void ProgressTaskSlider()
    {
        taskSliders.value = taskSliders.value + 1;
    }
    public void SetTaskSlider(int value, int maxValue)
    {
        taskSliders.gameObject.SetActive(true);

        taskSliders.maxValue = maxValue;
        taskSliders.value = value;
    }
    public void ActivateImposterPanel()
    {
        imposterWonPanel.SetActive(false);
        crewmateWonPanel.SetActive(false);

        crewmatePanel.SetActive(false);
        imposterPanel.SetActive(true);
    }
    public void ActivateCrewmatePanel()
    {
        imposterWonPanel.SetActive(false);
        crewmateWonPanel.SetActive(false);

        imposterPanel.SetActive(false);
        crewmatePanel.SetActive(true);
    }

    public void ActivateDeadPanel()
    {
        imposterPanel.SetActive(false);
        crewmatePanel.SetActive(false);
    }

    public void ActivateImpostersWonUI()
    {
        imposterPanel.SetActive(false);
        crewmatePanel.SetActive(false);
        taskSliders.gameObject.SetActive(false);

        imposterWonPanel.SetActive(true);
    }

    public void ActivateCrewMateWonUI()
    {
        imposterPanel.SetActive(false);
        crewmatePanel.SetActive(false);
        taskSliders.gameObject.SetActive(false);

        crewmateWonPanel.SetActive(true);
    }
}
