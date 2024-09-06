using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    public struct Line
    {
        public List<Vector2> positions;
        public bool loop;
        public int Count=>positions.Count;

        public Vector2 this[int index]
        {
            get => positions[index];
        }
    }
}