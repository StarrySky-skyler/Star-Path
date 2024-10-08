// ********************************************************************************
// @author: Starry Sky
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2024/10/05 14:10
// @version: 1.0
// @description:
// ********************************************************************************

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SecretBaseScene
{
    public class YukiPaintingFade : MonoBehaviour
    {
        // 渐变时间
        public float fadeTime;

        public bool TurnBlack { get; set; }

        private Image _img;

        private void Awake()
        {
            _img = GetComponent<Image>();
            TurnBlack = false;
        }

        private void Update()
        {
            if (_img.color.a < 1 && !TurnBlack)
            {
                _img.color += new Color(0F, 0F, 0F, Mathf.Lerp(0F, 1F, fadeTime) * Time.deltaTime);
            }

            if (TurnBlack)
            {
                StartCoroutine(TurnBlackHandler());
            }
        }

        private void OnEnable()
        {
            // 设置透明
            _img.color = new Color(1F, 1F, 1F, 0F);
        }

        private IEnumerator TurnBlackHandler()
        {
            if (_img.color.r > 0)
            {
                _img.color -= new Color(Mathf.Lerp(0F, 1F, fadeTime) * Time.deltaTime,
                    Mathf.Lerp(0F, 1F, fadeTime) * Time.deltaTime, Mathf.Lerp(0F, 1F, fadeTime) * Time.deltaTime, 0F);
                yield return null;
            }
        }
    }
}
