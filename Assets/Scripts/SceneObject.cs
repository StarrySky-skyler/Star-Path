using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{
    /// <summary>
    /// 交互计数器
    /// </summary>
    public int interactCount;
    /// <summary>
    /// 交互对话
    /// </summary>
    public string[] objectDialogue;

    private void Awake()
    {
        interactCount = 0;
    }
}
