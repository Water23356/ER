using ER.Control;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.Shikigami.Message
{
    /// <summary>
    /// 确认框
    /// </summary>
    public class ConfirmMessagePanel : MonoBehaviour, IConfirmPanel
    {
        #region 组件
        [SerializeField]
        private TMP_Text t_title;
        [SerializeField]
        private TMP_Text t_text;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Button bt_confirom;
        [SerializeField]
        private Button bt_cancel;
        #endregion

        #region 私有字段
        private bool visible = false;//窗口是否可见+
        private float counter;//帧计数器
        private Action actCofirm;//回调函数
        private Action actCancel;//回调函数

        #endregion
        public string Title { get => t_title.text; set => t_title.text = value; }
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
        public string Text { get => t_text.text; set => t_text.text = value; }
        public Action ActConfirm { get => actCofirm; set => actCofirm=value; }
        public Action ActCancel { get => actCancel; set => actCancel=value; }

        public void ClosePanel()
        {
            animator.SetBool("enable", false);
            visible = false;
        }

        public void OpenPanel(Action act_confirm = null, Action act_cancel = null)
        {
            actCofirm=act_confirm;
            actCancel=act_cancel;
            gameObject.SetActive(true);
            animator.SetBool("enable", true);
            visible = true;
        }

        protected virtual void Start()
        {
            bt_confirom.onClick.AddListener(() => 
            { 
                ClosePanel();
                actCofirm?.Invoke();
            });
            bt_cancel.onClick.AddListener(() =>
            {
                ClosePanel();
                actCancel?.Invoke();
            });
        }

        protected virtual void OnDisable()
        {
        }
        public void SetPanelDisable()
        {
            gameObject.SetActive(false);
        }

    }
}