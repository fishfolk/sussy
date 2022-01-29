using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteSelection : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsList;

    private void OnEnable()
    {
        foreach (var v in objectsList)
            v.SetActive(false);

        objectsList[Random.Range(0, objectsList.Count)].SetActive(true);
    }
}
