using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 剧本管理器单例
/// </summary>
public class DialogueManager
{
    private static DialogueManager instance;

    // 私有构造器
    private DialogueManager() { }

    // 静态单例
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DialogueManager();
            }
            return instance;
        }
    }

}
