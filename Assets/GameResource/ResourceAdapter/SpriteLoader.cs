using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dev
{
    public class SpriteLoader : BaseResourceLoader<SpriteResource>
    {

        public SpriteLoader() : base("sprite")
        {
        }

        protected override IEnumerator GetRequest(string url, RegistryName regName, Action callback)
        {
            UnityWebRequest request = new UnityWebRequest(url);
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
                    //OnLoaded(registryName, obj.Result);
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
