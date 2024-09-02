using System.Collections.Generic;
using UnityEngine;

namespace ER.StateNodeGraph
{
    [CreateAssetMenu(fileName = "StateGraph", menuName = "游戏核心资产/敌人行为状态图")]
    public class StateGraph : ScriptableObject
    {
        public List<StateKeyPair> nodes = new List<StateKeyPair>();

        public StateNode GetNode(string key)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].key == key)
                {
                    return nodes[i].value;
                }
            }
            return null;
        }

        public StateNode GetNode(int index)
        {
            foreach (var node in nodes)
            {
                if (node.value.index == index) return node.value;
            }
            return null;
        }

        public StateNode[] GetNextNodes(int index)
        {
            var node = GetNode(index);
            List<StateNode> nexts = new List<StateNode>();
            foreach (var nt in node.nexts)
            {
                var next = GetNode(nt);
                if (next != null)
                {
                    nexts.Add(next);
                }
            }
            return nexts.ToArray();
        }

        public float[] GetNextWeights(int index)
        {
            var node = GetNode(index);
            return node.weights.ToArray();
        }

    }
}