using System;
using UnityEngine;
using ER.ForEditor;
using Dev3;
using System.Collections.Generic;
using System.IO;

namespace Dev
{
    [NeededLoader]
    public class LanguageFileLoader : MonoBehaviour, IResourceLoader
    {
        [ReadOnly]
        [SerializeField]
        private string m_head = "lang";
        public string Head { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Clear()
        {
            GameLocalized.Instance.Clear();
        }

        public IRegisterResource Get(RegistryName regName)
        {
            var keys = GameLocalized.Instance.GetTextFile(regName);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                dic[key] = GLL.GetText(key);
            }
            LanguageFileResource res = new LanguageFileResource(regName,dic);
            return res;
        }

        public IRegisterResource[] GetAll()
        {
            Debug.LogWarning("不要使用该方法获取全部的文本键值对");
            return null;
        }

        public RegistryName[] GetAllRegName()
        {
            Debug.LogWarning("不要使用该方法获取全部的文本键");
            return null;
        }

        public bool IsLoaded(RegistryName regName)
        {
            return GameLocalized.Instance.IsLoaded(regName);
        }

        public void Load(RegistryName regName, Action<IRegisterResource> callback)
        {
            GameLocalized.Instance.LoadTextFile(regName);
            callback?.Invoke(null);
        }

        public void Register(IRegisterResource resource)
        {
            var res = resource as LanguageFileResource;
            if (res !=null)
            {
                GameLocalized.Instance.LoadText(res.registryName,res.GetDic());
            }
        }

        public void UnLoad(RegistryName regName)
        {
            GameLocalized.Instance.UnLoadTextFile(regName);
        }
    }
}