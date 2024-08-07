using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource BgmPlayer;              // 背景音乐播放组件
    public AudioSource SoundPlayer;            // 点击音效播放组件

    private void Awake()
    {
        Instance = this;
        BgmPlayer.loop = true;
        SoundPlayer.loop = false;
    }

    void Start()
    {
        if (!BgmPlayer.isPlaying)
        {
            BgmPlayer.Play();
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName">音效文件名（不加后缀）</param>
    public void PlaySound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>("AudioClips/Sound/" + soundName);
        SoundPlayer.PlayOneShot(clip);
    }
}
