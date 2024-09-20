using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev
{
    /// <summary>
    /// 注册资源
    /// </summary>
    public interface IRegisterResource
    {
        /// <summary>
        /// 资源注册名
        /// </summary>
        public RegistryName registryName { get; }
        /// <summary>
        /// 获取原始资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>();
    }
}
