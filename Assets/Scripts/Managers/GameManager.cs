using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 单例
    public static GameManager Instance;

    void Start()
    {
        Instance = this;
        // 限制最高帧率200
        Application.targetFrameRate = 200;
    }

    private void Update()
    {
        HandleFullScreen();
        HandleCursorDisplay();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            HandleKeyZ();
        }
    }

    /// <summary>
    /// 处理Z键
    /// </summary>
    public void HandleKeyZ()
    {
        // 如果正在输出
        if (UIManager.Instance.IsOutputingDialogue)
        {
            UIManager.Instance.NeedSkip = true;
        }
        // 如果输出完成
        else
        {
            LoadNextEvent();
        }
    }

    /// <summary>
    /// f11全屏
    /// </summary>
    private void HandleFullScreen()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
            Debug.Log($"切换全屏模式，当前全屏为{Screen.fullScreen}");
        }
    }

    /// <summary>
    /// alt鼠标显示
    /// </summary>
    private void HandleCursorDisplay()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    /// <summary>
    /// 根据枚举获取角色名全写
    /// </summary>
    /// <param name="characterType">角色枚举</param>
    /// <returns>角色名字符串</returns>
    public string GetCharacterName(CharacterType characterType)
    {
        return GameEventManager.Instance.GetCharacterName(characterType);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName">音效文件名（不加后缀）</param>
    public void PlaySound(string soundName)
    {
        AudioManager.Instance.PlaySound(soundName);
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="show">是否显示</param>
    public void DisplayDialogueUI(bool show = false)
    {
        UIManager.Instance.DisplayDialogueUI(show);
    }

    /// <summary>
    /// 设置对话框角色名
    /// </summary>
    /// <param name="characterType">角色枚举</param>
    public void SetDialogueUICharacterName(CharacterType characterType)
    {
        UIManager.Instance.SetDialogueUICharacterName(characterType);
    }

    /// <summary>
    /// 设置对话框内容
    /// </summary>
    /// <param name="content">对话框内容</param>
    public void SetDialogueUIContent(string content)
    {
        UIManager.Instance.SetDialogueUIContent(content);
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="character">角色枚举</param>
    /// <param name="eventType">事件类型枚举</param>
    /// <param name="eventData">事件数据</param>
    public void AddEvent(CharacterType character, EventType eventType, string eventData)
    {
        GameEventManager.Instance.AddEvent(character, eventType, eventData);
    }

    /// <summary>
    /// 载入下一事件并处理
    /// </summary>
    public void LoadNextEvent()
    {
        UIManager.Instance.LoadNextEvent();
    }

    /// <summary>
    /// 等待遮罩结束
    /// </summary>
    public void WaitForScreenMaskFinished(Action action)
    {
        UIManager.Instance.WaitForScreenMaskFinished(action);
    }
}
