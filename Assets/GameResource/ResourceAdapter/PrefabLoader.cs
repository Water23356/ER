using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dev
{
    [NeededLoader]
    public class PrefabLoader : BaseResourceLoader<PrefabResource>
    {
        public PrefabLoader() : base("prefab")
        {
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected override IEnumerator GetRequest(string url, RegistryName regName, Action callback)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {

                var content = DownloadHandlerAssetBundle.GetContent(request);
                GameObject prefab = content.LoadAsset<GameObject>(regName.ToString());

                prefab = Instantiate(prefab, transform);//需要提前实例化, 否则释放 content 时会导致数据 prefab 直接丢失
                prefab.SetActive(false);

                UCreateResource(regName,new PrefabResource(regName,prefab));
                content.Unload(false);
            }
            callback?.Invoke(); 
            request.Dispose();

        }

        protected override void LoadWithAddressable(string url, RegistryName regName, Action callback)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(url);
            handle.Completed += (obj) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    PrefabResource res = new PrefabResource(regName, obj.Result);
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
