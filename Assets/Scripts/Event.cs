using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 剧本数据类
/// </summary>
[Serializable]
public class Event
{
    /// <summary>
    /// 角色枚举
    /// </summary>
    public CharacterType Character { get; set; }
    /// <summary>
    /// 事件类型枚举
    /// </summary>
    public EventType EventType { get; set; }
    /// <summary>
    /// 事件数据
    /// </summary>
    public string EventData { get; set; }

}

public enum CharacterType
{
    Sky = 1,
    Yuki = 2,
    Jing = 3,
    Tsuki = 4,
    PangBai = 5
}

public enum EventType
{
    // 对话事件，事件数据为剧本
    Dialogue,
    // 音效事件，事件数据为音效文件
    Sound
}
