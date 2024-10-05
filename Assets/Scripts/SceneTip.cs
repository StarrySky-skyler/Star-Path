// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2024/10/04 19:10
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class SceneTip : MonoBehaviour
{
    // 渐变时间，测试默认为3
    public float fadeTime;
    // 渐变后显示时间，测试默认为2.5
    public float displayTime;
    
    private TextMeshProUGUI _text;

    // 是否透明转显示完成
    private bool _isBlack;
    // 是否显示转透明（全动画）完成
    private bool _isFinished;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _isBlack = false;
        _isFinished = false;
    }

    private void Start()
    {
        _text.text = Managers.ScenesManager.Instance.GetCurrentSceneName();
        if (_text.text == "???")
        {
            _text.color = new Color32(153, 41, 173, 0);
        }
        else
        {
            _text.color = new Color(1, 1, 1, 0);
        }
    }

    private void Update()
    {
        var rest = new Action(DestroySelf);
        StartCoroutine(WaitForFade(rest));
    }

    private void OnEnable()
    {
        // 初始化透明颜色
        var color = _text.color;
        color.a = 0;
        _text.color = color;
    }

    /// <summary>
    /// 渐变显示
    /// </summary>
    private void HandleFadeToBlack()
    {
        if (_text.color.a < 1 && !_isBlack)
        {
            _text.color += new Color(0, 0, 0, Mathf.Lerp(0, 1, fadeTime) * Time.deltaTime);
        }
        else
        {
            _isBlack = true;
        }
    }

    /// <summary>
    /// 渐变隐藏
    /// </summary>
    private void HandleFadeToTransparent()
    {
        if (_isBlack)
        {
            if (_text.color.a > 0)
            {
                _text.color -= new Color(0, 0, 0, Mathf.Lerp(0, 1, fadeTime) * Time.deltaTime);
            }
            else
            {
                _isFinished = true;
            }
        }
    }

    /// <summary>
    /// 渐变后回调函数销毁自身
    /// </summary>
    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private IEnumerator WaitForFade([CanBeNull] Action fallBack = null)
    {
        HandleFadeToBlack();
        if (_isBlack)
        {
            // 等待显示时间
            yield return new WaitForSeconds(displayTime);
        }
        HandleFadeToTransparent();
        while (!_isBlack || !_isFinished)
        {
            yield return null;
        }
        fallBack?.Invoke();
    }
}
