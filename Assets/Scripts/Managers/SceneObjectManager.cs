using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectManager : MonoBehaviour
{
    // 单例
    public static SceneObjectManager Instance;

    // 玩家
    private GameObject player;
    private PlayerControl playerControl;

    // 玩家最近有碰撞盒物体的距离
    private float minDistance;
    public GameObject minDistanceGameObject;
    private SceneObject minDistanceSceneObject;
    private LayerMask layerMask;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        minDistance = float.MaxValue;
        layerMask = 1 << 3;
        Instance = this;
    }

    private void Update()
    {
        minDistance = float.MaxValue;
        minDistanceSceneObject = null;
        minDistanceGameObject = null;
        // 获取玩家周围的碰撞盒
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.y);
        RaycastHit2D[] hit = Physics2D.RaycastAll(position, playerControl.vector2Towards, 0.4f, layerMask);
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.4f, layerMask);
        // 获取最短距离和对应物体
        foreach (var collider in hit)
        {
            Vector2 colliderPosition = new Vector2(collider.transform.position.x, collider.transform.position.y);
            float distance = Vector2.Distance(position, colliderPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceGameObject = collider.collider.gameObject;
                minDistanceSceneObject = minDistanceGameObject.GetComponent<SceneObject>();
            }
        }
    }

    /// <summary>
    /// 处理场景物品交互
    /// </summary>
    public void HandleObjectInteract()
    {
        playerControl.allowMove = false;
        GameManager.Instance.loadMainDialogue = false;

        HandleDialogue();

    }

    /// <summary>
    /// 处理场景物品交互对话框
    /// </summary>
    /// <param name="contents">交互显示的文字，按交互次数排列</param>
    private void HandleDialogue()
    {
        // 如果对话框未显示
        if (!GameManager.Instance.dialogueDisplayStatus)
        {
            string content;

            if (minDistanceSceneObject.interactCount < minDistanceSceneObject.objectDialogue.Length)
            {
                content = minDistanceSceneObject.objectDialogue[minDistanceSceneObject.interactCount++];
            }
            else
            {
                content = minDistanceSceneObject.objectDialogue[^1];
            }

            GameManager.Instance.SetDialogueUICharacterName(CharacterType.PangBai);
            GameManager.Instance.SetDialogueUIContent(content);
            GameManager.Instance.DisplayDialogueUI(true);
            GameManager.Instance.DisplayChoicePanel(false);
            GameManager.Instance.SetDialogueUIInteractable(true);
        }
    }
}
