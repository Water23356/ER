﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ER.ResourceManager
{
    public class TextResource : IRegisterResource
    {
        private RegistryName m_registryName;
        public RegistryName registryName => m_registryName;

        private string value;

        public TextResource(RegistryName regName, string value)
        {
            m_registryName = regName;
            this.value = value;
        }

        public T Get<T>()
        {
            if (value is T)
                return (T)(object)value;
            Debug.LogWarning($"资源类型封装错误: 预期类型: {typeof(T).FullName} 实际类型: {value.GetType().FullName}");
            return default(T);
        }
        public static implicit operator string(TextResource obj)
        {
            return obj.value;
        }
    }
}
