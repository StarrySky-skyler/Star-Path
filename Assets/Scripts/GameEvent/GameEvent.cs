using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剧本数据类
/// </summary>
[Serializable]
public class GameEvent : ISerializationCallbackReceiver
{
    /// <summary>
    /// 角色枚举
    /// </summary>
    [NonSerialized]
    public CharacterType characterType;

    /// <summary>
    /// 角色枚举对应的字符串
    /// </summary>
    public string characterString;

    /// <summary>
    /// 事件类型枚举
    /// </summary>
    [NonSerialized]
    public EventType eventType;

    /// <summary>
    /// 事件类型对应的字符串
    /// </summary>
    public string eventString;

    /// <summary>
    /// 事件数据
    /// </summary>
    public string eventData;

    /// <summary>
    /// 选择跳转id
    /// </summary>
    public int jumpId;

    /// <summary>
    /// 非选择主动跳转id
    /// </summary>
    public int toId;

    /// <summary>
    /// 非选择主动目标id
    /// </summary>
    public int toDesId;

    /// <summary>
    /// 反序列化后，信息转为对象
    /// </summary>
    public void OnAfterDeserialize()
    {
        // 字符串转枚举
        CharacterType type = (CharacterType)Enum.Parse(typeof(CharacterType), characterString);
        EventType type2 = (EventType)Enum.Parse(typeof(EventType), eventString);
        // 枚举保存
        characterType = type;
        eventType = type2;
    }

    /// <summary>
    /// 序列化前
    /// </summary>
    public void OnBeforeSerialize()
    {
        characterString = characterType.ToString();
        eventString = eventType.ToString();
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
    Sky,
    Yuki,
    Jing,
    Tsuki,
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
    LoadMenuScene
}
