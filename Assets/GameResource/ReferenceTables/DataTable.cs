using System.Collections.Generic;
using UnityEngine;

namespace Dev
{
    [CreateAssetMenu(fileName = "RefDataTable", menuName = "ER/DataTable")]
    public class RefDataTable
        : ScriptableObject
    {
        public List<RefTableRow> rows = new List<RefTableRow>();
    }
}