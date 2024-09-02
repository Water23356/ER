using ER.ForEditor;
using UnityEngine;

namespace ER.Resource
{
    public enum ExecuteMode
    {
        /// <summary>
        /// 无效
        /// </summary>
        None,
        /// <summary>
        /// Awake激活替换
        /// </summary>
        OnAwake,
        /// <summary>
        /// Start激活替换
        /// </summary>
        OnStart,
        /// <summary>
        /// OnEnable激活替换
        /// </summary>
        OnEnable,

    }
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererBuilder:MonoBehaviour
    {
    
        private SpriteRenderer spriteRenderer;
        private SpriteResource spriteResource;
        [DisplayLabel("资源注册名")]
        [SerializeField]
        private string assetName;
        public ExecuteMode executeMode = ExecuteMode.OnAwake;


        public string AssetName
        {
            get => assetName;
            set
            {
                assetName = value;
                spriteResource = GR.Get<SpriteResource>(assetName);
            }
        }
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteResource = GR.Get<SpriteResource>(assetName);
            if(executeMode == ExecuteMode.OnAwake)
                UpdateSprite();
        }

        private void Start()
        {
            if (executeMode == ExecuteMode.OnStart)
                UpdateSprite();
        }
        private void OnEnable()
        {
            if (executeMode == ExecuteMode.OnEnable)
                UpdateSprite();
        }

        public void UpdateSprite()
        {
            if (spriteResource != null)
            {
                spriteRenderer.sprite = spriteResource.Value;
                spriteRenderer.color = spriteResource.Color;
            }
            else
            {
                Debug.LogWarning($"精灵图资源不存在: {assetName}");
            }
        }
    }
}