using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dev
{
    [CreateAssetMenu(fileName = "MetaSprite", menuName = "ER/元/索引表")]
    [MetaTable("index")]
    public class MetaIndexDic: AssetModifyConfigure
    {
        public override string metaHead => "index";
        public Row[] indexes;

        public override T Get<T>()
        {
            return default(T);
        }

        [Serializable]
        public struct Row
        {
            public string regName;
            public string loadPath;
        }
    }
}