using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawnerManager : MonoBehaviour
{
    [SerializeField] List<GameObject> BubblesList;

    private void Awake()
    {
        StartCoroutine(spawnBubbles());
    }
    IEnumerator spawnBubbles()
    {
        WaitForSeconds delay = new WaitForSeconds(5);

        while (true)
        {
            respawnBubbles();
            yield return delay;
        }
    }

    void respawnBubbles()
    {
        foreach (var v in BubblesList)
        {
            v.transform.localPosition = Random.insideUnitCircle * 25;
        }
    }
}
