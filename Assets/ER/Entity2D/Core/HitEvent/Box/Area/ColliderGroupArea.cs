using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    public class ColliderGroupArea : Area
    {
        [SerializeField]
        private Collider2D[] colliders;

        [SerializeField]
        private TagFilter m_filter;

        public TagFilter filter => m_filter;

        private Dictionary<Collider2D, int> processed = new();
        private Dictionary<ColliderGroupArea, int> processedGroup = new();

        public void Start()
        {
            foreach (var collider in colliders)
            {
                var sub_flag = collider.GetOrAddComponent<ColliderFlag>();
                sub_flag.group = this;
            }
        }

        public void OnEnter(Collider2D other)
        {
            if (!filter.Filter(other.tag)) return;
            if (!processed.ContainsKey(other))
            {
                processed.Add(other, 0);
                InvokeEnterEvent(other.gameObject);
            }
            processed[other] += 1;
        }

        public void OnExit(Collider2D other)
        {
            if (!filter.Filter(other.tag)) return;
            if (!processed.ContainsKey(other))
            {
                return;
            }
            processed[other] -= 1;
            if (processed[other] <= 0)
            {
                processed.Remove(other);
                InvokeExitEvent(other.gameObject);
            }
        }

        public void OnEnter(ColliderGroupArea other)
        {
            if (!filter.Filter(other.tag)) return;
            if (!processedGroup.ContainsKey(other))
            {
                processedGroup.Add(other, 0);
                InvokeEnterEvent(other.gameObject);
            }
            processedGroup[other] += 1;
        }

        public void OnExit(ColliderGroupArea other)
        {
            if (!filter.Filter(other.tag)) return;
            if (!processedGroup.ContainsKey(other))
            {
                return;
            }
            processedGroup[other] -= 1;
            if (processedGroup[other] <= 0)
            {
                processedGroup.Remove(other);
                InvokeExitEvent(other.gameObject);
            }
        }


        private void OnDisable()
        {
            processed.Clear();
            processedGroup.Clear();
        }
    }
}