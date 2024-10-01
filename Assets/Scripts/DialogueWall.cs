using Managers;
using UnityEngine;

public class DialogueWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"触发剧情碰撞，当前物体名为{gameObject.name}");
            GameManager.Instance.LoadNextEvent();
            Destroy(gameObject);
        }
    }
}