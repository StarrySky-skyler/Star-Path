using System;
using System.Collections;
using LocalDataHandler;
using Player;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        // 单例
        public static UIManager Instance;

        // 对话框父物体
        public GameObject parentDialogueUI;

        // TMP角色名文字组件
        public TextMeshProUGUI tmpDialogueCharacter;

        // TMP剧情内容文字组件
        public TextMeshProUGUI tmpDialogueContent;

        // 全屏遮罩
        public GameObject startScreenMask;
        public GameObject endScreenMask;

        // 选择框父物体
        public GameObject buttonChoicesParent;

        // 选择框
        public GameObject[] buttonChoices;

        // 选择框文本
        public TextMeshProUGUI[] tmpChoices;

        // 剧情完成后的提示箭头
        public GameObject dialogueNextTip;

        // 是否正在逐字输出
        public bool IsOutputingDialogue { get; private set; }

        // 是否跳过逐字输出
        public bool NeedSkip { get; set; }

        void Awake()
        {
            Instance = this;
            IsOutputingDialogue = false;
            startScreenMask.SetActive(true);
            endScreenMask.SetActive(false);
            dialogueNextTip.SetActive(false);
        }

        /// <summary>
        /// 多选框点击调用函数
        /// </summary>
        /// <param name="id">按钮id</param>
        public void ButtonChoiceClick(int id)
        {
            var activate = true;
            Cursor.lockState = CursorLockMode.Locked;
            while (activate)
            {
                GameEvent next = GameEventManager.Instance.LoadNextEvent();
                if (next.jumpId == id)
                {
                    GameEventManager.EventIndex -= 1;
                    GameManager.Instance.LoadNextEvent();
                    activate = false;
                }

                // 保存礼物数据
                switch (next.jumpId)
                {
                    // 吃东西
                    case 1:
                        ConfigsOperate.Instance.WriteGift(1);
                        break;
                    // 送礼物
                    case 2:
                        ConfigsOperate.Instance.WriteGift(2);
                        break;
                }
            }
        }

        /// <summary>
        /// 显示/隐藏选择框
        /// </summary>
        /// <param name="show">显示状态</param>
        /// <param name="choicesCount">选择数量</param>
        public void DisplayChoicePanel(bool show, int choicesCount = 0)
        {
            buttonChoicesParent.SetActive(show);
            // 隐藏所有按钮
            foreach (var btn in buttonChoices)
            {
                btn.SetActive(false);
            }

            // 显示按钮数对应的按钮
            for (int i = 0; i < choicesCount; i++)
            {
                buttonChoices[i].SetActive(true);
            }

            // 显示按钮文本
            if (show)
            {
                string[] contents = GameEventManager.Instance.GetChoicesContent(choicesCount);
                for (global::System.Int32 i = 0; i < contents.Length; i++)
                {
                    tmpChoices[i].text = contents[i];
                }
            }
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="show">是否显示</param>
        public void DisplayDialogueUI(bool show)
        {
            parentDialogueUI.SetActive(show);
        }

        /// <summary>
        /// 设置对话框是否可点击
        /// </summary>
        /// <param name="interactable">是否可点击</param>
        public void SetDialogueUIInteractable(bool interactable = true)
        {
            GameManager.Instance.InteractableZ = interactable;
        }
    
        /// <summary>
        /// 设置对话框内容
        /// </summary>
        /// <param name="characterType">角色枚举类型</param>
        /// <param name="content">对话框内容</param>
        public void SetDialogueUI(CharacterType characterType, string content)
        {
            var characterName = GameManager.Instance.GetCharacterName(characterType);
            tmpDialogueCharacter.text = characterName;
            StartCoroutine(LoadDialogueContentByLetter(content));
        }

        /// <summary>
        /// 协程逐字输出
        /// </summary>
        /// <param name="content">对话框内容</param>
        /// <returns></returns>
        private IEnumerator LoadDialogueContentByLetter(string content)
        {
            IsOutputingDialogue = true;
            tmpDialogueContent.text = "";
            dialogueNextTip.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().AllowMove = false;
            // 遍历对话框内容
            foreach (var letter in content)
            {
                if (!NeedSkip)
                {
                    tmpDialogueContent.text += letter;
                    yield return new WaitForSeconds(0.05f);
                }
                else
                {
                    tmpDialogueContent.text = content;
                    dialogueNextTip.SetActive(true);
                    IsOutputingDialogue = false;
                    NeedSkip = false;
                    yield break;
                }
            }

            IsOutputingDialogue = false;
            dialogueNextTip.SetActive(true);
        }

        /// <summary>
        /// 等待遮罩结束
        /// </summary>
        /// <param name="action">剩下的操作</param>
        /// <param name="isStart">是否为起始遮罩</param>
        public void WaitForScreenMaskFinished(Action action, bool isStart = true)
        {
            StartCoroutine(WaitMask(action, isStart));
        }

        private IEnumerator WaitMask(Action restOperation, bool isStart)
        {
            // 如果为起始遮罩
            if (isStart)
            {
                while (!startScreenMask.GetComponent<ScreenMask>().IsFinished)
                {
                    yield return null;
                }

                restOperation();
            }
            // 结束遮罩
            else
            {
                endScreenMask.SetActive(true);
                while (!endScreenMask.GetComponent<ScreenMask>().IsFinished)
                {
                    yield return null;
                }

                restOperation();
            }
        }
    }
}
