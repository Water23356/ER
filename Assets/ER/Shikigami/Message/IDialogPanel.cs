using ER.Control;

namespace ER.Shikigami.Message
{
    /// <summary>
    /// 对话器接口
    /// </summary>
    public interface IDialogPanel : Minion
    {
        /// <summary>
        /// 面板是否可见
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 最终文本内容
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

        /// <summary>
        /// 设置对话文本
        /// </summary>
        /// <param name="reset">是否重置展示进度</param>
        /// <param name="text">新的文本</param>
        public void SetText(string text, bool reset = false);

        /// <summary>
        /// 添加新的文本(不重置)
        /// </summary>
        /// <param name="reset">是否重置展示进度</param>
        /// <param name="text">额外添加的文本</param>
        public void Append(string extra, bool reset = false);
    }
}