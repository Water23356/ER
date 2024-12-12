using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER
{
    /// <summary>
    /// 进度面板类, 用于记录一个任务的工作进度, 并在任务完毕后执行回调
    /// </summary>
    public class ProgressMonitor
    {
        private int m_total;//总任务数
        private int m_current;//当前完成任务数
        private bool m_isDone;//是否完成

        /// <summary>
        /// 当进度增加时
        /// </summary>
        public event Action<ProgressMonitor> OnAddProgress;
        /// <summary>
        /// 当任务全部完成时
        /// </summary>
        public event Action OnDone;
        /// <summary>
        /// 总任务数
        /// </summary>
        public int total { get { return m_total; } }
        /// <summary>
        /// 当前完成任务数
        /// </summary>
        public int current { get { return m_current; } }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isDone { get { return m_isDone; } }
        /// <summary>
        /// 增加进度
        /// </summary>
        public void AddProgress()
        {
            if (isDone) return;
            m_current++;
            OnAddProgress?.Invoke(this);
            if (m_current >= m_total)
            {
                m_isDone = true;
                OnDone?.Invoke();
            }
        }
        public ProgressMonitor(int total)
        {
            this.m_total = total;
            if (total <= 0)
            {
                m_isDone = true;
            }
        }
    }
}
