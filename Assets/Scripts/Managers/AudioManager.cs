using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource bgmPlayer; // 背景音乐播放组件
        public AudioSource soundPlayer; // 点击音效播放组件

        // 淡入淡出过渡时间
        public float fadeDuration;

        private void Awake()
        {
            Instance = this;
            bgmPlayer.loop = true;
            soundPlayer.loop = false;
            bgmPlayer.playOnAwake = false;
            soundPlayer.playOnAwake = false;
            bgmPlayer.Stop();
            soundPlayer.Stop();
        }

        void Start()
        {
            if (!bgmPlayer.isPlaying)
            {
                PlayBgm();
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

        /// <summary>
        /// 播放/切换bgm
        /// </summary>
        /// <param name="bgmName">bgm文件名（不加后缀）</param>
        public void PlayBgm([CanBeNull] string bgmName = null)
        {
            if (bgmName != null)
            {
                var clip = Resources.Load<AudioClip>("AudioClips/Bgm/" + bgmName);
                bgmPlayer.Stop();
                bgmPlayer.clip = clip;
                StartCoroutine(FadeAudio(0f, 1f));
                bgmPlayer.Play();
            }
            else
            {
                bgmPlayer.Stop();
                StartCoroutine(FadeAudio(0f, 1f));
                bgmPlayer.Play();
            }
        }

        /// <summary>
        /// 停止bgm
        /// </summary>
        public void StopBgm()
        {
            StartCoroutine(FadeAudio(1f, 0f));
            if (bgmPlayer.volume == 0f)
            {
                bgmPlayer.Stop();
            }
        }

        /// <summary>
        /// 暂停bgm
        /// </summary>
        public void PauseBgm()
        {
            StartCoroutine(FadeAudio(1f, 0f));
            if (bgmPlayer.volume == 0f)
            {
                bgmPlayer.Pause();
            }
        }

        /// <summary>
        /// 淡入淡出音频
        /// </summary>
        /// <param name="startVolume">开始音量</param>
        /// <param name="targetVolume">目标结束时音量</param>
        /// <returns></returns>
        private IEnumerator FadeAudio(float startVolume, float targetVolume)
        {
            var currentTime = 0f;
            bgmPlayer.volume = startVolume;

            // 线性赋值实现淡入淡出效果
            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                bgmPlayer.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
                yield return null;
            }

            bgmPlayer.volume = targetVolume;
        }
    }
}
