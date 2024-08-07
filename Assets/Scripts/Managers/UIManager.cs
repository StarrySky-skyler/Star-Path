using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // 单例
    public static UIManager Instance;

    // 对话框父物体
    public GameObject parentDialogueUI;
    // TMP角色名文字组件
    public TextMeshProUGUI tmpDialogueCharacter;
    // TMP剧情内容文字组件
    public TextMeshProUGUI tmpDialogueContent;
    // 全屏遮罩
    public GameObject screenMask;

    // 是否正在逐字输出
    public bool IsOutputingDialogue {get; private set;}
    // 是否跳过逐字输出
    public bool NeedSkip { get; set; }

    void Awake()
    {
        Instance = this;
        IsOutputingDialogue = false;
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="show">是否显示</param>
    public void DisplayDialogueUI(bool show = false)
    {
        if (show)
        {
            parentDialogueUI.SetActive(true);
        }
        else
        {
            parentDialogueUI.SetActive(false);
        }
    }

    /// <summary>
    /// 设置对话框角色名
    /// </summary>
    /// <param name="characterType">角色枚举</param>
    public void SetDialogueUICharacterName(CharacterType characterType)
    {
        string characterName = GameManager.Instance.GetCharacterName(characterType);
        tmpDialogueCharacter.text = characterName;
    }

    /// <summary>
    /// 设置对话框内容
    /// </summary>
    /// <param name="content">对话框内容</param>
    public void SetDialogueUIContent(string content)
    {
        StartCoroutine(LoadDialogueContentByLetter(content));
    }

    /// <summary>
    /// 协程逐字输出
    /// </summary>
    /// <param name="content">对话框内容</param>
    /// <returns></returns>
    private IEnumerator LoadDialogueContentByLetter(string content)
    {
        IsOutputingDialogue = true;
        tmpDialogueContent.text = "";
        GameObject.FindWithTag("Player").GetComponent<PlayerControl>().allowMove = false;
        // 遍历对话框内容
        foreach (var letter in content)
        {
            if (!NeedSkip)
            {
                tmpDialogueContent.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                tmpDialogueContent.text = content;
                IsOutputingDialogue = false;
                NeedSkip = false;
                yield break;
            }
        }
        IsOutputingDialogue = false;
    }

    /// <summary>
    /// 载入下一事件并处理
    /// </summary>
    public void LoadNextEvent()
    {
        // 获取下一事件
        GameEvent nextEvent = GameEventManager.Instance.LoadNextEvent();
        switch (nextEvent.eventType)
        {
            // 对话事件
            case EventType.Dialogue:
                SetDialogueUICharacterName(nextEvent.characterType);
                SetDialogueUIContent(nextEvent.eventData);
                DisplayDialogueUI(true);
                break;
            // 选择事件
            case EventType.Choose:
                DisplayDialogueUI(false);
                break;
            // 音效事件
            case EventType.Sound:
                break;
            default:
                DisplayDialogueUI(false);
                GameObject.FindWithTag("Player").GetComponent<PlayerControl>().allowMove = true;
                break;
        }
    }

    /// <summary>
    /// 等待遮罩结束
    /// </summary>
    public void WaitForScreenMaskFinished(Action action)
    {
        StartCoroutine(WaitMask(action));
    }

    private IEnumerator WaitMask(Action restOperation)
    {
        while (!screenMask.GetComponent<ScreenMask>().isFinished)
        {
            yield return null;
        }
        restOperation();
    }
}
