using System;
using System.Collections;
using System.Collections.Generic;

namespace ER
{
    /// <summary>
    /// 优先队列
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    public class PriorityQueue<T> : IEnumerable<T>
    {
        #region 属性

        private LinkedList<T> values;//元素表
        private Func<T, T, bool> Comparer;//元素比较器

        #endregion 属性

        /// <summary>
        /// 刷新排序(临近交换排序, 适用于只有少部分逆序的情况)
        /// </summary>
        public void Refresh()
        {
            bool sorted = true;
            while (sorted)
            {
                sorted = false;
                var start = values.First;
                var node = start;
                var next = node?.Next;
                while (next != null)
                {
                    if (!Comparer(node.Value, next.Value) && Comparer(next.Value, node.Value))
                    {
                        Swap(values, node, next);
                        sorted = true;
                        next = node.Next;
                        continue;
                    }
                    node = next;
                    next = next.Next;
                }
            }
        }

        private void Swap(LinkedList<T> list, LinkedListNode<T> node1, LinkedListNode<T> node2)
        {
            list.Remove(node1);
            list.AddAfter(node2, node1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_Compare">排序方法, 返回 true 则将参数目标1放在索引更小的位置</param>
        public PriorityQueue(Func<T, T, bool> _Compare)
        {
            values = new();
            Comparer = _Compare;
        }
        /// <summary>
        /// 清空元素
        /// </summary>
        public void Clear()
        {
            values.Clear();
        }

        public void Add(params T[] items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            LinkedListNode<T> node = values.First;
            while (node != null)
            {
                if (Comparer(item, node.Value))
                {
                    values.AddBefore(node, item);
                    return;
                }
                node = node.Next;
            }
            values.AddLast(item);
        }

        /// <summary>
        /// 移除指定元素
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            values.Remove(item);
        }

        public void RemoveAt(int index)
        {
            int i = 0;
            LinkedListNode<T> node = values.First;
            while (node != null)
            {
                if (i == index)
                {
                    values.Remove(node);
                    return;
                }
                i++;
                node = node.Next;
            }
        }

        /// <summary>
        /// 顶部元素
        /// </summary>
        public T Top
        {
            get
            {
                if (values.Count > 0) return values.First.Value;
                return default(T);
            }
        }

        /// <summary>
        /// 获取顶部元素, 并从表中移除它
        /// </summary>
        /// <returns></returns>
        public T GetTop()
        {
            if (values.Count > 0)
            {
                T top = values.First.Value;
                values.RemoveFirst();
                return top;
            }
            return default(T);
        }

        public LinkedListNode<T> FirtNode=>values.First;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }

        /// <summary>
        /// 容器中的元素数量
        /// </summary>
        public int Count { get => values.Count; }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= values.Count) return default(T);
                int i = 0;
                LinkedListNode<T> node = values.First;
                while (node != null)
                {
                    if (i == index)
                    {
                        return node.Value;
                    }
                    i++;
                    node = node.Next;
                }
                return default(T);
            }
        }
    }
}