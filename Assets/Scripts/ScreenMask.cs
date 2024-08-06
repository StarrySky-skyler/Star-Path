using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMask : MonoBehaviour
{
    // 逐渐黑屏 / 逐渐亮屏
    public bool isToBlack;
    // 运行状态
    public bool isFinished;

    // 全屏遮罩
    private RawImage rawImageScreen;

    private void Awake()
    {
        rawImageScreen = GetComponent<RawImage>();
        isFinished = false;
    }

    void Update()
    {
        // 黑屏过场
        if (isToBlack)
        {
            if (rawImageScreen.color.a < 1)
            {
                rawImageScreen.color += new Color(0, 0, 0, Mathf.Lerp(0, 1, 0.5f) * Time.deltaTime);
            }
            else
            {
                isFinished = true;
            }
        }
        // 亮屏过场
        else
        {
            if (rawImageScreen.color.a > 0)
            {
                rawImageScreen.color -= new Color(0, 0, 0, Mathf.Lerp(0, 1, 0.55f) * Time.deltaTime);
            }
            else
            {
                isFinished = true;
            }
        }
    }

    private void OnEnable()
    {
        if (isToBlack)
        {
            // 初始化图片为透明
            Color cr = rawImageScreen.color;
            cr.a = 0;
            rawImageScreen.color = cr;
        }
        else
        {
            // 初始化图片为黑色
            Color cr = rawImageScreen.color;
            cr.a = 1;
            rawImageScreen.color = cr;
        }
    }
}
