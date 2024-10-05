using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
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
                string str = strs[^1];
                strs = str.Split('.');
                str = strs[0];
                if (currentSceneName == str)
                {
                    SceneManager.LoadScene(i + 1);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取当前场景名
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSceneName()
        {
            var currentScene = SceneManager.GetActiveScene();
            var currentSceneName = currentScene.name;

            switch (currentSceneName)
            {
                case "SkyDreamScene":
                    return "当前场景：???";
                case "ClassroomScene":
                    return "当前场景：经常来的教室";
                case "ClassroomScene2(love)":
                    return "当前场景：有yuki的教室";
                case "ForestScene":
                    return "当前场景：秘密基地";
                case "InFrontOfYukiHomeScene":
                    return "当前场景：yuki家前的公园";
                case "RoadToHomeScene":
                    return "当前场景：放学回家的路上";
            }

            return "";
        }
    }
}