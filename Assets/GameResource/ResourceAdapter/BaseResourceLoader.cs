using ER.ForEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dev
{
    public abstract class BaseResourceLoader<T> : MonoBehaviour, IResourceLoader where T : IRegisterResource
    {
        [ReadOnly]
        [SerializeField]
        private string m_head = "base";

        [SerializeField]
        [DisplayLabel("默认资源")]
        private RegistryName defaultRegName = new RegistryName("base:default");//默认资源名

        private T defaultResource;//默认资源

        private Dictionary<RegistryName, LoadLabel> dic = new Dictionary<RegistryName, LoadLabel>();
        public string Head { get => m_head; set => m_head = value; }
        public RegistryName DefaultRegName { get => defaultRegName; set => defaultRegName = value; }

        /// <summary>
        /// 默认资源(加载后永远不会被卸载)
        /// </summary>
        public T Default { get => defaultResource; }

        public BaseResourceLoader(string head)
        { m_head = head; defaultRegName.Head = head; }

        public void Clear()
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

        public virtual IRegisterResource Get(RegistryName regName)
        {
            if (dic.TryGetValue(regName, out var res))
            {
                return res.content;
            }
            return Default;
        }

        public IRegisterResource[] GetAll()
        {
            IRegisterResource[] resources = new IRegisterResource[dic.Count];
            int i = 0;
            foreach (var res in dic.Values)
            {
                resources[i] = res.content;
                i++;
            }

            return resources;
        }

        public RegistryName[] GetAllRegName()
        {
            return dic.Keys.ToArray();
        }

        public bool IsLoaded(RegistryName regName)
        {
            return dic.ContainsKey(regName);
        }

        public void Load(RegistryName regName, Action<IRegisterResource> callback)
        {
            var path = ResourceIndexer.Convert(regName);
            if (path.state == ResourceIndexer.LoadURL.State.Error)
            {
                Debug.Log($"资产适配器错误: 在加载 {regName} 时出错!");
            }
            switch (path.state)
            {
                case ResourceIndexer.LoadURL.State.FromAddressable:
                    LoadWithAddressable(path.url, regName, callback);
                    break;

                case ResourceIndexer.LoadURL.State.FromWeb:
                    StartCoroutine(GetRequest(path.url, regName, callback));
                    break;

                case ResourceIndexer.LoadURL.State.Error:
                default:
                    Debug.Log($"资产适配器错误: 在加载 {regName} 时出错!");
                    break;
            }
        }

        public void Register(IRegisterResource resource)
        {
            if (resource is T)
            {
                dic[resource.registryName] = new LoadLabel()
                {
                    content = (T)resource,
                    loadType = LoadType.ByRegister
                };
            }
            else
            {
                Debug.LogError($"注册时资源失败: {resource.registryName}, 非目标类型加载器: 加载类型: {typeof(T).FullName} 资源类型: {resource.GetType().FullName}");
            }
        }

        /// <summary>
        /// UnityWebRequest 加载资源封装对象使用该方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="res"></param>
        protected void UCreateResource(RegistryName key, T res)
        {
            if (defaultResource == null && key == defaultRegName)
            {
                defaultResource = res;
            }
            else
            {
                dic[key] = new LoadLabel()
                {
                    content = res,
                    loadType = LoadType.ByUnityRequest
                };
            }
        }

        /// <summary>
        /// 使用Addressable 加载资源封装对象使用该方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="res"></param>
        /// <param name="handle"></param>
        protected void ACreateResource(RegistryName key, T res, AsyncOperationHandle handle)
        {
            if (defaultResource == null && key == defaultRegName)
            {
                defaultResource = res;
            }
            else
            {
                dic[key] = new LoadLabel()
                {
                    content = res,
                    loadType = LoadType.ByAddressable,
                    handle = handle,
                };
            }
        }

        protected abstract void LoadWithAddressable(string url, RegistryName regName, Action<IRegisterResource> callback);

        protected abstract IEnumerator GetRequest(string url, RegistryName regName, Action<IRegisterResource> callback);

        public void UnLoad(RegistryName regName)
        {
            if (dic.TryGetValue(regName, out var value))
            {
                OnUnload(value);
            }
        }

        protected enum LoadType
        {
            ByUnityRequest,
            ByAddressable,
            ByRegister
        }

        protected struct LoadLabel
        {
            public LoadType loadType;
            public T content;
            public AsyncOperationHandle handle;
        }
    }
}