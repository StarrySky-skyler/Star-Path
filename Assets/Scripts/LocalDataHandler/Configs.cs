// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2024/09/27 09:09
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace LocalDataHandler
{
    [Serializable]
    public class Configs
    {
        /// <summary>
        /// 礼物类型，1为吃东西，2为送礼物
        /// </summary>
        public int giftType;

        /// <summary>
        /// 礼物文本
        /// </summary>
        public string giftDetail;
    }

    public class ConfigsOperate
    {
        private static Configs _configs;

        // 单例
        private static ConfigsOperate _instance;

        public static ConfigsOperate Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConfigsOperate();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 私有化构造器 
        /// </summary>
        private ConfigsOperate()
        {
            _configs = ReadGift();
        }


        /// <summary>
        /// 保存礼物数据至本地json文件
        /// </summary>
        private void SaveGiftToJson()
        {
            // 转换 json
            var json = JsonUtility.ToJson(_configs, true);
            // utf8 编码写入
            using (var writer = new StreamWriter(Application.streamingAssetsPath + "/Configs.json",
                       false, Encoding.UTF8))
            {
                writer.Write(json);
            }
        }

        /// <summary>
        /// 设置礼物类型数据并保存
        /// </summary>
        /// <param name="giftType">礼物类型，1为吃东西，2为送礼物</param>
        public void WriteGiftType(int giftType)
        {
            _configs.giftType = giftType;
            SaveGiftToJson();
        }

        /// <summary>
        /// 设置礼物具体选择数据并保存
        /// </summary>
        /// <param name="giftData">礼物数据，对应按钮id，吃东西：1=>闪电泡芙配醒时春山奶茶2=>马卡龙配清茶3=>超级无敌巨无霸圣代|送礼物：1=>超级美丽星空限定版日记本2=>定制语录枫叶蝴蝶书签3=>最近很火的明星周边</param>
        public void WriteGiftData(int giftData)
        {
            switch (_configs.giftType)
            {
                case 1:
                    switch (giftData)
                    {
                        case 1:
                            _configs.giftDetail = "闪电泡芙配醒时春山奶茶";
                            break;
                        case 2:
                            _configs.giftDetail = "马卡龙配清茶";
                            break;
                        case 3:
                            _configs.giftDetail = "超级无敌巨无霸圣代";
                            break;
                    }
                    break;
                case 2:
                    switch (giftData)
                    {
                        case 1:
                            _configs.giftDetail = "超级美丽星空限定版日记本";
                            break;
                        case 2:
                            _configs.giftDetail = "定制语录枫叶蝴蝶书签";
                            break;
                        case 3:
                            _configs.giftDetail = "最近很火的明星周边";
                            break;
                    }
                    break;
            }

            SaveGiftToJson();
        }

        public Configs ReadGift()
        {
            UpdateGift();
            return _configs;
        }

        private void UpdateGift()
        {
            string content;
            // 以 utf8 读取文件
            using (var reader = new StreamReader(Application.streamingAssetsPath + "/Configs.json",
                       Encoding.UTF8))
            {
                content = reader.ReadToEnd();
            }
            
            _configs = JsonUtility.FromJson<Configs>(content);
        }
    }
}
