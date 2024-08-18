using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _relateVector;

    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        _relateVector = transform.position - _player.transform.position;
    }

    void Update()
    {
        // 相机跟随玩家
        transform.position = _player.transform.position + _relateVector;
    }
}