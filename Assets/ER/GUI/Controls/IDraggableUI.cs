using System;

namespace ER.GUI
{
    public interface IDraggableUI
    {
        /// <summary>
        /// 当被放置时触发的函数
        /// </summary>
        public void DragConfirm(IDropArea area);
    }
}