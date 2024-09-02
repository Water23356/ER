using System.Collections.Generic;

namespace ER.Shikigami.Message
{
    /// <summary>
    /// 选择器面板
    /// </summary>
    public interface ISelectPanel : Minion
    {
        /// <summary>
        /// 面板是否可见
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 选项文本
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel();

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel();
    }
}