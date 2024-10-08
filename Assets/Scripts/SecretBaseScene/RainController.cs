// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2024/10/05 12:10
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using UnityEngine;

namespace SecretBaseScene
{
    public class RainController : MonoBehaviour
    {
        private GameObject _player;
        // 相对玩家位置的偏移量
        private Vector3 _offset;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _offset = transform.position - _player.transform.position;
        }

        private void Update()
        {
            transform.position = _player.transform.position + _offset;
        }
    }
}
