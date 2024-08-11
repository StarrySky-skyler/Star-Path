using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // 允许移动
    public bool allowMove;
    // 玩家朝向
    public Vector2 vector2Towards;

    private Animator animator;
    private Rigidbody2D rigidbody2;

    // 移动速度
    private float moveSpeed;

    // 是否移动状态
    private bool isMove;

    void Awake()
    {
        // 获取组件
        animator = GetComponent<Animator>();
        rigidbody2 = GetComponent<Rigidbody2D>();

        allowMove = true;
        isMove = false;
        moveSpeed = 5.5f;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // 如果是教室场景，玩家默认朝上
        if (SceneManager.GetActiveScene().name == "ClassroomScene")
        {
            animator.SetFloat("VerticalValue", 1f);
        }
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
        HandleRunSpeedUp();
        float horizontalValue = Input.GetAxisRaw("Horizontal");
        float verticalValue = Input.GetAxisRaw("Vertical");
        // 玩家移动
        if ((horizontalValue != 0 || verticalValue != 0) && allowMove)
        {
            isMove = true;
            vector2Towards = new Vector2(horizontalValue, verticalValue);
            Vector2 speed = moveSpeed * vector2Towards;
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

    /// <summary>
    /// shift加速移动
    /// </summary>
    private void HandleRunSpeedUp()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            moveSpeed = 9f;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = 9f;
        }
        else
        {
            moveSpeed = 5.5f;
        }
    }
}
