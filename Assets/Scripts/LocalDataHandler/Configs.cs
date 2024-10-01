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
            _configs = new Configs();
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
        /// 设置礼物数据并保存
        /// </summary>
        /// <param name="giftType">礼物类型，1为吃东西，2为送礼物</param>
        public void WriteGift(int giftType)
        {
            _configs.giftType = giftType;
            SaveGiftToJson();
        }

        public Configs ReadGift()
        {
            string content;
            // 以 utf8 读取文件
            using (var reader = new StreamReader(Application.streamingAssetsPath + "/Configs.json",
                       Encoding.UTF8))
            {
                content = reader.ReadToEnd();
            }

            _configs = JsonUtility.FromJson<Configs>(content);
            return _configs;
        }
    }
}