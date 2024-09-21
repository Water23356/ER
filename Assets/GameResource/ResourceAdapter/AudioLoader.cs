using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dev
{
    [NeededLoader]
    public class AudioLoader : BaseResourceLoader<AudioResource>
    {
        private AudioType m_audioType = AudioType.MPEG;

        public AudioType audioType { get => m_audioType; set => m_audioType = value; }

        public AudioLoader() : base("audio")
        {
        }

        protected override IEnumerator GetRequest(string url, RegistryName regName, Action callback)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, audioType);// 注意限定的
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                var clip = DownloadHandlerAudioClip.GetContent(request);
                UCreateResource(regName, new AudioResource(regName, clip));
            }
            callback?.Invoke();
            request.Dispose();
        }

        protected override void LoadWithAddressable(string url, RegistryName regName, Action callback)
        {
            var handle = Addressables.LoadAssetAsync<AudioClip>(url);
            handle.Completed += (obj) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    AudioResource res = new AudioResource(regName, obj.Result);
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