using ER.Entity2D.Agents;
using ER.ForEditor;
using UnityEngine;

namespace ER.Entity2D
{
    public class StateBase:MonoBehaviour
    {
        public StateAgent agent { get; set; }
        public EntityAgent entity { get => agent?.entity; }
    }
}