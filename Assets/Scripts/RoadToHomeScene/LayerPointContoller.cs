using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerPointContoller : MonoBehaviour
{
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var spriteRenderer = _player.GetComponent<SpriteRenderer>();
        switch (gameObject.tag)
        {
            case "LayerPoint1":
                spriteRenderer.sortingLayerName = "Layer 1";
                break;
            case "LayerPoint2":
                spriteRenderer.sortingLayerName = "Layer 2";
                break;
            case "LayerPoint3":
                spriteRenderer.sortingLayerName = "Layer 3";
                break;
        }
    }
}
