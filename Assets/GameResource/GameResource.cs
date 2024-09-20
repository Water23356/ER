using System.Collections.Generic;
using UnityEngine;

namespace Dev
{
    public class GameResource
    {
        private static GameResource instance;

        public static GameResource Instance
        {
            get
            {
                return instance;
            }
        }

        private Dictionary<string, IResourceLoader> loaders = new Dictionary<string, IResourceLoader>();

        public void ELoad(string regName)
        {
            var key = new RegistryName(regName);
            var loader = GetLoader(key.Head);
            if (loader != null)
            {
                if (!loader.IsLoaded(key))
                    loader.Load(key, null);
            }
            else
            {
                Debug.LogError($"在加载 {regName} 时失败: 缺失 {key.Head} 资源加载器!");
            }
        }

        public void Load(string regName)
        {
            var key = new RegistryName(regName);
            var loader = GetLoader(key.Head);
            if (loader != null)
            {
                loader.Load(key, null);
            }
            else
            {
                Debug.LogError($"在加载 {regName} 时失败: 缺失 {key.Head} 资源加载器!");
            }
        }

        public void UnLoad(string regName)
        {
            var key = new RegistryName(regName);
            var loader = GetLoader(key.Head);
            if (loader != null)
            {
                loader.UnLoad(key);
            }
            else
            {
                Debug.LogError($"在卸载 {regName} 时失败: 缺失 {key.Head} 资源加载器!");
            }
        }

        public bool IsLoaded(string regName)
        {
            var key = new RegistryName(regName);
            var loader = GetLoader(key.Head);
            if (loader != null)
            {
                return loader.IsLoaded(key);
            }
            else
            {
                Debug.LogError($"缺失 {key.Head} 资源加载器!");
                return false;
            }
        }

        public void Clear()
        {
            foreach (var loader in loaders.Values)
            {
                loader.Clear();
            }
        }

        public IRegisterResource Get(string regName)
        {
            var key = new RegistryName(regName);
            var loader = GetLoader(key.Head);
            if (loader != null)
            {
                return loader.Get(key);
            }
            else
            {
                Debug.LogError($"在获取 {regName} 时失败: 缺失 {key.Head} 资源加载器!");
                return null;
            }
        }

        public T Get<T>(string regName) where T : IRegisterResource
        {
            var res = Get(regName);
            if (res is T)
            {
                return (T)res;
            }
            Debug.Log($"在获取 {regName} 时失败: 与预期类型不匹配! 预期类型: {typeof(T).FullName} 实际类型: {res.GetType().FullName}");
            return default(T);
        }

        public IRegisterResource[] GetAll(string head)
        {
            var loader = GetLoader(head);
            if (loader != null)
            {
                return loader.GetAll();
            }
            else
            {
                Debug.LogError($"在获取 {head} 类型的资源 时失败: 缺失 {head} 资源加载器!");
                return null;
            }
        }

        public IResourceLoader GetLoader(string head)
        {
            if (loaders.TryGetValue(head, out var loader))
            {
                return loader;
            }
            return null;
        }
    }
}