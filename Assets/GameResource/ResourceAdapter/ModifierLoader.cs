using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ER;
using System.Reflection;

namespace Dev
{
    [NeededLoader]
    public class ModifierLoader : BaseResourceLoader<AssetModifyConfigure>
    {
        private Dictionary<string, Type> tableTypes = new Dictionary<string, Type>();

        public ModifierLoader() : base("meta")
        {
        }

        private void Start()
        {
            
        }
        private void FixedMetaTable()
        {
            var atype = typeof(MetaTableAttribute);
            Utils.HandleType(atype, type =>
            {
                var head = type.GetCustomAttribute<MetaTableAttribute>().metaHead;
                tableTypes[head] = type;
            });
        }

        protected override IEnumerator GetRequest(string url, RegistryName regName, Action callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;//外部则以json格式存储, 因此将之转换为原对象
                var type = GetMetaType(regName.Path);
                if (type != null)
                {
                    var obj = ScriptableObject.CreateInstance(type);
                    JsonUtility.FromJsonOverwrite(json, obj);
                    UCreateResource(regName, (AssetModifyConfigure)obj);
                }
                else
                {
                    Debug.LogError($"加载资源失败: {regName}");
                }
            }
            callback?.Invoke();
            request.Dispose();
        }

        protected override void LoadWithAddressable(string url, RegistryName regName, Action callback)
        {
            var type = GetMetaType(regName.Path);
            if (type != null)
            {
                var handle = Addressables.LoadAssetAsync<AssetModifyConfigure>(url);
                handle.Completed += (obj) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        ACreateResource(regName, obj.Result, handle);
                    }
                    else
                    {
                        Debug.LogError($"加载资源失败:{regName}");
                    }
                    callback?.Invoke();
                };
            }
            else
            {
                Debug.LogError($"加载资源失败: {regName}");
            }
           
        }

        private Type GetMetaType(string path)
        {
            var parts = path.Split('/', 2);
            if (tableTypes.TryGetValue(parts[0], out var type))
            {
                return type;
            }
            Debug.LogError($"该元数据类型未注册: {parts[0]}");
            return null;
        }
    }
}