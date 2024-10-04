using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.ResourceManager
{
    [CreateAssetMenu(fileName = "RefDataTable", menuName = "ER/DataTable")]
    public class RefDataTable
        : ScriptableObject
    {
        [HideInInspector]
        public List<RefTableRow> rows = new List<RefTableRow>();
    }
    [Serializable]
    public class RefTableRow
    {
        public string regName;
        public string loadPath;
        public UnityEngine.Object assetReference;
    }
}