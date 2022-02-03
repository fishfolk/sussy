using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerInventory : MonoBehaviour
{
    [SerializeField] public GameObject MineButton;
    [SerializeField] GameObject MineObject;
    Image MineButtonImage;

    bool canPlantMine = false;

    [SerializeField] float mineCoolDown = 10;

    void Awake()
    {
        MineButtonImage = MineButton.GetComponent<Image>();
        canPlantMine = false;
        MineButtonImage.fillAmount = 1;
    }

    public void StartCrewMate()
    {
        MineButton.SetActive(true);
        canPlantMine = true;

        MineButton.GetComponent<Animator>().SetBool("Kill", true);
    }

    IEnumerator ResetMine()
    {
        //Reset the Variables
        canPlantMine = false;

        MineButtonImage.fillAmount = 0;

        //Start the Timer
        float TimeLeft = mineCoolDown;
        while (TimeLeft != 0)
        {
            yield return new WaitForSeconds(1);
            TimeLeft--;

            //Change the Time Text and the Image Fill Amount
            MineButtonImage.fillAmount = 1 - TimeLeft / mineCoolDown;
        }

        MineButton.GetComponent<Animator>().SetBool("Kill", true);

        //Set variables for the next kill
        MineButtonImage.fillAmount = 1;
        canPlantMine = true;
    }

    //Called when the kill button is pressed
    public void PlantMine()
    {
        if (canPlantMine)
        {
            Instantiate(MineObject, this.transform.position, Quaternion.identity);

            //Start the CoolDown
            StartCoroutine(ResetMine());

            AudioController.Instance.PlaySFX(Clips.PickUp);
            MineButton.GetComponent<Animator>().SetBool("Kill", false);
        }
    }
}



