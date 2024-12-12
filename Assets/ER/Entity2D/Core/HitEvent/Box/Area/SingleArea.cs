using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ER.Entity2D
{
    public class SingleArea : Area
    {
        [SerializeField]
        private TagFilter m_filter;
        public TagFilter filter=>m_filter;

        private Dictionary<ColliderGroupArea, int> processedGroup = new();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(filter.Filter(collision.gameObject.tag))
                HandleEnter(collision.gameObject);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (filter.Filter(collision.gameObject.tag))
                HandleExit(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (filter.Filter(collision.gameObject.tag))
                HandleEnter(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (filter.Filter(collision.gameObject.tag))
                HandleExit(collision.gameObject);
        }

        private void HandleEnter(GameObject other)
        {
            var flag = other.GetComponent<ColliderFlag>();
            if (flag != null && flag.enabled && flag.group != null)
            {
                if (!processedGroup.ContainsKey(flag.group))
                {
                    processedGroup.Add(flag.group, 0);
                    InvokeEnterEvent(flag.group.gameObject);
                }
                processedGroup[flag.group] += 1;
            }
            else
            {
                InvokeEnterEvent(other);
            }
        }

        private void HandleExit(GameObject other)
        {
            var flag = other.GetComponent<ColliderFlag>();
            if (flag != null && flag.enabled && flag.group != null)
            {
                if (!processedGroup.ContainsKey(flag.group))
                {
                    return;
                }
                processedGroup[flag.group] -= 1;
                if (processedGroup[flag.group] <= 0)
                {
                    processedGroup.Remove(flag.group);
                    InvokeExitEvent(flag.group.gameObject);
                }
            }
            else
            {
                InvokeExitEvent(other);
            }
        }
    }
}