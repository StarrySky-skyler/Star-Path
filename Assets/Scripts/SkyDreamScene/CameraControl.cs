using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject player;
    private Vector3 relateVector;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        relateVector = transform.position - player.transform.position;
    }

    void Update()
    {
        // 相机跟随玩家
        transform.position = player.transform.position + relateVector;
    }
}
