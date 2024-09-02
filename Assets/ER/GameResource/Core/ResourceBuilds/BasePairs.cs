
using ER.ForEditor;
using System;
using UnityEngine;

namespace ER.Resource
{
    [Serializable]
    public struct FloatPair
    {
        [DisplayLabel("键")]
        public string key;
        [DisplayLabel("值")]
        public float value;
    }
    [Serializable]
    public struct IntPair
    {
        [DisplayLabel("键")]
        public string key;
        [DisplayLabel("值")]
        public int value;
    }
    [Serializable]
    public struct StringPair
    {
        [DisplayLabel("键")]
        public string key;
        [DisplayLabel("值")]
        [Multiline(5)]
        public string value;
    }
    [Serializable]
    public struct BoolPair
    {
        [DisplayLabel("键")]
        public string key;
        [DisplayLabel("值")]
        public bool value;
    }

}