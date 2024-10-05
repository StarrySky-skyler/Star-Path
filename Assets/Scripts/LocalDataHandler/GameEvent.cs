using System;
using System.Collections.Generic;
using UnityEngine;

namespace LocalDataHandler
{
    /// <summary>
    /// 剧本数据类
    /// </summary>
    [Serializable]
    public class GameEvent : ISerializationCallbackReceiver
    {
        /// <summary>
        /// 角色枚举
        /// </summary>
        [NonSerialized] public CharacterType CharacterType;

        /// <summary>
        /// 角色枚举对应的字符串
        /// </summary>
        public string character;

        /// <summary>
        /// 事件类型枚举
        /// </summary>
        [NonSerialized] public EventType EventType;

        /// <summary>
        /// 事件类型对应的字符串
        /// </summary>
        public string eventType;

        /// <summary>
        /// 事件数据
        /// </summary>
        public string eventData;

        /// <summary>
        /// 选择按钮事件跳转目标id
        /// </summary>
        public int jumpId;

        /// <summary>
        /// 剧情读取到toId后，跳转到toDesId
        /// </summary>
        public int toId;

        /// <summary>
        /// toId的目标剧情
        /// </summary>
        public int toDesId;

        /// <summary>
        /// 是否侦测礼物，0为无需跳转，1为需要跳转
        /// </summary>
        public int giftDetect;

        /// <summary>
        /// 礼物跳转id，1为吃东西，2为送礼物
        /// </summary>
        public int giftDesId;

        /// <summary>
        /// 是否保存当前礼物详细数据（非类型）
        /// </summary>
        public bool saveGiftData;

        /// <summary>
        /// 是否保存礼物类型
        /// </summary>
        public bool saveGiftType;

        /// <summary>
        /// 反序列化后，信息转为对象
        /// </summary>
        public void OnAfterDeserialize()
        {
            // 字符串转枚举
            CharacterType type = (CharacterType)Enum.Parse(typeof(CharacterType), character);
            EventType type2 = (EventType)Enum.Parse(typeof(EventType), eventType);
            // 枚举保存
            CharacterType = type;
            EventType = type2;
        }

        /// <summary>
        /// 序列化前
        /// </summary>
        public void OnBeforeSerialize()
        {
            character = CharacterType.ToString();
            eventType = EventType.ToString();
        }
    }

    /// <summary>
    /// 序列化装饰
    /// </summary>
    [Serializable]
    public class Wrapper
    {
        public List<GameEvent> list;
    }

    public enum CharacterType
    {
        Blank,
        Tsuki,
        Yuki,
        Jing,
        Sky,
        PangBai,
        StrangeA,
        StrangeB,
        StrangeC
    }

    public enum EventType
    {
        Blank,

        /// <summary>
        /// 对话事件，事件数据为剧本
        /// </summary>
        Dialogue,

        /// <summary>
        /// 选择事件，数据为字符串，每个选项以 | 隔开
        /// </summary>
        Choice,

        /// <summary>
        /// 音效事件，数据为音效文件
        /// </summary>
        Sound,
    
        /// <summary>
        /// 播放bgm事件，数据为bgm文件
        /// </summary>
        PlayBgm,
    
        /// <summary>
        /// 停止bgm事件
        /// </summary>
        StopBgm,

        /// <summary>
        /// 关闭对话框
        /// </summary>
        CloseDialogue,

        /// <summary>
        /// 载入下一场景
        /// </summary>
        LoadNextScene,

        /// <summary>
        /// 载入初始界面
        /// </summary>
        LoadMenuScene,
        
        /// <summary>
        /// 展示yuki的画
        /// </summary>
        ShowPaint,
        
        /// <summary>
        /// 画变为全黑
        /// </summary>
        PaintToBlack,
        
        /// <summary>
        /// 关闭画
        /// </summary>
        ClosePaint,
        
        /// <summary>
        /// 显示下雨特效
        /// </summary>
        ShowRainEffect
    }
}