using ER;
using System.Collections.Generic;
using UnityEngine;

namespace ER.GUI
{
    /// <summary>
    /// UI控件排序机(挂在父物体上)
    /// </summary>
    public class ControlSorter : MonoBehaviour
    {
        private PriorityQueue<IControlSortItem> sortItems = new PriorityQueue<IControlSortItem>(Comparer);

        public void Refresh()
        {
            sortItems.Refresh();
        }

        public void Clear()
        {
            sortItems.Clear();
        }

        public void AddControl(IControlSortItem item)
        {
            sortItems.Add(item);
        }

        private static bool Comparer(IControlSortItem a, IControlSortItem b)
        {
            return a.Sort <= b.Sort;
        }

        public void UpdateSort()
        {
            int i = 0;
            foreach (IControlSortItem item in sortItems)
            {
                item.Transform.SetSiblingIndex(i++);
            }
        }
    }
}