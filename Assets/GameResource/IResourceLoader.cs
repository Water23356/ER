using System;

namespace Dev
{
    /// <summary>
    /// 资源加载器
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// 资源头
        /// </summary>
        public string Head { get; set; }

        /// <summary>
        /// 注册新的资源(动态注册)
        /// </summary>
        /// <param name="resource"></param>
        public void Register(IRegisterResource resource);

        /// <summary>
        /// 加载指定资源
        /// </summary>
        /// <param name="regName">资源注册名</param>
        /// <param name="callback">加载完毕后的回调</param>
        /// <param name="skipConvert">是否跳过url重定向</param>
        public void Load(RegistryName regName, Action callback, bool skipConvert = false);

        /// <summary>
        /// 卸载该类型的所有资源缓存
        /// </summary>
        public void Clear();

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="regName"></param>
        public void UnLoad(RegistryName regName);

        /// <summary>
        /// 判断指定资源是否已经被加载
        /// </summary>
        /// <param name="regName"></param>
        /// <returns></returns>
        public bool IsLoaded(RegistryName regName);

        /// <summary>
        /// 取得指定资源
        /// </summary>
        /// <param name="regName"></param>
        /// <returns></returns>
        public IRegisterResource Get(RegistryName regName);

        /// <summary>
        /// 获取所有已经被加载的资源
        /// </summary>
        /// <returns></returns>
        public IRegisterResource[] GetAll();

        /// <summary>
        /// 获取所有已经被加载的资源名称
        /// </summary>
        /// <returns></returns>
        public string[] GetAllRegName();
    }
}