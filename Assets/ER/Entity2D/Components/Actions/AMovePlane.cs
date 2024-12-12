using UnityEngine;

namespace ER.Entity2D.Components
{
    public class AMovePlane : ActionBase
    {
        private float speed = 10;
        private Vector2 m_movDir;

        public Vector2 movDir
        {
            get => m_movDir;
            set
            {
                m_movDir = value;
            }
        }

        public override void Init()
        {
        }

        public override void Entry()
        {
            enabled = true;
        }

        public override void Exit()
        {
            enabled = false;
        }

        private void Update()
        {
            entity.gameObject.transform.position += (Vector3)movDir * speed * Time.deltaTime;
        }
    }
}