using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWords : MonoBehaviour
{
    // 开始全屏遮罩
    public GameObject startMask;

    // 结束全屏遮罩
    public GameObject endMask;

    private ScreenMask _startMask;
    private ScreenMask _endMask;

    private void Awake()
    {
        _startMask = startMask.GetComponent<ScreenMask>();
        _endMask = endMask.GetComponent<ScreenMask>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(WaitMask());
    }

    IEnumerator WaitMask()
    {
        // 等待初始遮罩完成
        while (!_startMask.IsFinished)
        {
            yield return null;
        }

        Destroy(startMask);
        yield return new WaitForSeconds(3);
        endMask.SetActive(true);

        // 等待结束遮罩完成
        while (!_endMask.IsFinished)
        {
            yield return null;
        }

        SceneManager.LoadScene(2);
    }
}