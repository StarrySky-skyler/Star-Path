using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 剧本管理器单例
/// </summary>
public class GameEventManager
{
    public List<GameEvent> GameEvents { get; private set; }

    private static GameEventManager instance;
    // 事件索引
    private int eventIndex = 0;

    // 私有构造器
    private GameEventManager()
    {
        GameEvents = new List<GameEvent>();
        LoadEvents();
    }

    // 静态单例
    public static GameEventManager Instance
    {
        get
        {
            // 如果单例不存在
            if (instance == null)
            {
                // 创建单例
                instance = new GameEventManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="character">角色枚举</param>
    /// <param name="eventType">事件类型枚举</param>
    /// <param name="eventData">事件数据</param>
    public void AddEvent(CharacterType character, EventType eventType, string eventData)
    {
        GameEvent gameEvent = new GameEvent();
        gameEvent.characterType = character;
        gameEvent.eventType = eventType;
        gameEvent.eventData = eventData;
        GameEvents.Add(gameEvent);
    }

    /// <summary>
    /// 保存事件泛型集合至 json 文件
    /// </summary>
    public void SaveEvents()
    {
        // 转换 json
        string json = JsonUtility.ToJson(new Wrapper() { list = GameEvents}, true);
        // utf8 编码写入
        using (StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/EventData.json",
                                                            false, Encoding.UTF8))
        {
            writer.Write(json);
        }
        Debug.Log("将数据写入EventData.json完成");
    }

    /// <summary>
    /// 载入剧本数据
    /// </summary>
    private void LoadEvents()
    {
        string content;
        // 以 utf8 读取文件
        using (StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/EventData.json",
                                                      Encoding.UTF8))
        {
            content = reader.ReadToEnd();
        }
        GameEvents = JsonUtility.FromJson<Wrapper>(content).list;
    }

    /// <summary>
    /// 加载下个事件
    /// </summary>
    public GameEvent LoadNextEvent()
    {
        if (eventIndex <= GameEvents.Count)
        {
            return GameEvents[eventIndex++];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 根据枚举获取角色名全写
    /// </summary>
    /// <param name="characterType">角色枚举</param>
    /// <returns>角色名字符串</returns>
    public string GetCharacterName(CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterType.Sky:
                return "<color=#29AFC8>星痕sky</color>";
            case CharacterType.Yuki:
                return "<color=#DC84BA>星梦yuki</color>";
            case CharacterType.Jing:
                return "<color=#E79D24>静</color>(物理老师)";
            case CharacterType.Tsuki:
                return "<color=#A824E7>Tsuki</color>";
            case CharacterType.PangBai:
                return "<color=#18C32B>旁白</color>";
            case CharacterType.StrangeA:
                return "<color=#2936C8>陌生的声音A</color>";
            case CharacterType.StrangeB:
                return "<color=#2936C8>陌生的声音B</color>";
            case CharacterType.StrangeC:
                return "<color=#2936C8>陌生的声音C</color>";
            default:
                return "null";
        }
    }
}
