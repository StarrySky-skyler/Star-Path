using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // 允许移动
    public bool AllowMove { get; set; }

    // 玩家朝向
    public Vector2 Vector2Towards { get; set; }

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    // 移动速度
    private float _moveSpeed;

    // 是否移动状态
    private bool _isMove;

    void Awake()
    {
        // 获取组件
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        AllowMove = true;
        _isMove = false;
        _moveSpeed = 5.5f;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // 如果是教室场景，玩家默认朝上
        if (SceneManager.GetActiveScene().name == "ClassroomScene")
        {
            _animator.SetFloat("VerticalValue", 1f);
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
        if ((horizontalValue != 0 || verticalValue != 0) && AllowMove)
        {
            _isMove = true;
            Vector2Towards = new Vector2(horizontalValue, verticalValue);
            Vector2 speed = _moveSpeed * Vector2Towards;
            _rigidbody.velocity = speed;
            _animator.SetFloat("HorizontalValue", horizontalValue);
            _animator.SetFloat("VerticalValue", verticalValue);
        }
        // 玩家停止移动
        else
        {
            _rigidbody.velocity = Vector2.zero;
            _isMove = false;
        }

        _animator.SetBool("IsMove", _isMove);
    }

    /// <summary>
    /// shift加速移动
    /// </summary>
    private void HandleRunSpeedUp()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            _moveSpeed = 9f;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _moveSpeed = 9f;
        }
        else
        {
            _moveSpeed = 5.5f;
        }
    }
}