using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoOutPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 禁止移动
            collision.gameObject.GetComponent<PlayerControl>().allowMove = false;
            // 等待结束遮罩，载入回家的路场景
            Action nextOperation = new Action(GameManager.Instance.LoadNextScene);
            GameManager.Instance.WaitForScreenMaskFinished(nextOperation, false);
        }
    }
}
