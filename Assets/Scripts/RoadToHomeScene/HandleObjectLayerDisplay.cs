using UnityEngine;

namespace RoadToHomeScene
{
    public class HandleObjectLayerDisplay : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _playerColliderPoint;
        // 场景物体精灵图
        private SpriteRenderer _objSpriteSpriteRenderer;
        // 玩家精灵图
        private SpriteRenderer _playerSpriteRenderer;

        private int _sortingOrder;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerColliderPoint = _player.transform.Find("ColliderPoint").gameObject;
            _objSpriteSpriteRenderer = GetComponent<SpriteRenderer>();
            _sortingOrder = _objSpriteSpriteRenderer.sortingOrder;
            _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < 10f)
            {
                // 玩家在物体下方
                if (transform.position.y > _playerColliderPoint.transform.position.y)
                {
                    _objSpriteSpriteRenderer.sortingOrder = _playerSpriteRenderer.sortingOrder - 1;
                }
                // 玩家在物体上方
                else
                {
                    _objSpriteSpriteRenderer.sortingOrder = _playerSpriteRenderer.sortingOrder + 1;
                }
            }
            else
            {
                _objSpriteSpriteRenderer.sortingOrder = _sortingOrder;
            }
        }
    }
}
