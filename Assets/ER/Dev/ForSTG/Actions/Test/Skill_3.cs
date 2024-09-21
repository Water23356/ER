using ER.ForEditor;
using UnityEngine;

namespace ER.STG.Actions
{
    /// <summary>
    /// 圆弧弹幕扫荡
    /// </summary>
    public class Skill_3 : RoleAction
    {
        [SerializeField]
        private int keepTime = 300;//6s

        [SerializeField]
        private float speed = 3;

        [SerializeField]
        private Vector2 shootDir = Vector2.down;

        [SerializeField]
        private int shootCD = 3;

        [DisplayLabel("射击角度大小")]
        [SerializeField]
        private float angleRange = 30;

        [DisplayLabel("弹幕数量")]
        [SerializeField]
        private int projectileCount = 6;

        [Tooltip("负数表示不限")]
        [DisplayLabel("最大弹幕角度差")]
        [SerializeField]
        private float maxDistance = -1;

        private int selfTimer = 0;

        public Skill_3() : base()
        {
            Timer.limitTick = 120;
        }

        public override void ActionEffect()
        {
            enabled = true;
            selfTimer = 0;
        }

        private void ShootWave()
        {
            float needRange = angleRange;
            if (maxDistance > 0)
            {
                needRange = Mathf.Min(needRange, maxDistance * (projectileCount - 1));
            }
            var halfRange = needRange / 2;

            for (int i = 0; i <= projectileCount; i++)
            {
                float angle = Mathf.Lerp(-halfRange, halfRange, i / (float)projectileCount);
                var dir = shootDir.Rotate(angle).normalized;
                Shoot(dir * speed, Color.white);
            }
        }

        private void FixedUpdate()
        {
            if (selfTimer % shootCD == 0)
            {
                ShootWave();
            }
            selfTimer++;

            if (selfTimer > keepTime && keepTime > 0)
            {
                enabled = false;
            }
        }
    }
}