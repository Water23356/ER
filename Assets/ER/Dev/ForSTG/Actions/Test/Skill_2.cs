using UnityEngine;

namespace ER.STG.Actions
{
    /// <summary>
    /// 单向整圆扫描
    /// </summary>
    public class Skill_2 : RoleAction
    {

        [SerializeField]
        private int keepTime = 300;//3s

        [SerializeField]
        private float speed = 3;

        [SerializeField]
        private int cellAngle = 2;

        [SerializeField]
        private Vector2 startDir = Vector2.down;

        private int selfTimer = 0;

        public Skill_2() : base()
        {
            Timer.limitTick = 120;
        }

        public override void ActionEffect()
        {
            enabled = true;
            selfTimer = 0;
        }

        private void FixedUpdate()
        {
            selfTimer++;
            var dir = startDir.Rotate(cellAngle * selfTimer).normalized;
            Shoot(dir * speed, Color.white);

            if (selfTimer > keepTime && keepTime > 0)
            {
                enabled = false;
            }
        }
    }
}