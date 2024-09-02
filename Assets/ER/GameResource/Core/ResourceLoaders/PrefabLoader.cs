using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    public class PrefabLoader : SOLoader<PrefabResource>
    {
        public PrefabLoader() 
        {
            head = "prfb";
        }
        //protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        //{
        //    Debug.LogError("暂不支持加载外部的预制体资源");
        //    return null;
        //}

        //protected override void LoadWithAddressable(string url, string registryName, Action callback)
        //{
        //    var handle = Addressables.InstantiateAsync(url);
        //    handle.Completed += (obj) =>
        //    {
        //        if (obj.Status == AsyncOperationStatus.Succeeded)
        //        {
        //            GameObject pfb = Instantiate(obj.Result);
        //            pfb.transform.SetParent(transform);
        //            pfb.SetActive(false);


        //            var res = new PrefabResource(registryName, pfb);
        //            ACreateResource(registryName, res, handle);

        //            Debug.Log($"加载资源成功:{registryName}");
        //        }
        //        else
        //        {
        //            Debug.LogError($"加载资源失败:{registryName}");
        //        }
        //        Debug.Log($"callback:{callback != null} {callback}");
        //        callback?.Invoke();
        //    }; 
        //}
    }
}