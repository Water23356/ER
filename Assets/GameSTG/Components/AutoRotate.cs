using ER;
using UnityEngine;

namespace STG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AutoRotate : MonoBehaviour, IEntityCompenont
    {
        [SerializeField]
        private Vector2 m_defaultDir;

        private Rigidbody2D m_rigidbody;

        public Rigidbody2D rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>();
                return m_rigidbody;
            }
        }

        public Vector2 defaultDir { get => m_defaultDir; set => m_defaultDir = value; }

        public LuaHandler GetAdpator()
        {
            return new LuaHandler(this);
        }

        private void Update()
        {
            var dir = rigidbody.velocity;
            if (dir != Vector2.zero)
            {
                transform.localEulerAngles = new Vector3(0, 0, defaultDir.ClockAngle(dir));
            }
        }

        public struct LuaHandler
        {
            private AutoRotate origin;

            public LuaHandler(AutoRotate origin)
            {
                this.origin = origin;
            }

            public Vector2 defaultDir { get => origin.defaultDir; set => origin.defaultDir = value; }

            public bool enabled
            {
                get => origin.enabled; set => origin.enabled = value;
            }
        }
    }
}