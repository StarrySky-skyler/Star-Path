using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 单例
    public static GameManager Instance;

    // z键是否可用
    public bool InteractableZ { get; set; }

    // 是否加载主剧情
    public bool LoadMainDialogue { get; set; }

    // 对话框显示状态
    public bool DialogueDisplayStatus { get; set; }

    void Awake()
    {
        Instance = this;
        // 限制最高帧率160
        Application.targetFrameRate = 160;
        InteractableZ = true;
        LoadMainDialogue = true;
    }

    private void Start()
    {
        foreach (var gameEvent in GameEventManager.Instance.GameEvents)
        {
            Debug.Log($"{gameEvent.CharacterType}:\n{gameEvent.eventData}");
        }

        Action rest = RestStart;
        WaitForScreenMaskFinished(rest);
    }

    private void Update()
    {
        HandleFullScreen();
        HandleCursorDisplay();
        DialogueDisplayStatus = UIManager.Instance.parentDialogueUI.activeSelf;
        // 按下z键 且 允许继续剧情
        if (Input.GetKeyDown(KeyCode.Z))
        {
            HandleKeyZ();
        }
    }

    /// <summary>
    /// 剩下的start代码（委托需要）
    /// </summary>
    private void RestStart()
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
            // 加载主线剧情
            if (LoadMainDialogue && InteractableZ)
            {
                LoadNextEvent();
            }
            // 加载场景物品交互对话
            else
            {
                if (SceneObjectManager.Instance.ClosestGameObject != null)
                {
                    // 对话框显示，关闭对话框
                    if (DialogueDisplayStatus)
                    {
                        Debug.Log("触发关闭场景物品交互对话框事件");
                        GameObject.FindWithTag("Player").GetComponent<PlayerControl>().AllowMove = true;
                        DisplayDialogueUI();
                        InteractableZ = false;
                    }
                    // 对话框隐藏，显示物品交互对话
                    else
                    {
                        SceneObjectManager.Instance.HandleObjectInteract();
                    }
                }
            }
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
        return GameEventManager.GetCharacterName(characterType);
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
    /// 播放/切换bgm
    /// </summary>
    /// <param name="bgmName">bgm文件名（不加后缀）</param>
    public void PlayBgm(string bgmName)
    {
        AudioManager.Instance.PlayBgm(bgmName);
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
        LoadMainDialogue = true;
        switch (nextEvent.EventType)
        {
            // 对话事件
            case EventType.Dialogue:
                Debug.Log("触发对话事件");
                SetDialogueUIInteractable();
                SetDialogueUICharacterName(nextEvent.CharacterType);
                SetDialogueUIContent(nextEvent.eventData);
                DisplayDialogueUI(true);
                DisplayChoicePanel();
                SetDialogueUIInteractable();
                // 处理非选择的剧情跳转
                if (nextEvent.toId != 0)
                {
                    var activate = true;
                    while (activate)
                    {
                        GameEvent next = GameEventManager.Instance.LoadNextEvent();
                        if (next.toDesId == nextEvent.toId)
                        {
                            GameEventManager.EventIndex -= 1;
                            activate = false;
                        }
                    }
                }
                
                // 处理礼物相关
                if (nextEvent.giftDetect == 1)
                {
                    var giftData = GiftOperate.Instance.ReadGift();
                    bool activate;
                    switch (giftData.giftType)
                    {
                        // 吃东西
                        case 1:
                            activate = true;
                            while (activate)
                            {
                                GameEvent next = GameEventManager.Instance.LoadNextEvent();
                                if (next.giftDesId == 1)
                                {
                                    GameEventManager.EventIndex -= 1;
                                    activate = false;
                                }
                            }
                            break;
                        // 送礼物
                        case 2:
                            activate = true;
                            while (activate)
                            {
                                GameEvent next = GameEventManager.Instance.LoadNextEvent();
                                if (next.giftDesId == 2)
                                {
                                    GameEventManager.EventIndex -= 1;
                                    activate = false;
                                }
                            }
                            break;
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
            
            // bgm事件
            case EventType.Bgm:
                Debug.Log("触发切换bgm事件");
                PlayBgm(nextEvent.eventData);
                break;

            // 关闭对话框事件
            case EventType.CloseDialogue:
                Debug.Log("触发关闭对话框事件");
                DisplayDialogueUI();
                InteractableZ = false;
                LoadMainDialogue = false;
                GameObject.FindWithTag("Player").GetComponent<PlayerControl>().AllowMove = true;
                break;

            // 载入下一场景事件
            case EventType.LoadNextScene:
                Debug.Log("触发载入下一场景事件");
                DisplayDialogueUI();
                DisplayChoicePanel();
                Action loadNext = ScenesManager.Instance.LoadNextScene;
                WaitForScreenMaskFinished(loadNext, false);
                break;

            case EventType.LoadMenuScene:
                Debug.Log("载入初始界面");
                DisplayChoicePanel();
                DisplayDialogueUI();
                GameEventManager.EventIndex = 0;
                Action loadMenu = LoadMenuSceneAction;
                WaitForScreenMaskFinished(loadMenu, false);
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
    /// <param name="choicesCount"></param>
    public void DisplayChoicePanel(bool show = false, int choicesCount = 0)
    {
        UIManager.Instance.DisplayChoicePanel(show, choicesCount);
    }
}