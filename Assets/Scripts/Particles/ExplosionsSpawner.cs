using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExplosionsType
{
    small,
    mid,
    big
}

public class ExplosionsSpawner : MonoBehaviour
{
    public static ExplosionsSpawner Instance;

    public GameObject smallExplosion;
    public GameObject midExplosion;
    public GameObject bigExplosion;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    public void Spawn(ExplosionsType explosionType, Vector2 pos)
    {
        GameObject explosionObject;

        switch (explosionType)
        {
            case ExplosionsType.mid:
                explosionObject = midExplosion;
                break;
            case ExplosionsType.big:
                explosionObject = bigExplosion;
                break;
            default:
                explosionObject = smallExplosion;
                break;
        }

        Instantiate(explosionObject, pos, Quaternion.identity);

        if (AudioController.Instance != null)
            AudioController.Instance.PlaySFX(Clips.Explosion);
    }
}
