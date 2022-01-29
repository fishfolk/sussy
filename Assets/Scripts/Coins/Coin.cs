using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int ID;

    private void Awake()
    {
        ExplosionsSpawner.Instance.Spawn(ExplosionsType.big, transform.position);
    }

    public void CollectCoin()
    {
        FindObjectOfType<GameConnectionManager>().CompleteTask(ID);

        RemoveCoin();
    }

    public void CollectLocalCoin()
    {
        GameManager.Instance.CollectCoin();
        RemoveCoin();
    }

    public void SetCoinSprite(Sprite sprite)
    {
        foreach (var v in GetComponentsInChildren<SpriteRenderer>())
            v.sprite = sprite;
    }

    public void RemoveCoin()
    {
        ExplosionsSpawner.Instance.Spawn(ExplosionsType.small, transform.position);
        Destroy(this.gameObject);
    }
}
