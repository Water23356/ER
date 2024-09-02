namespace ER.SceneManager
{
    /// <summary>
    /// 场景初始化配置器, 在加载完场景后执行初始化函数
    /// </summary>
    public interface ISceneConfigure
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Initialize();
    }
}