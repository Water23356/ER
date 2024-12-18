﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.ResourceManager
{
    [NeededLoader]
    public class TextLoader : BaseResourceLoader<TextResource>
    {
        public TextLoader() : base("text")
        {
        }

        protected override IEnumerator GetRequest(string url, RegistryName regName, Action<IRegisterResource> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                callback?.Invoke(null);
            }
            else
            {
                string text = request.downloadHandler.text;
                var res = new TextResource(regName, text);
                UCreateResource(regName, res);
                callback?.Invoke(res);
            }
            request.Dispose();
        }

        protected override void LoadWithAddressable(string url, RegistryName regName, Action<IRegisterResource> callback)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(url);
            handle.Completed += (obj) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    TextResource res = new TextResource(regName, obj.Result.text);
                    ACreateResource(regName, res, handle);
                    callback?.Invoke(res);
                }
                else
                {
                    Debug.LogError($"加载资源失败:{regName}");
                    callback?.Invoke(null);
                }
            };
        }
    }
}