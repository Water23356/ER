using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.StateNodeGraph
{
    [Serializable]
    public class StateNode
    {
        //可视化区
        public string name;

        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public Vector2 nodeSize;

        [HideInInspector]
        public string key;

        //数据存储区
        public int index;

        public List<string> nexts;

        public List<float> weights;
    }
    [Serializable]
    public struct StateKeyPair
    {
        public string key;
        public StateNode value;

    }
}