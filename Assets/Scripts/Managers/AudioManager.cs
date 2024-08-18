using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmPlayer; // 背景音乐播放组件
    public AudioSource soundPlayer; // 点击音效播放组件

    private void Awake()
    {
        Instance = this;
        bgmPlayer.loop = true;
        soundPlayer.loop = false;
    }

    void Start()
    {
        if (!bgmPlayer.isPlaying)
        {
            bgmPlayer.Play();
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName">音效文件名（不加后缀）</param>
    public void PlaySound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>("AudioClips/Sound/" + soundName);
        soundPlayer.PlayOneShot(clip);
    }
}