// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2024/09/27 09:09
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using UnityEngine;
using System.IO;
using System.Text;

[Serializable]
public class Gift
{
    /// <summary>
    /// 礼物类型，1为吃东西，2为送礼物
    /// </summary>
    public int giftType;
}

public class GiftOperate
{
    private static Gift _gift;

    // 单例
    private static GiftOperate _instance;

    public static GiftOperate Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GiftOperate();
            }

            return _instance;
        }
    }

    /// <summary>
    /// 私有化构造器 
    /// </summary>
    private GiftOperate()
    {
        _gift = new Gift();
    }


    /// <summary>
    /// 保存礼物数据至本地json文件
    /// </summary>
    private void SaveGiftToJson()
    {
        // 转换 json
        var json = JsonUtility.ToJson(_gift, true);
        // utf8 编码写入
        using (var writer = new StreamWriter(Application.streamingAssetsPath + "/Gift.json",
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
        _gift.giftType = giftType;
        SaveGiftToJson();
    }

    public Gift ReadGift()
    {
        string content;
        // 以 utf8 读取文件
        using (var reader = new StreamReader(Application.streamingAssetsPath + "/Gift.json",
                   Encoding.UTF8))
        {
            content = reader.ReadToEnd();
        }

        _gift = JsonUtility.FromJson<Gift>(content);
        return _gift;
    }
}
