using ER.Save;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 全局uid对象管理器, 需要初始化设定 SavePath (仍需要优化)
    /// </summary>
    public sealed class UIDManager : Singleton<UIDManager>
    {
        /// <summary>
        /// 加载本地UID资源时触发的事件, 使用本接口来实现不同情况下的 资源加载, 本类并不提供具体的 load 反序列化功能
        /// </summary>
        public static event Action<UIDObjectInfo[]> OnLoadEvent;

        private Dictionary<UID, IUIDObject> items = new Dictionary<UID, IUIDObject>();
        /// <summary>
        /// 注册uuid对象
        /// </summary>
        /// <param name="item"></param>
        public void Registry(IUIDObject item)
        {
            Debug.Log($"UID注册:{item.UUID}");
            items[item.UUID]=item;
        }
        public void Clear()
        {
            items.Clear();
        }
        public void Unregistry(IUIDObject item)
        {
            Debug.Log($"UID注销:{item.UUID}");
            if (items.ContainsKey(item.UUID))
            {
                Debug.Log($"UID注销成功:{item.UUID}");
                items.Remove(item.UUID);
            }
                
        }
        public void Unregistry(UID uuid)
        {
            Debug.Log($"UID注销:{uuid}");
            if (items.ContainsKey(uuid))
            {
                Debug.Log($"UID注销成功:{uuid}");
                items.Remove(uuid);
            }
                
        }
        public bool Contains(UID uuid)
        {
            return items.ContainsKey(uuid);
        }
        public bool Contains(IUIDObject item)
        {
            return (items.ContainsKey(item.UUID));
        }
        /// <summary>
        /// 获取指定类型的所有对象
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public IUIDObject[] GetWithClassName(string className)
        {
            List<IUIDObject> uids = new List<IUIDObject>();
            foreach (var pair in items)
            {
                if (pair.Key.ClassName == className)
                {
                    uids.Add(pair.Value);
                }
            }
            return uids.ToArray();
        }
        /// <summary>
        /// 根据uuid获取指定对象
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public IUIDObject Get(UID uuid)
        {
            if (items.TryGetValue(uuid, out IUIDObject item))
            {
                return item;
            }
            Debug.Log($"目标UID对象不存在:{uuid}");
            return null;
        }
        public T Get<T>(UID uuid) where T : class, IUIDObject
        {
            IUIDObject item = Get(uuid);
            if (item == null)
            {
                return null;
            }
            return item as T;
        }
        /// <summary>
        /// 获取所有的UID对象
        /// </summary>
        /// <returns></returns>
        public IUIDObject[] GetAll()
        {
            return items.Values.ToArray();
        }
        /// <summary>
        /// 获取指定类型的所有uid对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IUIDObject[] GetAll(System.Type type)
        {
            List<IUIDObject> uids = new List<IUIDObject>();
            foreach (var v in items.Values)
            {
                if (v.GetType() == type)
                {
                    uids.Add(v);
                }
            }
            return uids.ToArray();
        }
        /// <summary>
        /// 从库中移除指定uuid对象
        /// </summary>
        /// <param name="className"></param>
        public void Remove(string className)
        {
            foreach (var pair in items)
            {
                if (pair.Key.ClassName == className)
                {
                    items.Remove(pair.Key);
                }
            }
        }
        /// <summary>
        /// 从库中移除指定uuid对象
        /// </summary>
        /// <param name="uuid"></param>
        public void Remove(UID uuid)
        {
            if (items.ContainsKey(uuid))
            {
                items.Remove(uuid);
            }
        }
        /// <summary>
        /// 将uid对象持久化存储
        /// </summary>
        public void Save(string savePath)
        {
            List<UIDObjectInfo> datas = new List<UIDObjectInfo>();
            foreach (var item in items)
            {
                UIDObjectInfo data = item.Value.Serialize();
                if (!data.IsEmpty())
                {
                    datas.Add(data);
                }
            }
            string text = JsonConvert.SerializeObject(datas);
            File.WriteAllText(savePath, text);
        }
        /// <summary>
        /// 获取存储信息并复原这些uid对象(需要子类实现)
        /// </summary>
        public void Load(string savePath)
        {
            OnLoadEvent?.Invoke(LoadUIDInfo(savePath));
            //ObjectUIDInfo[] datas = LoadUIDInfo();
            //接下来通过 datas[i] 中的uuid信息判断该 信息属于哪一个 IUID 的派生类,
            //然后使用该类的 Deserialize() 重新复原出该对象
        }
        /// <summary>
        /// 读取储存在本地的uid信息
        /// </summary>
        /// <returns></returns>
        private UIDObjectInfo[] LoadUIDInfo(string savePath)
        {
            string text = File.ReadAllText(savePath);
            UIDObjectInfo[] datas = JsonConvert.DeserializeObject<UIDObjectInfo[]>(text);
            return datas;
        }

        public static void Save(IUIDObject[] uids, string savePath)
        {
            List<UIDObjectInfo> datas = new List<UIDObjectInfo>();
            foreach (var item in uids)
            {
                UIDObjectInfo data = item.Serialize();
                if (!data.IsEmpty())
                {
                    datas.Add(data);
                }
            }
            string text = JsonConvert.SerializeObject(datas);
            File.WriteAllText(savePath, text);
        }

    }
}
