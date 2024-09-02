using UnityEngine;

namespace ER.GUI
{
    public interface ILerpLayoutGroup
    {
        /// <summary>
        /// 插值速度
        /// </summary>
        public float LerpSpeed { get; set; }
        /// <summary>
        /// 最大插值速度
        /// </summary>
        public float MaxLerpSpeed { get; set; }


        /// <summary>
        /// 刷新布局
        /// </summary>
        public void UpdateLayout();
    }
}