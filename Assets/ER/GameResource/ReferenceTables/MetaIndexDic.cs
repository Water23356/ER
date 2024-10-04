using System;
using UnityEngine;

namespace ER.ResourceManager
{
    [CreateAssetMenu(fileName = "MetaIndexDic", menuName = "ER/元/索引表")]
    [MetaTable("index")]
    public class MetaIndexDic: MetaResource
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