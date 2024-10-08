using System;
using Managers;
using UnityEngine;
using Player;

public class DialogueWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"触发剧情碰撞，当前物体名为{gameObject.name}");
            GameManager.Instance.LoadNextEvent();
            Destroy(gameObject);
        }
    }
}
