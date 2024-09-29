using UnityEngine;
using UnityEngine.UI;

public class ScreenMask : MonoBehaviour
{
    // 运行状态
    public bool IsFinished { get; private set; }

    // 全屏遮罩
    private RawImage _rawImageScreen;

    // 逐渐黑屏 / 逐渐亮屏
    private bool _isToBlack;

    private void Awake()
    {
        _rawImageScreen = GetComponent<RawImage>();
        IsFinished = false;
        switch (gameObject.name)
        {
            case "StartScreenMask":
                _isToBlack = false;
                break;
            case "EndScreenMask":
                _isToBlack = true;
                break;
        }
    }

    void Update()
    {
        // 黑屏过场
        if (_isToBlack)
        {
            if (_rawImageScreen.color.a < 1)
            {
                _rawImageScreen.color += new Color(0, 0, 0, Mathf.Lerp(0, 1, 0.5f) * Time.deltaTime);
            }
            else
            {
                IsFinished = true;
            }
        }
        // 亮屏过场
        else
        {
            if (_rawImageScreen.color.a > 0)
            {
                _rawImageScreen.color -= new Color(0, 0, 0, Mathf.Lerp(0, 1, 0.55f) * Time.deltaTime);
            }
            else
            {
                IsFinished = true;
            }
        }
    }

    private void OnEnable()
    {
        if (_isToBlack)
        {
            // 初始化图片为透明
            Color cr = _rawImageScreen.color;
            cr.a = 0;
            _rawImageScreen.color = cr;
        }
        else
        {
            // 初始化图片为黑色
            Color cr = _rawImageScreen.color;
            cr.a = 1;
            _rawImageScreen.color = cr;
        }
    }
}