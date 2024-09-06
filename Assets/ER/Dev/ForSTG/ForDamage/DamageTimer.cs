using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    /// <summary>
    /// 伤害计时器
    /// </summary>
    public class DamageTimer : MonoBehaviour
    {
        private Dictionary<string, DamageTimerCell> cells = new Dictionary<string, DamageTimerCell>();

        public void Register(string timerName, int limitTick)
        {
            cells[timerName] = new DamageTimerCell()
            {
                Ticks = -1,
                DamageTimerTag = timerName,
                ImmunityTime = limitTick
            };
        }

        public void Unregister(string timerName)
        {
            cells.Remove(timerName);
        }

        public void ResetTimer(string timerName)
        {
            if (cells.TryGetValue(timerName, out var cell))
            {
                cell.Reset();
            }
        }

        /// <summary>
        /// 判断该伤害是否能够造成伤害
        /// </summary>
        /// <param name="timerName"></param>
        /// <returns></returns>
        public bool CanDamage(string timerName)
        {
            if (cells.TryGetValue(timerName, out var cell))
            {
                return cell.canDamage;
            }
            return true;
        }

        /// <summary>
        /// 激活计时器, 该伤害能够造成伤害则返回true
        /// </summary>
        /// <param name="stringName"></param>
        /// <returns></returns>
        public bool ActiveTimer(string timerName)
        {
            if (cells.TryGetValue(timerName, out var cell))
            {
                var state = cell.canDamage;
                if (state)
                {
                    cell.Active();
                }
                return state;
            }
            var limit = DamageTimerDictionary.TickNeed(timerName);
            Register(timerName, limit);
            return true;
        }

        private void FixedUpdate()
        {
            foreach (var cell in cells.Values)
            {
                cell.Update();
            }
        }
    }
}