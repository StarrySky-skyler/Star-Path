using System;
using Managers;
using Player;
using UnityEngine;

public class GoOutPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 禁止移动
            other.gameObject.GetComponent<PlayerControl>().AllowMove = false;
            // 等待结束遮罩，载入回家的路场景
            Action nextOperation = GameManager.Instance.LoadNextEvent;
            GameManager.Instance.WaitForScreenMaskFinished(nextOperation, false);
        }
    }
}
