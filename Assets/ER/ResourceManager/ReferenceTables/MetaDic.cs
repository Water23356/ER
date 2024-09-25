using ER.ForEditor;
using System;
using UnityEngine;

namespace ER.ResourceManager
{
    [CreateAssetMenu(fileName = "MetaDic", menuName = "ER/元/资源表")]
    [MetaTable("dic")]
    public class MetaDic : AssetModifyConfigure
    {
        public override string metaHead => "dic";
        [DisplayLabel("包含")]
        [GetFromRegistryName]
        public string[] pack;

        public override T Get<T>()
        {
            return default(T);
        }
    }
}