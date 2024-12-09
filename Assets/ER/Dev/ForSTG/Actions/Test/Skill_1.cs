using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.STG.Actions
{
    /// <summary>
    /// 圆弧弹幕扫荡
    /// </summary>
    public class Skill_1 : EntityAction
    {
        //private readonly string poolName = "pool:projectile";

        [SerializeField]
        private float angleLimit = 15f;

        private int selfTimer = 0;

        [SerializeField]
        private int skillTime = 300;//6s

        [DisplayLabel("射击槽CD")]
        [SerializeField]
        private List<ShootSlot> shootSlot = new List<ShootSlot>();

        [SerializeField]
        private float angleOffset = 0.5f;

        public Skill_1() : base()
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
            for (int i = 0; i < shootSlot.Count; i++)
            {
                var slot = shootSlot[i];

                var angle = Mathf.Sin(selfTimer * slot.angleCell + i * angleOffset) * angleLimit;
                var dir = Vector2.up.Rotate(angle);
                if (selfTimer % slot.shootCD == 0)
                {
                    Shoot(dir*slot.speed, slot.color);
                }
            }

            selfTimer++;
            if (selfTimer > skillTime && skillTime > 0)
            {
                enabled = false;
            }
        }

        [Serializable]
        public struct ShootSlot
        {
            public int shootCD;
            public Color color;
            public float speed;
            public float angleCell;
        }
    }
}