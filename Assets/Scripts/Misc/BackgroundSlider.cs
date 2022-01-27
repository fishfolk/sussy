using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script to create the parallax movement for the background
public class BackgroundSlider : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] float step = 0.003f;

    Vector2 currnetSize;
    Vector2 startSize;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currnetSize = startSize = spriteRenderer.size;
    }

    void Update()
    {
        currnetSize.x += step;
        spriteRenderer.size = currnetSize;
    }

    public void Reset()
    {
        currnetSize = startSize;
        spriteRenderer.size = currnetSize;
    }
}
