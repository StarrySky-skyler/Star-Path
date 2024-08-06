using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public GameObject btnLayoutMain;            // 主界面所有按钮Layout
    public GameObject panelArchive;             // 存档UI
    public GameObject panelHelp;                // 帮助UI
    public GameObject FullScreenMask;           // 全屏遮罩

    private void Start()
    {
        Screen.fullScreen = true;
        // 显示主界面按钮
        btnLayoutMain.SetActive(true);
        panelArchive.SetActive(false);
        panelHelp.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
            Debug.Log($"切换全屏模式，当前全屏为{Screen.fullScreen}");
        }
    }

    /// <summary>
    /// 新游戏
    /// </summary>
    public void ButtonNewGameClick()
    {
        Debug.Log("点击新游戏按钮");
        MenuAudioManager.Instance.PlaySound("Hit");
        StartCoroutine(WaitForScreenMask());
    }

    /// <summary>
    /// 协程等待遮罩完成
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForScreenMask()
    {
        FullScreenMask.SetActive(true);
        ScreenMask screenMask = FullScreenMask.GetComponent<ScreenMask>();
        while (!screenMask.isFinished)
        {
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 读取游戏
    /// </summary>
    public void ButtonReadGameClick()
    {
        Debug.Log("点击读取游戏按钮");
        MenuAudioManager.Instance.PlaySound("Hit");
        // 隐藏主界面
        btnLayoutMain.SetActive(false);
        // 显示存档UI
        panelArchive.SetActive(true);
    }

    /// <summary>
    /// 操作教程
    /// </summary>
    public void ButtonHelpClick()
    {
        Debug.Log("点击帮助按钮");
        MenuAudioManager.Instance.PlaySound("Hit");
        // UI控制
        btnLayoutMain.SetActive(false);
        panelHelp.SetActive(true);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ButtonExitClick()
    {
        Debug.Log("点击退出游戏按钮");
#if UNITY_EDITOR
        //如果是在编辑器环境下
        UnityEditor.EditorApplication.isPlaying = false;
#else
            //在打包出来的环境下
            Application.Quit();
#endif
    }

    /// <summary>
    /// 返回主界面
    /// </summary>
    public void ButtonBackClick()
    {
        MenuAudioManager.Instance.PlaySound("Hit");
        panelArchive.SetActive(false);
        panelHelp.SetActive(false);
        btnLayoutMain.SetActive(true);
    }
}
