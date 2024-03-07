using ER.Template;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 游戏全局静态配置
    /// </summary>
    public class GameConfig:MonoBehaviour,MonoInit
    {
        private static Dictionary<string, object> cfg;
        [Tooltip("游戏全局静态配文件置")]
        [SerializeField]
        private static TextAsset GameConfigText;

        public static Dictionary<string,object> Value
        {
            get=>cfg;
        }
        
        public void Init()
        {
            if (cfg == null)
            {
                string config = GameConfigText.text;
                cfg = JsonConvert.DeserializeObject<Dictionary<string, object>>(config);
            }
        }
    }
}