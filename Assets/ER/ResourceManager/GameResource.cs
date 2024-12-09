using ER;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ER.ResourceManager
{
    public static class GR
    {
        public static readonly string IGNORE_HEAD = "Assets/res/";

        /// <summary>
        /// 将一个资产地址转为对应的加载地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string ADToKey(string assetAddress)
        {
            int index = assetAddress.IndexOf('.');
            string result = assetAddress;
            if (index >= 0)
            {
                result = result.Substring(0, index).Trim();
            }
            //Debug.Log("地址: " + assetAddress + " 去尾: " + result);
            if (result.StartsWith(IGNORE_HEAD)) //去头
            {
                result = result.Substring(IGNORE_HEAD.Length);
            }
            //Debug.Log("资源加载键: " + result);
            return result;
        }

        /// <summary>
        /// 将资产加载键 转换为 对应的 注册名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string KeyToRegName(string key)
        {
            StringBuilder sb = new StringBuilder();
            int index = key.IndexOf('/');
            if (index < 0)
                return key;
            sb.Append(key.Substring(0, index));
            sb.Append(":origin:");
            sb.Append(key.Substring(index + 1));
            return sb.ToString();
        }

        /// <summary>
        /// 将一个资产地址转为对应的资源名
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string ADToRegName(string assetAddress)
        {
            string key = ADToKey(assetAddress);
            return KeyToRegName(key);
        }

        public static void ELoad(string regName, Action<IRegisterResource> callback = null)
        {
            GameResource.Instance.ELoad(regName, callback);
        }

        public static void ELoad(RegistryName regName, Action<IRegisterResource> callback = null)
        {
            GameResource.Instance.ELoad(regName, callback);
        }

        public static void Load(string regName, Action<IRegisterResource> callback = null)
        {
            GameResource.Instance.Load(regName, callback);
        }

        public static void Load(RegistryName regName, Action<IRegisterResource> callback = null)
        {
            GameResource.Instance.Load(regName, callback);
        }

        public static void UnLoad(string regName)
        {
            GameResource.Instance.UnLoad(regName);
        }

        public static void UnLoad(RegistryName regName)
        {
            GameResource.Instance.UnLoad(regName);
        }

        public static bool IsLoaded(string regName)
        {
            return GameResource.Instance.IsLoaded(regName);
        }

        public static bool IsLoaded(RegistryName regName)
        {
            return GameResource.Instance.IsLoaded(regName);
        }

        public static void Clear()
        {
            GameResource.Instance.Clear();
        }

        public static IRegisterResource Get(string regName)
        {
            return GameResource.Instance.Get(regName);
        }

        public static T Get<T>(string regName) where T : IRegisterResource
        {
            return GameResource.Instance.Get<T>(regName);
        }

        public static T Get<T>(RegistryName regName) where T : IRegisterResource
        {
            return GameResource.Instance.Get<T>(regName);
        }

        public static IRegisterResource[] GetAll(string head)
        {
            return GameResource.Instance.GetAll(head);
        }

        public static IResourceLoader GetLoader(string head)
        {
            return GameResource.Instance.GetLoader(head);
        }
    }

    public class GameResource : MonoBehaviour
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

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void FixedAllResourceLoader()
        {
            Utils.HandleType(typeof(NeededLoaderAttribute), (type) =>
            {
                if (type.IsSubclassOf(typeof(MonoBehaviour)) && type.GetInterfaces().Contains(typeof(IResourceLoader)))
                {
                    var loader = (IResourceLoader)gameObject.AddComponent(type);
                    RegisterLoader(loader);
                }
            });
        }

        public void RegisterLoader(IResourceLoader loader)
        {
            loaders[loader.Head] = loader;
        }

        public void ELoad(string regName, Action<IRegisterResource> callback = null)
        {
            ELoad(new RegistryName(regName), callback);
        }

        public void ELoad(RegistryName regName, Action<IRegisterResource> callback = null)
        {
            var loader = GetLoader(regName.Head);
            if (loader != null)
            {
                if (!loader.IsLoaded(regName))
                    loader.Load(regName, callback);
                else
                    callback?.Invoke(loader.Get(regName));
            }
            else
            {
                Debug.LogError($"在加载 {regName} 时失败: 缺失 {regName.Head} 资源加载器!");
            }
        }

        public void Load(string regName, Action<IRegisterResource> callback = null)
        {
            Load(new RegistryName(regName));
        }

        public void Load(RegistryName regName, Action<IRegisterResource> callback = null)
        {
            var loader = GetLoader(regName.Head);
            if (loader != null)
            {
                loader.Load(regName, callback);
            }
            else
            {
                Debug.LogError($"在加载 {regName} 时失败: 缺失 {regName.Head} 资源加载器!");
            }
        }

        public void UnLoad(string regName)
        {
            UnLoad(new RegistryName(regName));
        }

        public void UnLoad(RegistryName regName)
        {
            var loader = GetLoader(regName.Head);
            if (loader != null)
            {
                loader.UnLoad(regName);
            }
            else
            {
                Debug.LogError($"在卸载 {regName} 时失败: 缺失 {regName.Head} 资源加载器!");
            }
        }

        public bool IsLoaded(string regName)
        {
            return IsLoaded(new RegistryName(regName));
        }

        public bool IsLoaded(RegistryName regName)
        {
            var loader = GetLoader(regName.Head);
            if (loader != null)
            {
                return loader.IsLoaded(regName);
            }
            else
            {
                Debug.LogError($"缺失 {regName.Head} 资源加载器!");
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
            return Get(new RegistryName(regName));
        }

        public IRegisterResource Get(RegistryName regName)
        {
            var loader = GetLoader(regName.Head);
            if (loader != null)
            {
                return loader.Get(regName);
            }
            else
            {
                Debug.LogError($"在获取 {regName} 时失败: 缺失 {regName.Head} 资源加载器!");
                return null;
            }
        }

        public T Get<T>(RegistryName regName) where T : IRegisterResource
        {
            var res = Get(regName);
            if (res is T)
            {
                return (T)res;
            }
            Debug.Log($"在获取 {regName} 时失败: 与预期类型不匹配! 预期类型: {typeof(T).FullName} 实际类型: {res.GetType().FullName}");
            return default(T);
        }

        public T Get<T>(string regName) where T : IRegisterResource
        {
            return Get<T>(new RegistryName(regName));
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

        #region 加载任务处理
        /// <summary>
        /// 当加载任务全部完成后触发, 一次性
        /// </summary>
        public event Action OnTaskListDone;

        private Queue<ResourceLoadTask> tasks = new Queue<ResourceLoadTask>();

        public void StartLoadTask()
        {
            StartCoroutine(ExecuteLoadTask());
        }
        public IEnumerator StartLoadTaskAsync()
        {
            return ExecuteLoadTask();
        }

        private IEnumerator ExecuteLoadTask()
        {
            while (tasks.Count > 0)
            {
                var task = tasks.Dequeue();
                int i = 0;
                while (!task.IsDone)
                {
                    ELoad(task.dic[i], (res) => task.doneCount += 1);
                }

                yield return new WaitUntil(() => task.IsDone);
            }
            var action = OnTaskListDone;
            OnTaskListDone = null;
            action?.Invoke();
            yield return 0;
        }

        public ResourceLoadTask AddLoadDic(MetaDic dic)
        {
            var task = new ResourceLoadTask(dic);
            tasks.Append(task);
            return task;
        }
        public ResourceLoadTask AddLoadDic(string[] dic)
        {
            var task = new ResourceLoadTask(dic);
            tasks.Append(task);
            return task;
        }

        public class ResourceLoadTask
        {
            public int total;
            public int doneCount;
            public string[] dic;

            public bool IsDone
            {
                get => doneCount >= total;
            }

            public ResourceLoadTask(MetaDic dic)
            {
                this.dic = dic.pack;
                total = dic.pack.Length;
                doneCount = 0;
            }
            public ResourceLoadTask(string[] dic)
            {
                this.dic = dic;
                total = dic.Length;
                doneCount = 0;
            }
        }

        #endregion 加载任务处理
    }
}