using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    public class AudioLoader : SOLoader<AudioResource>
    {
        public AudioLoader()
        {
            head = "wav";
        }

        //protected override void LoadWithAddressable(string url, string registryName, Action callback)
        //{
        //    var handle = Addressables.LoadAssetAsync<AudioClip>(url);
        //    handle.Completed += (obj) =>
        //    {
        //        if (obj.Status == AsyncOperationStatus.Succeeded)
        //        {
        //            ACreateResource(registryName, new AudioResource(registryName, obj.Result), handle);
        //        }
        //        else
        //        {
        //            Debug.LogError($"加载资源失败:{registryName}");
        //        }
        //        callback?.Invoke();
        //    };
        //}

        //protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        //{
        //    UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
        //    yield return request.SendWebRequest();
        //    if (request.result == UnityWebRequest.Result.Success)
        //    {
        //        UCreateResource(registryName, new AudioResource(registryName, DownloadHandlerAudioClip.GetContent(request)));
        //    }
        //    else
        //    {
        //        Debug.LogError($"加载资源失败:{registryName}");
        //    }
        //    callback?.Invoke();
        //}
    }
}