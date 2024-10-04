using ER.ForEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ER.ResourceManager
{
    public abstract class MetaResource : ScriptableObject, IRegisterResource
    {
        [DisplayLabel("注册名")]
        public RegistryName m_registryName;
        /// <summary>
        /// 数据表头(用于指示是哪一类数据的配置文件)
        /// </summary>
        public abstract string metaHead { get; }

        public RegistryName registryName => m_registryName;

        public abstract T Get<T>();
    }
}
