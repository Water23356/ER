using System;

namespace ER.Shikigami.Message
{
    /// <summary>
    /// 消息窗口接口
    /// </summary>
    public interface IMessagePanel:Minion
    {
        /// <summary>
        /// 面板标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 面板是否可见
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel();

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel();


    }
    /// <summary>
    /// 确认消息框接口(确认/取消)
    /// </summary>
    public interface IConfirmPanel
    {
        /// <summary>
        /// 面板标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 面板是否可见
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 确认时触发的事件
        /// </summary>
        public Action ActConfirm { get; set; }
        /// <summary>
        /// 取消时触发的事件
        /// </summary>
        public Action ActCancel { get; set; }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel(Action act_confirm=null,Action act_cancel = null);

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel();
    }
}