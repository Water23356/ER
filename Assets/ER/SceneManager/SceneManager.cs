using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ER.SceneManager
{
    ///场景管理器
    public sealed class SceneManager : MonoSingleton<SceneManager>
    {

        private Dictionary<string, ISceneConfigure> scenes = new();

        /// <summary>
        /// 加载场景; 自动销毁旧场景
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="asyncLoad"></param>
        public void LoadScene(string sceneName, SceneTransition transition = null, bool asyncLoad = false, Action callback = null)
        {
            //异步加载
            if (asyncLoad)
            {
              
                StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, transition,callback));
            }
            else
            {
                
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                if (transition != null)
                {
                    transition.Progress = 1f;
                }
                callback?.Invoke();
            }
        }

        /// <summary>
        /// 加载场景; 自动销毁旧场景
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="asyncLoad"></param>
        public void LoadScene(ISceneConfigure scene, SceneTransition transition = null, bool asyncLoad = false, Action callback = null)
        {
            //异步加载
            if (asyncLoad)
            {
                scenes[scene.SceneName] = scene;
                
                StartCoroutine(LoadSceneAsync(scene.SceneName, LoadSceneMode.Single, transition,callback));
            }
            else
            {
                
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName, LoadSceneMode.Single);
                if (transition != null)
                {
                    transition.Progress = 1f;
                }
                scene.Initialize();
                callback?.Invoke();
            }
        }

        /// <summary>
        /// 叠加场景;保留旧场景
        /// </summary>
        public void CoverScene(string sceneName, SceneTransition transition = null, bool asyncLoad = false, Action callback = null)
        {
            //异步加载
            if (asyncLoad)
            {
                
                StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, transition,callback));
            }
            else
            {
               
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                if (transition != null)
                {
                    transition.Progress = 1f;
                }
                callback?.Invoke();   
            }
        }

        /// <summary>
        /// 叠加场景;保留旧场景
        /// </summary>
        public void CoverScene(ISceneConfigure scene, SceneTransition transition = null, bool asyncLoad = false, Action callback = null)
        {
            //异步加载
            if (asyncLoad)
            {
                scenes[scene.SceneName] = scene;
                StartCoroutine(LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive, transition,callback));
            }
            else
            {
             
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName, LoadSceneMode.Additive);
                if (transition != null)
                {
                    transition.Progress = 1f;
                }
                scene.Initialize();
                callback?.Invoke();
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="transition"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, SceneTransition transition = null,Action callback = null)
        {
            var opt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadMode);
            while (!opt.isDone)//场景是否加载完毕
            {
                //同步加载进度给场景过渡类
                if (transition != null)
                {
                    transition.Progress = opt.progress;
                    yield return null;
                }
            }
            if (transition != null)
            {
                transition.Progress = 1f;
            }
            scenes[sceneName].Initialize();
            callback?.Invoke();
        }
    }
}