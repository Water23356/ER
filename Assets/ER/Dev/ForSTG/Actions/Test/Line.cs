using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    public struct Line:IEnumerable<Vector2>
    {
        public List<Vector2> positions;
        public bool loop;
        public int Count=>positions.Count;

        public Vector2 this[int index]
        {
            get => positions[index];
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            foreach(var node in positions)
                yield return node;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var node in positions)
                yield return node;
        }
    }
}