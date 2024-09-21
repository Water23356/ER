using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dev
{
    [NeededLoader]
    public class SpriteLoader : BaseResourceLoader<SpriteResource>
    {
        public SpriteLoader() : base("sprite")
        {
        }

        protected override IEnumerator GetRequest(string url, RegistryName regName, Action callback)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                UCreateResource(regName,new SpriteResource(regName,sprite));
            }
            callback?.Invoke();
            request.Dispose();
        }

        protected override void LoadWithAddressable(string url, RegistryName regName, Action callback)
        {
            var handle = Addressables.LoadAssetAsync<Sprite>(url);
            handle.Completed += (obj) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    SpriteResource res = new SpriteResource(regName, obj.Result);
                    ACreateResource(regName, res, handle);
                }
                else
                {
                    Debug.LogError($"加载资源失败:{regName}");
                }
                callback?.Invoke();
            };
        }
    }
}
