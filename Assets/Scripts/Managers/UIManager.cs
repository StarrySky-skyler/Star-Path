using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject startScreenMask;
    public GameObject endScreenMask;

    // 选择框父物体
    public GameObject buttonChoicesParent;
    // 选择框
    public GameObject[] buttonChoices;
    // 选择框文本
    public TextMeshProUGUI[] tmpChoices;

    // 是否正在逐字输出
    public bool IsOutputingDialogue { get; private set; }
    // 是否跳过逐字输出
    public bool NeedSkip { get; set; }

    void Awake()
    {
        Instance = this;
        IsOutputingDialogue = false;
    }

    /// <summary>
    /// 多选框点击调用函数
    /// </summary>
    /// <param name="id">按钮id</param>
    public void ButtonChoiceClick(int id)
    {
        bool activate = true;
        Cursor.lockState = CursorLockMode.Locked;
        while (activate)
        {
            GameEvent next = GameEventManager.Instance.LoadNextEvent();
            if (next.jumpId == id)
            {
                GameEventManager.Instance.eventIndex -= 1;
                GameManager.Instance.LoadNextEvent();
                activate = false;
            }
        }
    }

    /// <summary>
    /// 显示/隐藏选择框
    /// </summary>
    /// <param name="show"></param>
    public void DisplayChoicePanel(bool show = false, int choicesCount = 0)
    {
        buttonChoicesParent.SetActive(show);
        // 隐藏所有按钮
        foreach (var btn in buttonChoices)
        {
            btn.SetActive(false);
        }
        // 显示按钮数对应的按钮
        for (int i = 0; i < choicesCount; i++)
        {
            buttonChoices[i].SetActive(true);
        }
        // 显示按钮文本
        if (show)
        {
            string[] contents = GameEventManager.Instance.GetChoicesContent(choicesCount);
            for (global::System.Int32 i = 0; i < contents.Length; i++)
            {
                tmpChoices[i].text = contents[i];
            }
        }
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="show">是否显示</param>
    public void DisplayDialogueUI(bool show = false)
    {
        parentDialogueUI.SetActive(show);
    }

    /// <summary>
    /// 设置对话框是否可点击
    /// </summary>
    /// <param name="interactable">是否可点击</param>
    public void SetDialogueUIInteractable(bool interactable = true)
    {
        GameManager.Instance.interactableZ = interactable;
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
    /// 等待遮罩结束
    /// </summary>
    /// <param name="action">剩下的操作</param>
    /// <param name="isStart">是否为起始遮罩</param>
    public void WaitForScreenMaskFinished(Action action, bool isStart = true)
    {
        StartCoroutine(WaitMask(action, isStart));
    }

    private IEnumerator WaitMask(Action restOperation, bool isStart)
    {
        // 如果为起始遮罩
        if (isStart)
        {
            while (!startScreenMask.GetComponent<ScreenMask>().isFinished)
            {
                yield return null;
            }
            restOperation();
        }
        // 结束遮罩
        else
        {
            endScreenMask.SetActive(true);
            while (!endScreenMask.GetComponent<ScreenMask>().isFinished)
            {
                yield return null;
            }
            restOperation();
        }
    }
}
