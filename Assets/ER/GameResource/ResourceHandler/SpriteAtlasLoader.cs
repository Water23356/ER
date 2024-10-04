using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace ER.ResourceManager
{
    [NeededLoader]
    public class SpriteAtlasLoader : BaseResourceLoader<SpriteAtlasResource>
    {
        public SpriteAtlasLoader() : base("atlas")
        {
        }

        protected override IEnumerator GetRequest(string url, RegistryName regName, Action<IRegisterResource> callback)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                callback?.Invoke(null);
            }
            else
            {
                var content = DownloadHandlerAssetBundle.GetContent(request);
                var atlas = content.LoadAsset<SpriteAtlas>(regName.ToString());
                var res = new SpriteAtlasResource(regName, atlas);
                UCreateResource(regName, res);
                content.Unload(false);
                callback?.Invoke(res);
            }
            request.Dispose();
        }

        protected override void LoadWithAddressable(string url, RegistryName regName, Action<IRegisterResource> callback)
        {
            var handle = Addressables.LoadAssetAsync<SpriteAtlas>(url);
            handle.Completed += (obj) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    SpriteAtlasResource res = new SpriteAtlasResource(regName, obj.Result);
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