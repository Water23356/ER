using ER.Entity2D.Enum;
using System.Linq;
using UnityEngine;

namespace  ER.Entity2D
{
    /// <summary>
    /// 标签过滤器
    /// </summary>
    [CreateAssetMenu(fileName = "TagFilter", menuName = "TagFilter", order = 1)]
    public class TagFilter : ScriptableObject
    {
        public enum Mode
        {
            Off,
            BlackList,
            WhiteList
        }

        [Tooltip("筛选模式")]
        public Mode filterMode = Mode.Off;

        [Tooltip("筛选名单")]
        public Tags[] list;

        private string[] tags;

        /// <summary>
        /// 过滤器, 如果结果保留则返回 true,若应被过滤则返回 false
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool Filter(string tag)
        {
            switch (filterMode)
            {
                case Mode.Off:
                    return true;

                case Mode.BlackList:
                    return !tags.Contains(tag);

                case Mode.WhiteList:
                    return tags.Contains(tag);
            }
            return true;
        }

        public bool Filter(Tags tag)
        {
            switch (filterMode)
            {
                case Mode.Off:
                    return true;

                case Mode.BlackList:
                    return !list.Contains(tag);

                case Mode.WhiteList:
                    return list.Contains(tag);
            }
            return true;
        }

        private void Awake()
        {
            tags = new string[list.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = list[i].ToString();
            }
        }
    }
}