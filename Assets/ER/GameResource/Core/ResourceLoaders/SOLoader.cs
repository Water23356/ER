using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    /// <summary>
    /// 用于加载ScriptableObject 类型的资产
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SOLoader<T> : AsyncResourceLoader<T> where T : BaseAssetConfigure
    {
        protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;//外部则以json格式存储, 因此将之转换为原对象
                var obj = ScriptableObject.CreateInstance<T>();
                JsonUtility.FromJsonOverwrite(json, obj);
                UCreateResource(registryName, obj);
                OnLoaded(registryName,obj);
            }
            else
            {
                Debug.LogError($"加载资源失败:{registryName}");
            }
            callback?.Invoke();
        }

        protected override void LoadWithAddressable(string url, string registryName, Action callback)
        {
            //Debug.Log($"url: {url}");
            var handle = Addressables.LoadAssetAsync<T>(url);
            handle.Completed += (obj) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    ACreateResource(registryName, obj.Result, handle);
                    OnLoaded(registryName, obj.Result);
                }
                else
                {
                    Debug.LogError($"加载资源失败:{registryName}");
                }
                callback?.Invoke();
            }; 

        }

        /// <summary>
        /// 当资源完成加载时触发
        /// </summary>
        /// <param name="key"></param>
        protected virtual void OnLoaded(string key,T obj)
        {

        }
    }

}