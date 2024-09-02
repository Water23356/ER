
using UnityEngine;

namespace ER.ForEditor
{
    /// <summary>
    /// 让编辑器 显示指定文本代替原先 显示字段名
    /// </summary>
    public class DisplayLabelAttribute : PropertyAttribute
    {
        public string label;

        public DisplayLabelAttribute(string label)
        {
            this.label = label;
        }
    }
}
