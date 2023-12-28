using ER.Parser;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace ER
{
    /// <summary>
    /// 声音管理器:
    /// 配置文件:
    ///   path: Assets\config\
    ///   address: config.SoundManager (Addressables)(ini文件)(txt格式)
    /// 音频文件:
    ///   path: Assets\res\audios\
    ///   address: sounds.xxx (xxx为某个具体名称)(clipName)
    /// </summary>
    public class SoundManager: MonoSingleton<SoundManager>
    {
        public GameObject CompositePlayerPrefab;

        private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();//音频缓存

        private INIParser config = new INIParser();//配置字典(路径索引)

        private CompositePlayer bgmPlayer;//背景音乐播放器

        [SerializeField]
        private ObjectPool pool;

        //private List<AudioPlayer> players = new List<AudioPlayer>();//取出的播放器对象

        /// <summary>
        /// 初始化读取配置文件
        /// </summary>
        public void Init()
        {
            TryCreateBGMPlayer();
            Addressables.LoadAssetAsync<TextAsset>("config.SoundManager").Completed += OnLoadConfigureDone;
        }
        private AudioPlayer GetAudioPlayer()
        {
            return (AudioPlayer)pool.GetObject(true);
        }
        private void OnLoadConfigureDone(AsyncOperationHandle<TextAsset> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                //加载配置文件
                TextAsset text = handle.Result;
                config.ParseINIText(text.text);
                Debug.Log("加载音频配置文件成功");
            }
            else
            {
                Debug.LogError("加载音频配置文件失败!");
            }
        }
        /// <summary>
        /// 读取指定音频作为缓存
        /// </summary>
        /// <param name="groupName"></param>
        private void LoadGroup(string groupName)
        {
            Addressables.LoadResourceLocationsAsync(groupName).Completed += (locationHandle) =>
            {
                foreach (var location in locationHandle.Result)
                {
                    Addressables.LoadAssetAsync<AudioClip>(location).Completed += (assetHandle) =>
                    {
                        AudioClip clip = assetHandle.Result;
                    };
                }
            };
        }
        /// <summary>
        /// (异步)加载指定音频资源
        /// </summary>
        /// <param name="clipName"></param>
        public void Load(string clipName)
        {
            Addressables.LoadAssetAsync<AudioClip>(clipName).Completed+=(handle)=>
            {
                AudioClip clip = handle.Result;
                clips[clipName] = clip;
            };
            
        }
        /// <summary>
        /// 尝试在主摄像机上添加BGM播放器
        /// </summary>
        public void TryCreateBGMPlayer()
        {
            Transform player = Camera.main.transform.Find("BGMPlayer");//检测是否存在播放器物体
            if (player == null)
            {
                
                GameObject obj = GameObject.Instantiate(CompositePlayerPrefab, Camera.main.transform);
                obj.name = "BGMPlayer";
                bgmPlayer = obj.GetComponent<CompositePlayer>();
            }
            else
            {
                bgmPlayer = player.GetComponent<CompositePlayer>();
            }
            bgmPlayer.Init();
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBGM()
        {
            bgmPlayer.Play();
        }
        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        public void PauseBGM()
        {
            bgmPlayer.Pause();
        }
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public void StopBGM()
        {
            bgmPlayer.Stop();
        }
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clipName"></param>
        public void PlayBGM(string clipName)
        {
            bgmPlayer.Play(clips[clipName]);
        }
        /// <summary>
        /// 创建一个播放器
        /// </summary>
        /// <param name="clipName">音频资源名称</param>
        /// <param name="playMode">播放模式</param>
        /// <param name="effect">效果</param>
        /// <returns></returns>
        public AudioPlayer CreatePlayer(string clipName,AudioPlayer.PlayMode playMode = AudioPlayer.PlayMode.Single, AudioPlayer.Effect effect = AudioPlayer.Effect.None)
        {
            AudioPlayer player = GetAudioPlayer();
            player.Clip = clips[clipName];
            player.Mode = playMode;
            player.AudioEffect = effect;
            return player;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="clipName"></param>
        public void Play(string clipName)
        {

        }

        public SoundManager() 
        { 
            
        }
    }
}
