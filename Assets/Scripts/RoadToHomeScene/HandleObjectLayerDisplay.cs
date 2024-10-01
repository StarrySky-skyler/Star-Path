using UnityEngine;

namespace RoadToHomeScene
{
    public class HandleObjectLayerDisplay : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _playerColliderPoint;
        private SpriteRenderer _sprite;

        private int _sortingOrder;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerColliderPoint = _player.transform.Find("ColliderPoint").gameObject;
            _sprite = GetComponent<SpriteRenderer>();
            _sortingOrder = _sprite.sortingOrder;
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < 10f)
            {
                // 玩家在物体下方
                if (transform.position.y > _playerColliderPoint.transform.position.y)
                {
                    _sprite.sortingOrder = _player.GetComponent<SpriteRenderer>().sortingOrder - 1;
                }
                // 玩家在物体上方
                else
                {
                    _sprite.sortingOrder = _player.GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
            }
            else
            {
                _sprite.sortingOrder = _sortingOrder;
            }
        }
    }
}
