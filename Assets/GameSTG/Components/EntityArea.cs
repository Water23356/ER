using UnityEngine;

namespace STG
{
    [RequireComponent(typeof(BoxCollider2D), typeof(CircleCollider2D))]
    public class EntityArea : MonoBehaviour, IEntityCompenont
    {
        private BoxCollider2D m_box;
        private CircleCollider2D m_circle;
        private int m_mode;//作用模式

        public BoxCollider2D box
        {
            get
            {
                if (m_box == null)
                    m_box = GetComponent<BoxCollider2D>();
                return m_box;
            }
        }

        public CircleCollider2D circle
        {
            get
            {
                if (m_circle == null)
                    m_circle = GetComponent<CircleCollider2D>();
                return m_circle;
            }
        }

        public int mode
        {
            get => m_mode; set
            {
                m_mode = value;
                switch (mode)
                {
                    case 0:
                        box.enabled = true;
                        circle.enabled = false;
                        break;

                    case 1:
                        circle.enabled = true;
                        box.enabled = false;
                        break;

                    default:
                        box.enabled = false;
                        circle.enabled = false;
                        break;
                }
            }
        }

        public float radius
        {
            get => circle.radius;
            set => circle.radius = value;
        }

        public Vector2 size
        {
            get => box.size;
            set => box.size = value;
        }

        private void OnDisable()
        {
            box.enabled = false;
            circle.enabled = false;
        }

        private void OnEnable()
        {
            mode = m_mode;
        }

        public LuaHandler GetAdpator()
        {
            return new LuaHandler(this);
        }

        public struct LuaHandler
        {
            private EntityArea origin;

            public LuaHandler(EntityArea origin)
            {
                this.origin = origin;
            }

            public int mode
            {
                get => origin.mode; set => origin.mode = value;
            }

            public float radius
            {
                get => origin.radius; set => origin.radius = value;
            }

            public Vector2 size
            {
                get => origin.size; set => origin.size = value;
            }

            public bool enabled
            {
                get => origin.enabled; set => origin.enabled = value;
            }
        }
    }
}