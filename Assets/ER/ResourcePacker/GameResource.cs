using ER.Parser;
using ER.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace ER.ResourcePacker
{
    /// <summary>
    /// 游戏资源: 用于 管理 和 缓存 游戏所需要的各种资源
    /// #1: 使用资源键 访问指定资源
    /// </summary>
    public class GameResource : MonoSingleton<GameResource>
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        public enum ResourceType
        {
            /// <summary>
            /// 文本资源
            /// </summary>
            Text,
            /// <summary>
            /// 图片资源
            /// </summary>
            Sprite,
            /// <summary>
            /// 音频资源
            /// </summary>
            AudioClip,
            /// <summary>
            /// 任意资源
            /// </summary>
            Any,
        }

        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        private Dictionary<string, string> texts = new Dictionary<string, string>();
        private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

        private List<LoadProgress> loadProgresses = new List<LoadProgress>();

        /// <summary>
        /// 卸载资源缓存
        /// </summary>
        /// <param name="keys"></param>
        public void Unload(params string[] keys)
        {
            foreach (string key in keys)
            {
                if (texts.ContainsKey(key))
                {
                    texts.Remove(key);
                }
                if (sprites.ContainsKey(key))
                {
                    sprites.Remove(key);
                }
                if (clips.ContainsKey(key))
                {
                    clips.Remove(key);
                }
            }
        }
        /// <summary>
        /// 加载资源缓存
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback">回调函数, 资源加载完毕触发</param>
        /// <param name="keys"></param>
        public void Load(ResourceType type, Action callback = null, params string[] keys)
        {
            int progressIndex = -1;
            if(callback!= null)
            {
                progressIndex = loadProgresses.Count;
                loadProgresses.Add(new LoadProgress
                {
                    loaded = 0,
                    count = keys.Length,
                    callback = callback,
                    done = false,
                });
            }
            switch (type)
            {
                case ResourceType.Text:
                    foreach (var key in keys)
                    {
                        if (ResourceIndexer.Instance.TryGetURL(key, out string url))
                        {
                            if (url.StartsWith('@'))//@开头的url表示游戏内部资源, 使用 Addressables 加载
                            {
                                Addressables.LoadAssetAsync<TextAsset>("url").Completed += (handle) =>
                                {
                                    if (handle.Status == AsyncOperationStatus.Succeeded)
                                    {
                                        texts[key] = handle.Result.text;
                                        if(progressIndex>-1 && progressIndex < loadProgresses.Count)
                                        {
                                            loadProgresses[progressIndex].AddProgress();
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("加载默认资源失败!");
                                    }
                                };
                            }
                            else// 外部资源使用特殊方法加载
                            {
                                StartCoroutine(LoadFileText(key, url, () =>
                                {
                                    if (progressIndex > -1 && progressIndex < loadProgresses.Count)
                                    {
                                        loadProgresses[progressIndex].AddProgress();
                                    }
                                }));
                            }
                        }
                    }
                    break;
                case ResourceType.Sprite:
                    foreach (var key in keys)
                    {
                        if (ResourceIndexer.Instance.TryGetURL(key, out string url))
                        {
                            if (url.StartsWith('@'))//@开头的url表示游戏内部资源, 使用 Addressables 加载
                            {
                                Addressables.LoadAssetAsync<Texture2D>("url").Completed += (handle) =>
                                {
                                    if (handle.Status == AsyncOperationStatus.Succeeded)
                                    {
                                        sprites[key] = handle.Result.TextureToSprite();
                                        if (progressIndex > -1 && progressIndex < loadProgresses.Count)
                                        {
                                            loadProgresses[progressIndex].AddProgress();
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("加载默认资源失败!");
                                    }
                                };
                            }
                            else// 外部资源使用特殊方法加载
                            {
                                StartCoroutine(LoadFileSprite(key, url, () =>
                                {
                                    if (progressIndex > -1 && progressIndex < loadProgresses.Count)
                                    {
                                        loadProgresses[progressIndex].AddProgress();
                                    }
                                }));
                            }
                        }
                    }
                    break;
                case ResourceType.AudioClip:
                    foreach (var key in keys)
                    {
                        if (ResourceIndexer.Instance.TryGetURL(key, out string url))
                        {
                            if (url.StartsWith('@'))//@开头的url表示游戏内部资源, 使用 Addressables 加载
                            {
                                Addressables.LoadAssetAsync<AudioClip>("url").Completed += (handle) =>
                                {
                                    if (handle.Status == AsyncOperationStatus.Succeeded)
                                    {
                                        clips[key] = handle.Result;
                                        if (progressIndex > -1 && progressIndex < loadProgresses.Count)
                                        {
                                            loadProgresses[progressIndex].AddProgress();
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("加载默认资源失败!");
                                    }
                                };
                            }
                            else// 外部资源使用特殊方法加载
                            {
                                if (url.EndsWith(".wav"))
                                {
                                    StartCoroutine(LoadFileAudioClip(key, url, AudioType.WAV, ()=>
                                    {
                                        if (progressIndex > -1 && progressIndex < loadProgresses.Count)
                                        {
                                            loadProgresses[progressIndex].AddProgress();
                                        }
                                    }));
                                }
                                else if (url.EndsWith(".ogg"))
                                {
                                    StartCoroutine(LoadFileAudioClip(key, url, AudioType.OGGVORBIS, () => 
                                    {
                                        if (progressIndex > -1 && progressIndex < loadProgresses.Count)
                                        {
                                            loadProgresses[progressIndex].AddProgress(); 
                                        } 
                                    }));
                                }
                            }
                        }
                    }
                    break;
                default:
                    Debug.LogError("错误资源类型");
                    break;
            }
        }

        /// <summary>
        /// 加载文本资源
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private IEnumerator LoadFileText(string key, string url, Action callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                string text = request.downloadHandler.text;
                texts[key] = text;
            }
            else
            {
                Debug.Log(request.error);
            }
            callback?.Invoke();
        }
        /// <summary>
        /// 加载音频资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="audioType"></param>
        /// <returns></returns>
        private IEnumerator LoadFileAudioClip(string key, string url, AudioType audioType, Action callback)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, audioType);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip audio = DownloadHandlerAudioClip.GetContent(request);
                clips[key] = audio;
            }
            else
            {
                Debug.Log(request.error);
            }
            callback?.Invoke();
        }
        /// <summary>
        /// 加载图片资源
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadFileSprite(string key, string url, Action callback)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D tt = DownloadHandlerTexture.GetContent(request);
                sprites[key] = tt.TextureToSprite();
            }
            else
            {
                Debug.Log(request.error);
            }
            callback?.Invoke();
        }
        /// <summary>
        /// 获取指定sprite资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Sprite GetSprite(string key)
        {
            if (sprites.TryGetValue(key, out Sprite sp))
            {
                return sp;
            }
            return null;
        }
        /// <summary>
        /// 获取指定文本资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(string key)
        {
            if (texts.TryGetValue(key, out string st))
            {
                return st;
            }
            return null;
        }
        /// <summary>
        /// 获取指定音频资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AudioClip GetAudioClip(string key)
        {
            if (clips.TryGetValue(key, out AudioClip ac))
            {
                return ac;
            }
            return null;
        }
        /// <summary>
        /// 判断指定资源是否已经加载
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool LoadedExist(string key, ResourceType type = ResourceType.Any)
        {
            switch (type)
            {
                case ResourceType.Text:
                    return texts.ContainsKey(key);
                case ResourceType.Sprite:
                    return sprites.ContainsKey(key);
                case ResourceType.AudioClip:
                    return clips.ContainsKey(key);
                case ResourceType.Any:
                default:
                    return texts.ContainsKey(key) || sprites.ContainsKey(key) || clips.ContainsKey(key);
            }
        }
    }

    public struct LoadProgress
    {
        public int loaded;
        public int count;
        public Action callback;
        public bool done;

        public void AddProgress()
        {
            loaded ++;
            if(loaded>=count)
            {
                done = true;
                callback?.Invoke();
            }
        }
    }

}