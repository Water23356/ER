using ER.Control;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.Shikigami.Message
{
    /// <summary>
    /// 基础的对话面板:
    /// 名称面板, 文本面板, 展示动画, 允许跳过动画
    /// <code>
    /// 式神指令:
    ///    # OpenDialogPanel:打开面板并注册控制权,即时
    ///    # CloseDialogPanel:关闭面板并注销控制权,即时
    ///    # SetDialogName:设置名称文本,即时
    ///         0:(string)名称字符串
    ///    # SetDialogText:设置文本,延时
    ///         0:(string)文本字符串
    ///    # AppendDialogText:额外添加文本,延时
    ///         0:(string)文本字符串
    /// </code>
    /// </summary>
    public class BasicDialogPanel : MonoBehaviour, IDialogPanel
    {
        #region 组件

        [SerializeField]
        private TMP_Text t_name;//名称文本

        [SerializeField]
        private TMP_Text t_text;//展示文本

        [SerializeField]
        private Animator animator;//自身动画器

        #endregion 组件

        #region 私有字段

        private string text;//最终文本
        private int progress;//展示进度(0<= x <= text.length)
        private float counter;//帧计数器
        private bool display = false;//是否处于正在展示的过程
        private Action<Instruct> callback;//回调函数
        private bool visible = false;//窗口是否可见+

        #endregion 私有字段

        #region 属性

        /// <summary>
        /// 每次更新显示的帧间隔(F)
        /// </summary>
        public int intervalF = 5;

        /// <summary>
        /// 每次更新展示字符的数量(F)
        /// </summary>
        public int countF = 1;

        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                if (visible)
                    OpenPanel();
                else
                    ClosePanel();
            }
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                SetText(text, false);
            }
        }
        /// <summary>
        /// 名称文本
        /// </summary>
        public string NameText
        {
            get=>t_name.text;
            set
                {
                t_name.text = value;
                LayoutRebuilder.ForceRebuildLayoutImmediate(t_name.transform.parent.GetComponent<RectTransform>());
            }
        }

        #endregion 属性



        #region 方法

        public void Append(string extra, bool reset = false)
        {
            text += extra;
            if (reset)
            {
                progress = 0;
            }
            display = true;
        }

        public void ClosePanel()
        {
            animator.SetBool("enable", false);
            visible = false;
        }

        public bool Execute(Instruct ist, Action<Instruct> callback = null)
        {
            this.callback = callback;
            switch (ist.name)
            {
                case "OpenDialogPanel":
                    OpenPanel();
                    return true;

                case "CloseDialogPanel":
                    ClosePanel();
                    return true;

                case "SetDialogName":
                    NameText = ist.marks[0];
                    return true;

                case "SetDialogText":
                    SetText(ist.marks[0], true);
                    return false;

                case "AppendDialogText":
                    Append(ist.marks[0], true);
                    return false;

                default:
                    Debug.LogError("未知指令:" + ist.name);
                    return true;
            }
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
            animator.SetBool("enable", true);
            visible = true;
        }

        public void SetText(string text, bool reset = false)
        {
            this.text = text;
            if (reset)
            {
                progress = 0;
            }
            display = true;
        }

        private void UpdateNextChars()//更新下一次显示的字符
        {
            progress = Math.Min(progress + countF, text.Length);
            t_text.text = text.Substring(0, progress);
            if (progress == text.Length)//意为文本展示完毕
            {
                display = false;
            }
        }

        private void SkipToEnd()
        {
            progress = text.Length;
            t_text.text = text;
            display = false;
        }

        protected virtual void FixedUpdate()
        {
            if (display)
            {
                if (counter > 0)
                {
                    counter--;
                }
                else
                {
                    counter = intervalF;
                    UpdateNextChars();
                }
            }
        }
        protected virtual void Update()
        {
            if (Input.GetButtonDown("Submit"))//动画播放中确认跳过动画
            {
                if(display)
                    SkipToEnd();
                else
                    callback?.Invoke(null);//调用回调函数
            }
        }


        public void SetPanelDisable()
        {
            gameObject.SetActive(false);
        }

        #endregion 方法
    }
}