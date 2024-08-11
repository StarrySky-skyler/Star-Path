using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 单例
    public static GameManager Instance;

    // z键是否可用
    public bool interactableZ;

    void Awake()
    {
        Instance = this;
        // 限制最高帧率200
        Application.targetFrameRate = 200;
        interactableZ = true;
    }

    private void Start()
    {
        foreach (var gameEvent in GameEventManager.Instance.GameEvents)
        {
            Debug.Log($"{gameEvent.characterType}:\n{gameEvent.eventData}");
        }
        Action rest = new Action(restStart);
        WaitForScreenMaskFinished(rest);
    }

    private void Update()
    {
        HandleFullScreen();
        HandleCursorDisplay();
        if (Input.GetKeyDown(KeyCode.Z) && interactableZ)
        {
            HandleKeyZ();
        }
    }

    /// <summary>
    /// 剩下的start代码（委托需要）
    /// </summary>
    private void restStart()
    {
        // 加载剧情
        LoadNextEvent();
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
        // 获取下一事件
        GameEvent nextEvent = GameEventManager.Instance.LoadNextEvent();
        switch (nextEvent.eventType)
        {
            // 对话事件
            case EventType.Dialogue:
                Debug.Log("触发对话事件");
                SetDialogueUICharacterName(nextEvent.characterType);
                SetDialogueUIContent(nextEvent.eventData);
                DisplayDialogueUI(true);
                DisplayChoicePanel(false);
                SetDialogueUIInteractable(true);
                // 处理非选择的剧情跳转
                if (nextEvent.toId != 0)
                {
                    bool activate = true;
                    while (activate)
                    {
                        GameEvent next = GameEventManager.Instance.LoadNextEvent();
                        if (next.toDesId == nextEvent.toId)
                        {
                            GameEventManager.Instance.eventIndex -= 1;
                            activate = false;
                        }
                    }
                }
                break;

            // 选择事件
            case EventType.Choice:
                Debug.Log("触发多项选择事件");
                SetDialogueUIInteractable(false);
                DisplayChoicePanel(true, GameEventManager.Instance.GetChoicesCount());
                Cursor.lockState = CursorLockMode.None;
                break;

            // 音效事件
            case EventType.Sound:
                Debug.Log("触发播放音效事件");
                PlaySound(nextEvent.eventData);
                break;

            // 关闭对话框事件
            case EventType.CloseDialogue:
                Debug.Log("触发关闭对话框事件");
                DisplayDialogueUI(false);
                GameObject.FindWithTag("Player").GetComponent<PlayerControl>().allowMove = true;
                break;

            // 载入下一场景事件
            case EventType.LoadNextScene:
                Debug.Log("触发载入下一场景事件");
                DisplayDialogueUI(false);
                DisplayChoicePanel(false);
                Action loadNext = new Action(ScenesManager.Instance.LoadNextScene);
                WaitForScreenMaskFinished(loadNext, false);
                break;

            case EventType.LoadMenuScene:
                Debug.Log("载入初始界面");
                DisplayChoicePanel(false);
                DisplayDialogueUI(false);
                GameEventManager.Instance.eventIndex = 0;
                Action loadMenu = new Action(LoadMenuSceneAction);
                WaitForScreenMaskFinished(loadMenu, false);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 载入初始场景委托回调
    /// </summary>
    private void LoadMenuSceneAction()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 等待遮罩结束
    /// </summary>
    /// <param name="action">剩下的操作</param>
    /// <param name="isStart">是否为起始遮罩</param>
    public void WaitForScreenMaskFinished(Action action, bool isStart = true)
    {
        UIManager.Instance.WaitForScreenMaskFinished(action, isStart);
    }

    /// <summary>
    /// 载入下一场景
    /// </summary>
    public void LoadNextScene()
    {
        ScenesManager.Instance.LoadNextScene();
    }

    /// <summary>
    /// 设置对话框是否可点击
    /// </summary>
    /// <param name="interactable">是否可点击</param>
    public void SetDialogueUIInteractable(bool interactable = true)
    {
        UIManager.Instance.SetDialogueUIInteractable(interactable);
    }

    /// <summary>
    /// 显示/隐藏选择框
    /// </summary>
    /// <param name="show"></param>
    public void DisplayChoicePanel(bool show = false, int choicesCount = 0)
    {
        UIManager.Instance.DisplayChoicePanel(show, choicesCount);
    }
}
