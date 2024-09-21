using ER.ForEditor;
using UnityEngine;

namespace ER.STG.Actions
{
    public class Shoot : RoleAction
    {
        private readonly string poolName = "pool:projectile";

        [SerializeField]
        private float speed = 9f;

        [DisplayLabel("最大间距")]
        [SerializeField]
        private float maxDistance = 0.1f;

        [DisplayLabel("射出数量")]
        [SerializeField]
        public int amount = 1;

        [DisplayLabel("排列范围")]
        [SerializeField]
        public float arrangeRange = 1;

        [DisplayLabel("偏移")]
        [SerializeField]
        public Vector2 offset = Vector2.up*0.22f;

        [DisplayLabel("射击方向")]
        [SerializeField]
        public Vector2 shootDir = Vector2.up;

        public override void ActionEffect()
        {
            var range = maxDistance * (amount - 1);
            range = Mathf.Min(range, arrangeRange);

            Vector2 offsetDir = shootDir.Vertical().normalized;
            Vector2 offsetPos = offset.Rotate(Vector2.up.ClockAngle(shootDir));

            for (int i = 0; i < amount; i++)
            {
                Shoot(shootDir * speed, offsetPos + offsetDir * range * SafeLerp(-0.5f, 0.5f, i, amount - 1), Color.white);
            }
        }

        private float SafeLerp(float min, float max, int current, float totoal)
        {
            if (totoal == 0 && current == 0)
            {
                Debug.Log("返回 0");
                return 0;
            }
            return Mathf.Lerp(-1, 1, current / totoal);
        }
    }
}