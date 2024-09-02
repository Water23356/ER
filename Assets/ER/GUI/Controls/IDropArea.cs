using UnityEngine.EventSystems;
namespace ER.GUI
{
    /// <summary>
    /// 放置区域
    /// </summary>
    public interface IDropArea : IDropHandler
    {
        /// <summary>
        /// 区域标识符
        /// </summary>
        public string AreaName { get; set; }
    }
}