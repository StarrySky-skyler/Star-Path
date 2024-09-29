using UnityEngine;

public class SceneObject : MonoBehaviour
{
    /// <summary>
    /// 交互计数器
    /// </summary>
    public int InteractCount { get; private set; }

    public CharacterType speaker = CharacterType.Sky;

    /// <summary>
    /// 交互对话
    /// </summary>
    public string[] objectDialogue;

    private void Awake()
    {
        InteractCount = 0;
    }

    /// <summary>
    /// 增加场景物体交互次数
    /// </summary>
    public void AddInteractCount()
    {
        InteractCount++;
    }
}