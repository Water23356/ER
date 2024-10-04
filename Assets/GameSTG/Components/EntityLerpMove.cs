using ER;
using UnityEngine;

namespace STG
{
    public class EntityLerpMove : MonoBehaviour, IEntityCompenont
    {
        [SerializeField]
        private Vector2 aimPos;//目标位置

        [SerializeField]
        private float lerpSpeed;//插值速度

        [SerializeField]
        private float maxSpeed;//最大速度

        [SerializeField]
        private float precision;//容差范围

        public Vector2 AimPos { get => aimPos; set => aimPos = value; }
        public float LerpSpeed { get => lerpSpeed; set => lerpSpeed = value; }
        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public float Precision { get => precision; set => precision = value; }

        private void Update()
        {
            bool catched = false;
            transform.position = Utils.LerpForTime(transform.position, AimPos, LerpSpeed, MaxSpeed, out catched);
            if (catched)
            {
                enabled = false;
            }
        }

        public struct LuaHandler
        {
            private EntityLerpMove origin;

            public LuaHandler(EntityLerpMove origin)
            {
                this.origin = origin;
            }

            public Vector2 aimPos
            {
                get => origin.AimPos; set { origin.AimPos = value; origin.enabled = true; }
            }

            public float lerpSpeed
            {
                get => origin.LerpSpeed; set => origin.LerpSpeed = value;
            }

            public float maxSpeed
            {
                get => origin.MaxSpeed; set => origin.MaxSpeed = value;
            }

            public float precision
            {
                get => origin.precision; set => origin.precision = value;
            }

            public bool enabled
            {
                get => origin.enabled; set => origin.enabled = value;
            }
        }
    }
}