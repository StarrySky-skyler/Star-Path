using System;
using JetBrains.Annotations;
using LocalDataHandler;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = LocalDataHandler.EventType;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        // 单例
        public static GameManager Instance;

        // z键是否可用
        public bool AllowLoadDialogue { get; set; }

        // 是否加载主剧情
        public bool LoadMainDialogue { get; set; }

        // 对话框显示状态
        public bool DialogueDisplayStatus { get; private set; }

        // 允许z键
        private bool _allowZ;
        private PlayerControl _player;
        
        // 下雨粒子效果
        [CanBeNull] private ParticleSystem _rainEffect;

        private void Awake()
        {
            Instance = this;
            // 限制最高帧率160
            Application.targetFrameRate = 160;
            // 初始化标志
            AllowLoadDialogue = true;
            LoadMainDialogue = true;
            _allowZ = false;
            
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
            // 获取下雨粒子系统
            try
            {
                _rainEffect = GameObject.Find("Particle System_Rain").GetComponent<ParticleSystem>();
            }
            catch (NullReferenceException)
            {
                _rainEffect = null;
            }
            // 停止下雨
            if (_rainEffect)
            {
                _rainEffect.Stop();
            }
        }

        private void Start()
        {
            // 等待全屏渐变动画完成后执行start
            Action rest = AfterScreenMaskFallBack;
            WaitForScreenMaskFinished(rest);
        }

        private void Update()
        {
            SwitchFullScreen();
            SwitchCursorDisplay();
            DialogueDisplayStatus = UIManager.Instance.parentDialogueUI.activeSelf;
            // 按下z键 且 允许继续剧情
            if (Input.GetKeyDown(KeyCode.Z) && _allowZ)
            {
                HandleKeyZ();
            }
        }

        /// <summary>
        /// 剩下的start代码（委托需要）
        /// </summary>
        private void AfterScreenMaskFallBack()
        {
            _allowZ = true;
            UIManager.Instance.sceneTipTxt.gameObject.SetActive(true);
            // 加载剧情
            LoadNextEvent();
        }

        /// <summary>
        /// 处理Z键
        /// </summary>
        private void HandleKeyZ()
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
                if (LoadMainDialogue && AllowLoadDialogue)
                {
                    LoadNextEvent();
                }
                // 加载场景物品交互对话
                else
                {
                    if (SceneObjectManager.Instance.ClosestGameObject)
                    {
                        // 对话框显示，关闭对话框
                        if (DialogueDisplayStatus)
                        {
#if UNITY_EDITOR
                            Debug.Log("触发关闭场景物品交互对话框事件");
#endif
                            _player.AllowMove = true;
                            DisplayDialogueUI(false);
                            AllowLoadDialogue = false;
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
        private void SwitchFullScreen()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }

        /// <summary>
        /// alt鼠标显示
        /// </summary>
        private void SwitchCursorDisplay()
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
        /// 停止bgm
        /// </summary>
        public void StopBgm()
        {
            AudioManager.Instance.StopBgm();
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="show">是否显示</param>
        public void DisplayDialogueUI(bool show)
        {
            UIManager.Instance.DisplayDialogueUI(show);
        }

        /// <summary>
        /// 设置对话框内容
        /// </summary>
        /// <param name="characterType">角色枚举类型</param>
        /// <param name="content">对话框内容</param>
        public void SetDialogueUI(CharacterType characterType, string content)
        {
            UIManager.Instance.SetDialogueUI(characterType, content);
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
        /// 处理剧情跳转
        /// </summary>
        /// <param name="dataEvent">事件类型</param>
        private void HandleDialogueJump(GameEvent dataEvent)
        {
            // 处理非选择的剧情跳转
            if (dataEvent.toId != 0)
            {
                var activate = true;
                while (activate)
                {
                    GameEvent next = GameEventManager.Instance.LoadNextEvent();
                    if (next.toDesId == dataEvent.toId)
                    {
                        GameEventManager.EventIndex -= 1;
                        activate = false;
                    }
                }
            }
        }

        /// <summary>
        /// 处理吃东西/送礼物的跳转
        /// </summary>
        /// <param name="dataEvent">事件类型</param>
        private void HandleGiftTypeJump(GameEvent dataEvent)
        {
            // 处理礼物相关
            if (dataEvent.giftDetect != 1) return;

            var giftData = ConfigsOperate.Instance.ReadGift();
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

        /// <summary>
        /// 保存玩家选择具体礼物数据
        /// </summary>
        /// <param name="dataEvent">事件类型</param>
        private void HandleSaveGift(GameEvent dataEvent)
        {
            if (dataEvent.saveGiftData)
            {
                ConfigsOperate.Instance.WriteGiftData(dataEvent.jumpId);
            }
            else if (dataEvent.saveGiftType)
            {
                ConfigsOperate.Instance.WriteGiftType(dataEvent.jumpId);
            }
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
#if UNITY_EDITOR
                    Debug.Log("触发对话事件");
#endif
                    // 初始化
                    AllowLoadDialogue = true;
                    // UI 设置
                    SetDialogueUI(nextEvent.CharacterType, nextEvent.eventData);
                    DisplayDialogueUI(true);
                    DisplayChoicePanel(false);
                    // 剧情跳转
                    HandleDialogueJump(nextEvent);
                    // 礼物
                    HandleGiftTypeJump(nextEvent);
                    HandleSaveGift(nextEvent);
                    break;
                // 选择事件
                case EventType.Choice:
#if UNITY_EDITOR
                    Debug.Log("触发多项选择事件");
#endif
                    AllowLoadDialogue = false;
                    DisplayChoicePanel(true, GameEventManager.Instance.GetChoicesCount());
                    Cursor.lockState = CursorLockMode.None;
                    break;
                // 音效事件
                case EventType.Sound:
#if UNITY_EDITOR
                    Debug.Log("触发播放音效事件");
#endif
                    PlaySound(nextEvent.eventData);
                    break;
                // 播放bgm事件
                case EventType.PlayBgm:
#if UNITY_EDITOR
                    Debug.Log("触发播放bgm事件");
#endif
                    PlayBgm(nextEvent.eventData);
                    break;
                // 停止bgm事件
                case EventType.StopBgm:
#if UNITY_EDITOR
                    Debug.Log("触发停止Bgm事件");
#endif
                    StopBgm();
                    break;
                // 关闭对话框事件
                case EventType.CloseDialogue:
#if UNITY_EDITOR
                    Debug.Log("触发关闭对话框事件");
#endif
                    DisplayDialogueUI(false);
                    AllowLoadDialogue = false;
                    LoadMainDialogue = false;
                    GameObject.FindWithTag("Player").GetComponent<PlayerControl>().AllowMove = true;
                    break;
                // 载入下一场景事件
                case EventType.LoadNextScene:
#if UNITY_EDITOR
                    Debug.Log("触发载入下一场景事件");
#endif
                    DisplayDialogueUI(false);
                    DisplayChoicePanel(false);
                    AllowLoadDialogue = false;
                    LoadMainDialogue = false;
                    _allowZ = false;
                    Action loadNext = ScenesManager.Instance.LoadNextScene;
                    WaitForScreenMaskFinished(loadNext, false);
                    break;
                case EventType.LoadMenuScene:
#if UNITY_EDITOR
                    Debug.Log("载入初始界面");
#endif
                    DisplayChoicePanel(false);
                    DisplayDialogueUI(false);
                    GameEventManager.EventIndex = 0;
                    Action loadMenu = LoadMenuSceneAction;
                    WaitForScreenMaskFinished(loadMenu, false);
                    break;
                // TODO:yuki画作展示相关的完善
                // TODO:剧情完善
                // TODO:背包系统（以下每个系统开发完成时，场景初添加Dialogue"Level Up!作者的开发能力提升了，你获得了xxx（提示酱：xxx)"这种
                // TODO:战斗系统
                // TODO:存档系统
                case EventType.ShowRainEffect:
#if UNITY_EDITOR
                    Debug.Log("下雨事件");
#endif
                    if (_rainEffect)
                    {
                        _rainEffect.Play();
                    }
#if UNITY_EDITOR
                    else
                    {
                        Debug.LogError("未找到下雨粒子系统物体");
                    }
#endif
                    break;
                case EventType.ShowPaint:
#if UNITY_EDITOR
                    Debug.Log("展示yuki画事件");
#endif
                    
                    break;
                case EventType.PaintToBlack:
#if UNITY_EDITOR
                    Debug.Log("yuki画变黑事件");
#endif
                    break;
                case EventType.ClosePaint:
#if UNITY_EDITOR
                    Debug.Log("关闭yuki画事件");
#endif
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
        /// 显示/隐藏选择框
        /// </summary>
        /// <param name="show">显示状态</param>
        /// <param name="choicesCount">选择数量</param>
        public void DisplayChoicePanel(bool show, int choicesCount = 0)
        {
            UIManager.Instance.DisplayChoicePanel(show, choicesCount);
        }
    }
}
