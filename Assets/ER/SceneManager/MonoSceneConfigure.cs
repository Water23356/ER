using UnityEngine;

namespace ER.SceneManager
{
    /// <summary>
    /// 场景初始化配置器, 在加载完场景后执行初始化函数
    /// </summary>
    [CreateAssetMenu(fileName = "CommonSceneConfigure", menuName = "创建场景初始化配置/通用配置", order = 1)]
    public class MonoSceneConfigure : ScriptableObject,ISceneConfigure
    {
        [SerializeField]
        protected string sceneName;

        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get => sceneName; }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            Debug.Log($"{sceneName} 场景加载完毕");
        }
    }
}