using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int ID;

    public void CollectCoin()
    {
        FindObjectOfType<GameConnectionManager>().CompleteTask(ID);

        RemoveCoin();
    }

    public void RemoveCoin()
    {
        Destroy(this.gameObject);
    }
}
