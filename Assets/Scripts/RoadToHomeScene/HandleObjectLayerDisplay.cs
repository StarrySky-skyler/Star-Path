using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObjectLayerDisplay : MonoBehaviour
{
    private GameObject _player;
    private SpriteRenderer _sprite;

    private int _sortingOrder;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform.Find("ColliderPoint").gameObject;
        _sprite = GetComponent<SpriteRenderer>();
        _sortingOrder = _sprite.sortingOrder;
    }

    private void Update()
    {
        // 玩家在物体下方
        if (transform.position.y > _player.transform.position.y)
        {
            _sprite.sortingOrder = _sortingOrder - 1;
        }
        // 玩家在物体上方
        else
        {
            _sprite.sortingOrder = _sortingOrder;
        }
    }
}
