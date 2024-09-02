using UnityEngine;

namespace ER
{
    /// <summary>
    /// 虚拟钩子
    /// </summary>
    public class VirtualAnchor : Anchor
    {
        private string tag;
        private Vector3 point;
        private object owner;
        public object Owner { get => owner; set => owner = value; }

        public string AnchorTag { get => tag; set => tag = value; }
        public Vector3 Point { get => point; set => point = value; }

        public VirtualAnchor(float x = 0, float y = 0, float z = 0)
        { tag = "Unknown"; point = new Vector3(x, y, z); }

        public VirtualAnchor(string tag, float x = 0, float y = 0, float z = 0)
        { this.tag = tag; point = new Vector3(x, y, z); }

        public void Destroy()
        { }
    }
}