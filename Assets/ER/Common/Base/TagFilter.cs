using ER.ForEditor;
using System;
using System.Collections.Generic;

namespace ER
{
    [Serializable]
    public class TagFilter
    {
        public enum FilterMode
        {
            None,
            Whitelist,
            Blacklist
        }

        [DisplayLabel("过滤模式")]
        public FilterMode mode = FilterMode.None;

        public List<string> tags = new List<string>();

        /// <summary>
        /// 只有满足条件的标签输入才会返回true
        /// <code>
        /// 白名单模式:
        ///     输入标签 被包括在列表中时 返回true, 否则返回false
        /// 黑名单模式:
        ///     输入标签 被包括在列表中时 返回false, 否则返回true
        /// 无:
        ///     返回true
        /// </code>
        /// </summary>
        /// <returns></returns>
        public bool Filter(string tag)
        {
            switch (mode)
            {
                case FilterMode.None:
                    return true;

                case FilterMode.Whitelist:
                    return tags.Contains(tag);

                case FilterMode.Blacklist:
                    return !tags.Contains(tag);
            }
            return true;
        }

        /// <summary>
        /// 只有满足条件的标签输入才会返回true,并执行handle代码
        /// <code>
        /// 白名单模式:
        ///     输入标签 被包括在列表中时 返回true, 否则返回false
        /// 黑名单模式:
        ///     输入标签 被包括在列表中时 返回false, 否则返回true
        /// 无:
        ///     返回true
        /// </code>
        /// </summary>
        /// <returns></returns>
        public bool Filter(string tag, Action handle)
        {
            switch (mode)
            {
                case FilterMode.None:
                    return true;

                case FilterMode.Whitelist:
                    if (tag.Contains(tag))
                    {
                        handle?.Invoke();
                        return true;
                    }
                    return false;

                case FilterMode.Blacklist:
                    if (!tag.Contains(tag))
                    {
                        handle?.Invoke();
                        return true;
                    }
                    return false;
            }
            handle?.Invoke();
            return true;
        }
    }
}