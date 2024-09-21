using ER.GUI;
using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 预制体资源
    /// </summary>
    [CreateAssetMenu(fileName = "GUIPanelResource", menuName = "创建可配置资产/GUI面板资产", order = 5)]
    public class GUIPanelResource : BaseAssetConfigure
    {
        private GameObject old_prfb = null;

        [SerializeField]
        private GameObject prfb;

        [Header("面板初始化")]
        public Vector2 anchoredPosition;
        public Vector2 offsetMin;
        public Vector2 offsetMax;
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public int sort;
        public GUIPanel.GUILayer layer; 

        /// <summary>
        /// 预制体资源对象
        /// </summary>
        public GameObject Value => prfb;

        public static explicit operator GameObject(GUIPanelResource source)
        {
            return source.Value;
        }

        public GUIPanelResource(string _registryName, GameObject origin)
        {
            prfb = origin;
            registryName = _registryName;
        }

        /// <summary>
        /// 获取该预制体的拷贝物体对象
        /// </summary>
        public GameObject Copy
        {
            get
            {
                return GameObject.Instantiate(prfb);
            }
        }

        private void OnValidate()
        {
            if (prfb != old_prfb)
            {
                old_prfb = prfb;
                var rectt = prfb.GetComponent<RectTransform>();
                anchoredPosition = rectt.anchoredPosition;
                offsetMin = rectt.offsetMin; offsetMax = rectt.offsetMax;
                anchorMax = rectt.anchorMax; anchorMin = rectt.anchorMin;
            }
        }
    }
}