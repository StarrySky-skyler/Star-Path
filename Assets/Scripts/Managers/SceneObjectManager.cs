using Player;
using UnityEngine;

namespace Managers
{
    public class SceneObjectManager : MonoBehaviour
    {
        // 单例
        public static SceneObjectManager Instance;

        // 玩家
        private GameObject _player;
        private PlayerControl _playerControl;
        private Transform _playerColliderPoint;

        public GameObject ClosestGameObject { get; private set; }
        private LayerMask _layerMask;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerControl = _player.GetComponent<PlayerControl>();
            _layerMask = 1 << 3;
            _playerColliderPoint = _player.transform.Find("ColliderPoint");
            Instance = this;
        }

        private void Update()
        {
            ClosestGameObject = null;
            // 获取玩家周围的碰撞盒
            Vector2 position = new Vector2(_playerColliderPoint.position.x, _playerColliderPoint.position.y);
            Debug.DrawRay(position, _playerControl.Vector2Towards, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(position, _playerControl.Vector2Towards, 1f, _layerMask);
            if (hit.collider)
            {
                ClosestGameObject = hit.collider.gameObject;
            }
        }

        /// <summary>
        /// 处理场景物品交互
        /// </summary>
        public void HandleObjectInteract()
        {
            _playerControl.AllowMove = false;
            GameManager.Instance.LoadMainDialogue = false;

            HandleDialogue();
        }

        /// <summary>
        /// 处理场景物品交互对话框
        /// </summary>
        private void HandleDialogue()
        {
            // 如果对话框未显示
            if (!GameManager.Instance.DialogueDisplayStatus)
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

                GameManager.Instance.SetDialogueUI(closestSceneObject.speaker, content);
                GameManager.Instance.DisplayDialogueUI(true);
                GameManager.Instance.DisplayChoicePanel(false);
                GameManager.Instance.SetDialogueUIInteractable();
            }
        }
    }
}