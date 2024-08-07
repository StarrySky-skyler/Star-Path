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
    Sky = 1,
    Yuki = 2,
    Jing = 3,
    Tsuki = 4,
    PangBai = 5,
    StrangeA = 6,
    StrangeB = 7,
    StrangeC = 8
}

public enum EventType
{
    /// <summary>
    /// 对话事件，事件数据为剧本
    /// </summary>
    Dialogue = 1,

    /// <summary>
    /// 选择事件
    /// </summary>
    Choose = 2,

    /// <summary>
    /// 音效事件，事件数据为音效文件
    /// </summary>
    Sound = 3,

    /// <summary>
    /// 关闭对话框
    /// </summary>
    CloseDialogue = 4
}
