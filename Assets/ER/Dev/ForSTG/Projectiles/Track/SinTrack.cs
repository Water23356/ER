using System.Collections.Generic;
using UnityEngine;

namespace ER.STG.Tracks
{
    public class SinTrack : ProjectileTrack
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

        public override void Set(Dictionary<string, object> props)
        {
            moveDir = (Vector2)props["moveDir"];
            angleRange = (float)props["angleRange"];
            changeCD = (int)props["changeCD"];
            startOffset = (float)props["startOffset"];
            maxTimes = (int)props["maxTimes"];
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
                owner.velocity = moveDir.Rotate(sinOffset * angleRange);
                times++;
                times %= maxTimes;
                CalculateSinOffset();
            }
        }

        private void CalculateSinOffset()
        {
            sinOffset = Mathf.Sin(Mathf.PI * Mathf.Lerp(0, 2, times / (float)(maxTimes)));
        }

        private void OnDisable()
        {
            timer = 0;
            enabled = false;
        }
    }
}