using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWords : MonoBehaviour
{
    // 开始全屏遮罩
    public GameObject StartMask;
    // 结束全屏遮罩
    public GameObject EndMask;

    private ScreenMask startMask;
    private ScreenMask endMask;

    private void Awake()
    {
        startMask = StartMask.GetComponent<ScreenMask>();
        endMask = EndMask.GetComponent<ScreenMask>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(WaitMask());
    }

    IEnumerator WaitMask()
    {
        // 等待初始遮罩完成
        while (!startMask.isFinished)
        {
            yield return null;
        }

        Destroy(StartMask);
        yield return new WaitForSeconds(5);
        EndMask.SetActive(true);

        // 等待结束遮罩完成
        while (!endMask.isFinished)
        {
            yield return null;
        }

        SceneManager.LoadScene(2);
    }
}
