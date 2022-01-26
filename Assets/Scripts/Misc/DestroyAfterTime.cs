using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 2;
    void Start()
    {
        Destroy(this.gameObject, time);
    }
}
