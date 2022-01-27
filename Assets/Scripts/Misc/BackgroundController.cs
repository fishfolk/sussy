using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public List<GameObject> backgrounds;

    private void OnEnable()
    {
        backgrounds[Random.Range(0, backgrounds.Count)].SetActive(true);
    }

}
