﻿using ER.ResourceManager;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace ER.SceneJumper
{
    /// <summary>
    /// 场景跳转器
    /// </summary>
    public class SceneJumper : MonoSingleton<SceneJumper>
    {
        [SerializeField]
        private SceneTransition m_transition;

        private SceneJumpInfo jumpInfo;

        public SceneTransition Transition { get => m_transition; set => m_transition = value; }

        public void ToScene(SceneJumpInfo info)
        {
            jumpInfo = info;
            StartCoroutine(TransitionScene());
        }

        private IEnumerator TransitionScene()
        {
            // 1. 淡入动画
            if (Transition != null)
            {
                yield return Transition.PlayStartAsync();
            }

            // 2. 资源依赖检测
            for (int i = 0; i < jumpInfo.depends.Length; i++)
            {
                var depend = jumpInfo.depends[i];
                if (depend != null)
                {
                    var task = GameResource.Instance.AddLoadDic(depend);
                    //绑定状态显示
                    Transition.progress = task;
                    Transition.statusText = jumpInfo.statusText[i];
                    yield return GameResource.Instance.StartLoadTaskAsync();//等待需求资源加载完毕 再加载下一条目
                }
            }

            // 3. 场景初始化
            yield return LoadAnInitScene();

            // 4. 淡出动画
            if (Transition != null)
            {
                yield return Transition.PlayEndAsync();
            }
        }

        private IEnumerator LoadAnInitScene()
        {
            string sceneName = jumpInfo.aimScene.ToString();
            var handler = Addressables.LoadSceneAsync(sceneName, jumpInfo.loadMode);
            //等待场景加载完毕
            yield return new WaitWhile(() => handler.IsDone);
            //获取场景中的初始化组件, 然后完成初始化(异步)
            if (handler.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                var rootObjects = scene.GetRootGameObjects();
                foreach (var item in rootObjects)
                {
                    var initializer = item.GetComponent<SceneInitializer>();
                    if (initializer != null)
                    {
                        yield return initializer.Initialize();
                        break;
                    }
                }
            }
        }
    }

    public struct SceneJumpInfo
    {
        /// <summary>
        /// 加载模式
        /// </summary>
        public LoadSceneMode loadMode;
        /// <summary>
        /// 目标场景的注册名
        /// </summary>
        public RegistryName aimScene;
        /// <summary>
        /// 依赖的资源包, 每个资源包又包含着一系列的资源
        /// </summary>
        public string[][] depends;
        /// <summary>
        /// 加载依赖资源时显示的文本 (相当于资源包的名称)
        /// </summary>
        public string[] statusText;
    }
}