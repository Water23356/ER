﻿using ER.ResourceManager;
using ER.ForEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ER.SceneJumper
{
    [CreateAssetMenu(fileName = "MetaMetaSceneJumpDic", menuName = "ER/元/场景加载")]
    [MetaTable("scene")]
    public class MetaSceneJump : AssetModifyConfigure
    {
        public override string metaHead => "scene";

        [DisplayLabel("加载模式")]
        public LoadSceneMode loadMode;

        [DisplayLabel("依赖包")]
        public MetaDic[] depends;

        [DisplayLabel("状态文本")]
        public string[] statusText;

        public override T Get<T>()
        {
            return default(T);
        }

        public SceneJumpInfo GetInfo()
        {
            return new SceneJumpInfo
            {
                loadMode = loadMode,
                depends = depends,
                aimScene = registryName,
                statusText = statusText
            };
        }
    }
}