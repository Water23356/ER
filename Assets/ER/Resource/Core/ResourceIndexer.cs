﻿using ER.Parser;
using ER.Template;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    /// <summary>
    /// 资源索引器, 将资源注册名 转化为 地址或者url
    /// 外部资源使用@作为前缀标识
    /// </summary>
    public class ResourceIndexer:MonoSingleton<ResourceIndexer>,MonoInit
    {
        /// <summary>
        /// 自定义资源加载配置文件路径
        /// </summary>
        public static string custom_config_path => ERinbone.Combine(ERinbone.ConfigPath,"res_indexer.ini");


        private Dictionary<string, string> dic=new Dictionary<string, string>();
        /// <summary>
        /// 重新初始化资源索引器
        /// </summary>
        public void Init()
        {
            dic.Clear();
        }
        /// <summary>
        /// 加载默认索引器, 并根据预设额外加载资源索引器
        /// </summary>
        /// <param name="handle"></param>
        private void OnLoadConfigureDone(AsyncOperationHandle<TextAsset> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                //加载配置文件
                TextAsset text = handle.Result;
                INIParser config = new INIParser();
                config.ParseINIText(text.text);
                Dictionary<string, string> _dic = config.GetSection("indexer");
                Debug.Log("字典有效:"+_dic!=null);
                foreach(KeyValuePair<string, string> pair in _dic)
                {
                    dic[pair.Key] = pair.Value;
                }
                //加载自定义配置文件
                config.Clear();
                config.ParseINIFile(custom_config_path);
                Dictionary<string,string> resources_cover = config.GetSection("url");//覆盖的 资源包 url 资源包名称(id):资源配置文件url
                foreach(var url in resources_cover)
                {
                    Load(url.Value,ResourcePack.IndexerFileName);//这里需要转化为 indexer 文件路径才能加载
                }
                Debug.Log("加载默认资源配置文件成功");
            }
            else
            {
                Debug.LogError("加载默认资源配置文件失败!");
            }
            MonoLoader.InitCallback();
        }
        /// <summary>
        /// 加载索引器
        /// </summary>
        private void Load(string pack_url,string url)
        {
            string path = ERinbone.Combine(pack_url, url);
            if (!File.Exists(path))
            {
                Debug.Log("加载资源配置文件出错: 无效url:");
                return;
            }
            INIParser parser = new INIParser();
            parser.ParseINIFile(path);

            Dictionary<string, string> _dic = parser.GetSection("indexer");
            foreach (KeyValuePair<string, string> pair in _dic)
            {
                dic[pair.Key] = ERinbone.Combine(pack_url, pair.Value);
                Debug.Log("字典键值对: "+ dic[pair.Key]);
            }
        }
        /// <summary>
        /// 访问和修改 键与url 的映射
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get => dic[key];
            set => dic[key] =value;
        }
        public bool TryGetURL(string key,out string url)
        {
            if(dic.TryGetValue(key,out url))
            {
                return true;
            }
            url = null;
            return false;
        }
    }
}