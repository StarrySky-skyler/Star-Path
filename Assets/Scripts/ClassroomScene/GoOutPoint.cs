using System;
using Managers;
using Player;
using UnityEngine;

namespace ClassroomScene
{
    public class GoOutPoint : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // 禁止移动
                collision.gameObject.GetComponent<PlayerControl>().AllowMove = false;
                // 等待结束遮罩，载入回家的路场景
                Action nextOperation = GameManager.Instance.LoadNextEvent;
                GameManager.Instance.WaitForScreenMaskFinished(nextOperation, false);
            }
        }
    }
}