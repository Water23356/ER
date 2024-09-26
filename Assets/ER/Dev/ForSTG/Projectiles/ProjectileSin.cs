using UnityEngine;

namespace ER.STG
{
    public class ProjectileSin : Projectile
    {
        /// <summary>
        /// 移动方向(射击方向)
        /// </summary>
        private Vector2 moveDir;

        private float angleRange = 120f; // 速度扫荡范围
        private int changeCD = 10; // 速度变化间隔(0.1s修改一次速度)
        private float startOffset = 0.25f;
        private int maxTimes = 20;

        private int timer;
        private int times;
        private float sinOffset;

        public void CopyMoveInfo(ProjectileSin projectile)
        {
            moveDir = projectile.moveDir;
            angleRange = projectile.angleRange;
            changeCD = projectile.changeCD;
            startOffset = projectile.startOffset;
            maxTimes = projectile.maxTimes;
            times = Mathf.RoundToInt(maxTimes * startOffset);
            CalculateSinOffset();
        }

        public void SetMoveInfo(Setting infos)
        {
            moveDir = infos.speed;
            angleRange = infos.angleRange;
            changeCD = infos.changeCD;
            startOffset = infos.startOffset;
            maxTimes = infos.maxTimes;

            times = Mathf.RoundToInt(maxTimes * startOffset);
            CalculateSinOffset();
        }

        private void FixedUpdate()
        {
            UpdateVelocity();
            timer++;
        }

        private void UpdateVelocity()
        {
            if (timer % changeCD == 0)
            {
                Rigidbody.velocity = moveDir.Rotate(sinOffset * angleRange);
                times++;
                times %= maxTimes;
                CalculateSinOffset();
            }
        }

        private void CalculateSinOffset()
        {
            sinOffset = Mathf.Sin(Mathf.PI * Mathf.Lerp(0, 2, times / (float)(maxTimes)));
        }

        public override void OnHide()
        {
            base.OnHide();
            timer = 0;
            enabled = false;
        }

        public struct Setting
        {
            public Vector2 speed;
            public float angleRange;
            public int changeCD;
            public float startOffset;
            public int maxTimes;
        }
    }
}