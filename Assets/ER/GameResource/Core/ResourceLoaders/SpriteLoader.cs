using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    public class SpriteLoader : SOLoader<SpriteResource>
    {
        public SpriteLoader()
        {
            head = "img";
        }


        //protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        //{
        //    UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        //    yield return request.SendWebRequest();
        //    if (request.result == UnityWebRequest.Result.Success)
        //    {
        //        UCreateResource(registryName, new SpriteResource(registryName, DownloadHandlerTexture.GetContent(request).TextureToSprite()));
        //    }
        //    else
        //    {
        //        Debug.LogError($"加载资源失败:{registryName}");
        //    }
        //    callback?.Invoke();
        //}

        //protected override void LoadWithAddressable(string url, string registryName, Action callback)
        //{
        //    var handle = Addressables.LoadAssetAsync<Texture2D>(url);
        //    handle.Completed += (obj) =>
        //    {
        //        if (obj.Status == AsyncOperationStatus.Succeeded)
        //        {
        //            ACreateResource(registryName, new SpriteResource(registryName, obj.Result.TextureToSprite()), handle);
        //        }
        //        else
        //        {
        //            Debug.LogError($"加载资源失败:{registryName}");
        //        }
        //        callback?.Invoke();
        //    };
        //}
    }
}