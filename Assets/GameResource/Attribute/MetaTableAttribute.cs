using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev
{
    /// <summary>
    /// 如果该资源加载器需要被加入 GameResource 管理列表, 使用该特性修饰将会自动被实例化装载
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public class MetaTableAttribute : Attribute
    {
        private string m_metaHead;
        public string metaHead => m_metaHead;
        public MetaTableAttribute(string meteHead)
        {
            m_metaHead = meteHead;
        }
    }
}
