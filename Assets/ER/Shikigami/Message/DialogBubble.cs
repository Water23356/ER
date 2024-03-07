using ER.Control;
using ER.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.Shikigami.Message
{
    /// <summary>
    /// 对话泡:
    /// 名称面板, 展示动画, 允许跳过动画
    /// <code>
    /// 式神指令:
    ///    # SetPosition:
    ///    # OpenDialogPanel:打开面板并注册控制权,即时
    ///    # CloseDialogPanel:关闭面板并注销控制权,即时
    ///    # SetDialogText:设置文本,延时
    ///         0:(string)文本字符串
    ///         1:(float) 显示完文本后 暂停播放时间(缺省=0)
    ///    # AppendDialogText:额外添加文本,延时
    ///         0:(string)文本字符串
    ///         1:(float) 显示完文本后 暂停播放时间(缺省=0)
    ///    # SetPosition:设置UI显示位置,即时
    ///         0:(float) 位置x
    ///         1:(float) 位置y
    ///    # SetOwner:设置UI所属者, 以及位置偏移量
    ///         0:(string) 所属者的锚点标签, 会根据这个标签获取 Transform 对象并进行绑定
    ///         1:(float) 位置x偏移量(缺省=0)
    ///         2:(float) 位置y偏移量(缺省=0)
    /// </code>
    /// </summary>
    public class DialogBubble :  MonoBehaviour, IDialogPanel
    {

        [SerializeField]
        private TMP_Text t_text;//展示文本

        [SerializeField]
        private Animator animator;//自身动画器
        [SerializeField]
        private RectTransform self;

        #region 私有字段

        private string text;//最终文本
        private int progress;//展示进度(0<= x <= text.length)
        private float counter;//帧计数器
        private bool display = false;//是否处于正在展示的过程
        private Action<Instruct> callback;//回调函数
        private bool visible = false;//窗口是否可见+
        private float timer = 0;//暂停播放时间
        private Transform owner;//对话泡所属对象, 如果存在, 则位置跟随该对象
        private Vector2 offset;//相对对象的位置偏移量

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
        #endregion

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

                case "SetDialogText":
                    if (ist.marks.Length >= 2)
                        timer = float.Parse(ist.marks[1]);
                    else
                        timer = 0;
                    SetText(ist.marks[0], true);
                    return false;

                case "AppendDialogText":
                    if (ist.marks.Length >= 2)
                        timer = float.Parse(ist.marks[1]);
                    else
                        timer = 0;
                    Append(ist.marks[0], true);
                    return false;

                default:
                    Debug.LogError("未知指令:" + ist.name);
                    return true;
            }
        }
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

        public void OpenPanel()
        {
            gameObject.SetActive(true);
            Action close = ()=>UIAnimator.Instance.AddAnimation(self, UIAnimator.AnimationType.BoxClose_Left);
            UIAnimator.Instance.AddAnimation(t_text.rectTransform, UIAnimator.AnimationType.FadeOut);

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
            LayoutRebuilder.ForceRebuildLayoutImmediate(self);//强制刷新自身布局, 防止UI大小不匹配字符长度

            if (progress == text.Length)//意为文本展示完毕
            {
                display = false;
            }
            if(timer > 0)
            {
                Invoke("_CallBack", timer);
            }
            else
            {
                _CallBack();
            }
        }

        private void _CallBack()
        {
            callback?.Invoke(null);//调用回调函数
        }

        private void SkipToEnd()
        {
            progress = text.Length;
            t_text.text = text;
            display = false;
        }

        private void FixedUpdate()
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
        public void SetPanelDisable()
        {
            gameObject.SetActive(false);
        }
    }
}