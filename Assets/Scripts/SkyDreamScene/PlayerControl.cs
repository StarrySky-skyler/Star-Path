using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2;

    // 是否移动状态
    private bool isMove;
    // 移动速度
    private float moveSpeed;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2 = GetComponent<Rigidbody2D>();
        isMove = false;
        moveSpeed = 5.5f;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMove();
    }

    /// <summary>
    /// 处理移动
    /// </summary>
    private void HandleMove()
    {
        float horizontalValue = Input.GetAxisRaw("Horizontal");
        float verticalValue = Input.GetAxisRaw("Vertical");
        // 玩家移动
        if (horizontalValue != 0 || verticalValue != 0)
        {
            isMove = true;
            Vector2 speed = moveSpeed * new Vector2(horizontalValue, verticalValue);
            rigidbody2.velocity = speed;
            animator.SetFloat("HorizontalValue", horizontalValue);
            animator.SetFloat("VerticalValue", verticalValue);
        }
        // 玩家停止移动
        else
        {
            rigidbody2.velocity = Vector2.zero;
            isMove = false;
        }
        animator.SetBool("IsMove", isMove);
    }
}
