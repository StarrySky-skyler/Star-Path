using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // 单例
    public static ScenesManager Instance;

    private void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// 载入下一场景
    /// </summary>
    public void LoadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        // 当前场景名
        string currentSceneName = currentScene.name;
        // build settings的所有场景数
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        // 获取当前场景索引
        for (int i = 0; i < sceneCount; i++)
        {
            string[] strs = SceneUtility.GetScenePathByBuildIndex(i).Split('/');
            string str = strs[strs.Length - 1];
            strs = str.Split('.');
            str = strs[0];
            if (currentSceneName == str)
            {

                SceneManager.LoadScene(i + 1);
                break;
            }
        }
    }
}
