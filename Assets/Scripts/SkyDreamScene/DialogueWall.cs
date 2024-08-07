using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueWall : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"触发剧情碰撞，当前物体名为{gameObject.name}");
            GameManager.Instance.HandleKeyZ();
            Destroy(gameObject);
        }
    }
}
