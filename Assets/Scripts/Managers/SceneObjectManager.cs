using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    // 单例
    public static SceneObjectManager Instance;

    // 玩家
    private GameObject _player;
    private PlayerControl _playerControl;

    public GameObject ClosestGameObject { get; private set; }
    private LayerMask _layerMask;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerControl = _player.GetComponent<PlayerControl>();
        _layerMask = 1 << 3;
        Instance = this;
    }

    private void Update()
    {
        ClosestGameObject = null;
        // 获取玩家周围的碰撞盒
        Vector2 position = new Vector2(_player.transform.position.x, _player.transform.position.y - 0.164f);
        Debug.DrawRay(position, _playerControl.vector2Towards, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(position, _playerControl.vector2Towards, 1f, _layerMask);
        if (hit.collider != null)
        {
            ClosestGameObject = hit.collider.gameObject;
        }
    }

    /// <summary>
    /// 处理场景物品交互
    /// </summary>
    public void HandleObjectInteract()
    {
        _playerControl.allowMove = false;
        GameManager.Instance.loadMainDialogue = false;

        HandleDialogue();
    }

    /// <summary>
    /// 处理场景物品交互对话框
    /// </summary>
    private void HandleDialogue()
    {
        // 如果对话框未显示
        if (!GameManager.Instance.dialogueDisplayStatus)
        {
            string content;
            SceneObject closestSceneObject = ClosestGameObject.GetComponent<SceneObject>();

            if (closestSceneObject.InteractCount < closestSceneObject.objectDialogue.Length)
            {
                content = closestSceneObject.objectDialogue[closestSceneObject.InteractCount];
                closestSceneObject.AddInteractCount();
            }
            else
            {
                content = closestSceneObject.objectDialogue[^1];
            }

            GameManager.Instance.SetDialogueUICharacterName(closestSceneObject.speaker);
            GameManager.Instance.SetDialogueUIContent(content);
            GameManager.Instance.DisplayDialogueUI(true);
            GameManager.Instance.DisplayChoicePanel();
            GameManager.Instance.SetDialogueUIInteractable();
        }
    }
}