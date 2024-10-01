using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace LocalDataHandler
{
    /// <summary>
    /// 剧本管理器单例
    /// </summary>
    public class GameEventManager
    {
        public List<GameEvent> GameEvents { get; private set; }

        private static GameEventManager _instance;

        // 事件索引
        public static int EventIndex;

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
                if (_instance == null)
                {
                    // 创建单例
                    _instance = new GameEventManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="character">角色枚举</param>
        /// <param name="eventType">事件类型枚举</param>
        /// <param name="eventData">事件数据</param>
        public void AddEvent(CharacterType character = CharacterType.Blank,
            EventType eventType = EventType.CloseDialogue,
            string eventData = "")
        {
            var gameEvent = new GameEvent
            {
                CharacterType = character,
                EventType = eventType,
                eventData = eventData
            };
            GameEvents.Add(gameEvent);
        }

        /// <summary>
        /// 保存事件泛型集合至 json 文件
        /// </summary>
        public void SaveDefaultEvents()
        {
            // 转换 json
            string json = JsonUtility.ToJson(new Wrapper() { list = GameEvents }, true);
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
            if (EventIndex < GameEvents.Count)
            {
                return GameEvents[EventIndex++];
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
        public static string GetCharacterName(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Blank:
                    return "";
                case CharacterType.Tsuki:
                    return "<color=#29AFC8>绘星tsuki</color>";
                case CharacterType.Yuki:
                    return "<color=#DC84BA>星梦yuki</color>";
                case CharacterType.Jing:
                    return "<color=#E79D24>静</color>(物理老师)";
                case CharacterType.Sky:
                    return "<color=#A824E7>小sky</color>";
                case CharacterType.PangBai:
                    return "<color=#18C32B>旁白</color>";
                case CharacterType.StrangeA:
                    return "<color=#FFFFFF>陌生的声音A</color>";
                case CharacterType.StrangeB:
                    return "<color=#FFFFFF>陌生的声音B</color>";
                case CharacterType.StrangeC:
                    return "<color=#FFFFFF>陌生的声音C</color>";
                default:
                    return "null";
            }
        }

        /// <summary>
        /// 获取多选按钮文本
        /// </summary>
        /// <param name="choicesCount">选项数</param>
        /// <returns>按钮文本数组</returns>
        public string[] GetChoicesContent(int choicesCount)
        {
            string[] contents = GameEvents[EventIndex - 1].eventData.Split('|');

            return contents;
        }

        /// <summary>
        /// 获取选项数
        /// </summary>
        /// <returns>选项数</returns>
        public int GetChoicesCount()
        {
            return GameEvents[EventIndex - 1].eventData.Split('|').Length;
        }
    }
}