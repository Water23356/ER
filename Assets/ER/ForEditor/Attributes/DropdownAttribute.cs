using UnityEngine;



namespace ER.ForEditor
{
    /// <summary>
    /// 下拉框绘制器
    /// </summary>
    public class DropdownAttribute : PropertyAttribute
    {
        private string[] options;

        public virtual string[] GetOptions()
        {
            return options;
        }

        public DropdownAttribute(params string[] options)
        {
            this.options = options;
        }
    }
}
