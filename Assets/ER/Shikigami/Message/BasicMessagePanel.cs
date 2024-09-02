using ER.Control;
using System;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.Shikigami.Message
{
    /// <summary>
    /// 一般消息框
    /// <code>
    /// 式神指令:
    ///     OpenDialogPanel:打开面板并注册控制权,延时
    ///         0:(string)标题字符串,
    ///         1:(string)内容字符串,
    ///     CloseDialogPanel:关闭面板并注销控制权,即时
    ///     SetTitle:设置标题字符串
    ///         0:(string)标题字符串
    ///     SetMessage:设置名称文本,即时
    ///         0:(string)文本字符串
    /// </code>
    /// </summary>
    public class BasicMessagePanel : MonoBehaviour, IMessagePanel
    {
        #region 组件
        [SerializeField]
        private TMP_Text t_title;
        [SerializeField]
        private TMP_Text t_text;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Button button;
        #endregion

        #region 私有字段
        private bool visible = false;//窗口是否可见+
        private float counter;//帧计数器
        private Action<Instruct> callback;//回调函数
        #endregion

        public string Title { get => t_title.text; set => t_title.text = value; }
        public bool Visible 
        {
            get => visible ;
            set
            {
                visible = value;
                if (visible)
                    OpenPanel();
                else
                    ClosePanel();
            }

        }
        public string Text { get => t_text.text; set => t_text.text = value; }

        public void ClosePanel()
        {
            animator.SetBool("enable", false);
            visible = false;
        }

        public bool Execute(Instruct ist, Action<Instruct> callback = null)
        {
            this.callback = callback;
            switch(ist.name)
            {
                case "OpenMessagePanel":
                    OpenPanel();
                    Title = ist.marks[0];
                    Text = ist.marks[1];
                    return false;
                case "CloseMessagePanel":
                    ClosePanel();
                    return true;
                case "SetMessage":
                    Text = ist.marks[0];
                    return true;
                case "SetTitle":
                    Title = ist.marks[0];
                    return true;
                default:
                    return true;
            }
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
            animator.SetBool("enable", true);
            button.Select();//强制使按钮聚焦
            //LayoutRebuilder.ForceRebuildLayoutImmediate();
            visible = true;
        }
        public void OpenPanel(string title,string text)
        {
            Title = title;
            Text = text;
            OpenPanel();
        }

        protected virtual void OnDisable()
        {
        }
        public void SetPanelDisable()
        {
            gameObject.SetActive(false);
        }
        protected virtual void Start()
        {
            button.onClick.AddListener(() =>
            {
                ClosePanel();
                callback?.Invoke(null);
            });
        }
    }
}