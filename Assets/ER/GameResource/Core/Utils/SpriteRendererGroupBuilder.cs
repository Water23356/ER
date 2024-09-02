using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Resource
{
    public class SpriteRendererGroupBuilder : MonoBehaviour
    {
        [Serializable]
        public struct SpriteResourcePair
        {
            public SpriteRenderer renderer;
            public string resourceKey;
            [HideInInspector]
            public SpriteResource resource;
        }

        [SerializeField]
        private SpriteResourcePair[] pairs;

        public ExecuteMode executeMode = ExecuteMode.OnAwake;

        private void Awake()
        {
            for(int i=0;i<pairs.Length;i++)
            {
                pairs[i].resource = GR.Get<SpriteResource>(pairs[i].resourceKey);
            }
            if (executeMode == ExecuteMode.OnAwake)
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
            for (int i = 0; i < pairs.Length; i++)
            {
                if (pairs[i].renderer == null) continue;
                if (pairs[i].resource != null)
                {
                    pairs[i].renderer.sprite = pairs[i].resource.Value;
                    pairs[i].renderer.color = pairs[i].resource.Color;
                }
                else
                {
                    Debug.LogWarning($"精灵图资源不存在: {pairs[i].resourceKey}");
                }
            }
            
        }

    }
}