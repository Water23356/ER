using System;
using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 加载任务
    /// </summary>
    [Serializable]
    public class LoadTask
    {
        public enum ClearMode
        {
            Keep,
            Clear,
            ClearForce
        }

        /// <summary>
        /// 加载情况:  0:load   1:load_force
        /// </summary>
        public LoadProgress progress_load;
        public LoadProgress progress_load_force;
        /// <summary>
        /// 是否清除之前的资源 0:不清除 1:清除 2:强制清除
        /// </summary>
        public ClearMode clear;

        [TextArea(5,20)]
        public string unloads;
        [TextArea(5, 20)]
        public string loads;
        [TextArea(5, 20)]
        public string load_forces;


        public string[] Unload { get => unloads.Split('\n'); }
        public string[] Load { get => loads.Split('\n'); }
        public string[] Load_force { get => load_forces.Split('\n'); }

    }


    public class LoadProgress
    {
        public int loaded;
        public int count;
        public Action callback;
        public bool done;

        public void AddProgress()
        {
            loaded++;
            if (loaded >= count)
            {
                done = true;
                callback?.Invoke();
            }
        }

        public static LoadProgress Done
        {
            get=>new LoadProgress()
            {
                loaded = 0,
                count = 0,
                callback = null,
                done = true
            };
        }
    }
}
