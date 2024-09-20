using Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    public abstract class AsyncResourceLoader<T> : MonoBehaviour, IResourceLoader where T : class, IResource
    {
        protected Dictionary<string, LoadLabel> dic = new Dictionary<string, LoadLabel>();//资源缓存 注册名:资源
        protected HashSet<string> force_load = new HashSet<string>();//用于记录被强制加载的资源的注册名

        [SerializeField]
        protected string head = "res";

        public string Head
        {
            get => head;
            set => head = value;
        }

        public void Clear()
        {
            Dictionary<string, LoadLabel> _dic = new Dictionary<string, LoadLabel>();
            foreach (var res in dic)
            {
                if (force_load.Contains(res.Key))
                {
                    dic.Add(res.Key, res.Value);
                }
                else//其他资源被卸载
                {
                    OnUnload(res.Value);
                }
            }
            dic = _dic;
        }

        public void ClearForce()
        {
            foreach (var res in dic)
            {
                OnUnload(res.Value);
            }

            dic.Clear();
        }

        /// <summary>
        /// 当资源卸载时进行处理, 注意回收资源, 默认处理了 Addressable 加载的资源回收, 其他加载模式的回收需要子类实现
        /// </summary>
        /// <param name="res"></param>
        protected virtual void OnUnload(LoadLabel res)
        {
            if (res.loadType == LoadType.ByAddressable)//需要进行卸载
            {
                if (res.handle.IsValid())
                {
                    Addressables.Release(res.handle);
                }
            }
            else if (res.loadType == LoadType.ByUnityRequest)
            {
            }
            else
            {
                Debug.LogError("错误加载枚举");
            }
        }

        public bool Exist(string registryName)
        {
            return dic.ContainsKey(registryName);
        }

        public virtual IResource Get(string registryName)
        {
            if (dic.TryGetValue(registryName, out var res))
            {
                return res.content;
            }
            return null;
        }

        /// <summary>
        /// UnityWebRequest 加载资源封装对象使用该方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="res"></param>
        protected void UCreateResource(string key, T res)
        {
            dic[key] = new LoadLabel()
            {
                content = res,
                loadType = LoadType.ByUnityRequest
            };
        }

        /// <summary>
        /// 使用Addressable 加载资源封装对象使用该方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="res"></param>
        /// <param name="handle"></param>
        protected void ACreateResource(string key, T res, AsyncOperationHandle handle)
        {
            dic[key] = new LoadLabel()
            {
                content = res,
                loadType = LoadType.ByAddressable,
                handle = handle,
            };
        }

        public string[] GetForceResource()
        {
            return force_load.ToArray();
        }

        public void ELoad(string registryName, Action callback, bool skipConvert = false)
        {
            if (!dic.ContainsKey(registryName))
            {
                Load(registryName, callback, skipConvert);
            }
            else
            {
                callback?.Invoke();
            }
        }

        public void Load(string registryName, Action callback, bool skipConvert = false)
        {
            bool defRes;

            string url = registryName;
            if (skipConvert)
            {
                if (url.StartsWith('@'))//@开头标识外部加载
                {
                    url = url.Substring(1);
                    defRes = false;
                }
                else
                {
                    defRes = true;
                }
                //处理注册名, head 使用解析器的 head, 模组使用 erinbone, 路径保持原样
                registryName = $"{head}:erinbone:{url}";
            }
            else
            {
                url = ResourceIndexer.Instance.Convert(registryName, out defRes);
            }
            if (defRes)
            {
                LoadWithAddressable(url, registryName, callback);
            }
            else
            {
                StartCoroutine(GetRequest(url, registryName, callback));
            }
        }

        protected abstract void LoadWithAddressable(string url, string registryName, Action callback);

        protected abstract IEnumerator GetRequest(string url, string registryName, Action callback);

        public void LoadForce(string registryName, Action callback, bool skipConvert = false)
        {
            Load(registryName, callback, skipConvert);
            force_load.Add(registryName);
        }

        public void Unload(string registryName)
        {
            if (dic.ContainsKey(registryName))
            {
                dic.Remove(registryName);
            }
            if (force_load.Contains(registryName))
            {
                force_load.Remove(registryName);
            }
        }

        public IResource[] GetAll()
        {
            IResource[] resources = new IResource[dic.Count];
            int i = 0;
            foreach (var res in dic.Values)
            {
                resources[i] = res.content;
                i++;
            }

            return resources;
        }

        public string[] GetAllRegistryName()
        {
            return dic.Keys.ToArray();
        }

        public void Init()
        {
            GR.AddLoader(this);
        }

        protected enum LoadType
        {
            ByUnityRequest,
            ByAddressable
        }

        protected struct LoadLabel
        {
            public LoadType loadType;
            public T content;
            public AsyncOperationHandle handle;
        }
    }
}