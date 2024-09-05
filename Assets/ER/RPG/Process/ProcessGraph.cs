using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ER.RPG
{
    [CreateAssetMenu(fileName = "ProcceGraph", menuName = "ER/流程图")]
    public class ProcessGraph : ScriptableObject,IEnumerable<ProcessGraphNode>
    {
        public Type infoType;//额外对象类型

        public List<ProcessGraphNode> nodes = new List<ProcessGraphNode>();

        public ProcessGraphNode GetNode(string key)
        {
            foreach (ProcessGraphNode node in nodes)
            {
                if(node.name == key)
                    return node;
            }
            return null;
        }

        public ProcessGraphNode GetParentNode(string key)
        {
            var node = GetNode(key);
            if(node == null) return null;

            var parent = GetNode(node.parent);
            return parent;
        }

        public ProcessGraphNode[] GetNextNodes(string key)
        {
            var node = GetNode(key);
            if (node == null) return null;

            ProcessGraphNode[] nodes = new ProcessGraphNode[node.childs.Count];
            for (int i = 0;i< nodes.Length;i++)
            {
                nodes[i] = GetNode(node.childs[i]);
            }
            return nodes;
        }

        public void RemoveNode(string key)
        {
            var node = GetNode(key);
            nodes.Remove(node);
        }

        public IEnumerator<ProcessGraphNode> GetEnumerator()
        {
            foreach(var node in nodes)
            {
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var node in nodes)
            {
                yield return node;
            }
        }
    }

    [Serializable]
    public class ProcessGraphNode
    {
        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public Vector2 nodeSize;

        /// <summary>
        /// 节点名称
        /// </summary>
        public string name;

        /// <summary>
        /// 父节点
        /// </summary>
        public string parent;

        /// <summary>
        /// 子节点
        /// </summary>
        public List<string> childs;

        /// <summary>
        /// 默认状态
        /// </summary>
        public Node.Status defaultStatus;

        /// <summary>
        /// 额外信息
        /// </summary>
        public object extraInfos;
    }
}