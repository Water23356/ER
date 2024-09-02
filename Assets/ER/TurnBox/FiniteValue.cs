using System;
using UnityEngine;

namespace ER.TurnBox
{
    [Serializable]
    public class FiniteValueInt
    {
        [SerializeField]
        private int m_max;

        [SerializeField]
        private bool m_max_limit;

        [SerializeField]
        private int m_min;

        [SerializeField]
        private bool m_min_limit;

        [SerializeField]
        private int m_current;

        /// <summary>
        /// 当值被修改时触发, Invoke(修改前值, 修改后的值), 当前值为修改后的值
        /// </summary>
        public event Action<int, int> OnValueModify;

        /// <summary>
        /// 当值被修改时, 若越过上界触发, Invoke(当前值), 越界后自动修正为边界值
        /// </summary>
        public event Action<int> OnValueOverflowMax;

        /// <summary>
        /// 当值被修改时, 若越过下界触发, Invoke(当前值), 越界后自动修正为边界值
        /// </summary>
        public event Action<int> OnValueOverflowMin;

        /// <summary>
        /// 值上界
        /// </summary>
        public int Max { get => m_max; set => m_max = value; }

        /// <summary>
        /// 上界是否有效
        /// </summary>
        public bool IsLimitMax { get => m_max_limit; set => m_max_limit = value; }

        /// <summary>
        /// 下界
        /// </summary>
        public int Min { get => m_min; set => m_min = value; }

        /// <summary>
        /// 下界是否有效
        /// </summary>
        public bool IsLimitMin { get => m_min_limit; set => m_min_limit = value; }

        public int Value
        {
            get => m_current;
            set
            {
                int tmp = m_current;
                m_current = value;
                OnValueModify?.Invoke(tmp, value);
                if (IsLimitMax && m_current >= Max)
                {
                    OnValueOverflowMax?.Invoke(m_current);
                    m_current = Max;
                }
                if (IsLimitMin && m_current <= Min)
                {
                    OnValueOverflowMin?.Invoke(m_current);
                    m_current = Min;
                }
            }
        }

        /// <summary>
        /// 获取当前值占总值长度的百分比
        /// </summary>
        public float Percentage { get => (float)(m_current - m_min) / (m_max - m_min); }

        public void ModfiyValue(int newValue, bool triggerEvent = true)
        {
            if (triggerEvent)
                Value = newValue;
            else
                m_current = newValue;
        }

        public static implicit operator int(FiniteValueInt value)
        {
            return value.Value;
        }

        /// <summary>
        /// 取非负数
        /// </summary>
        public static FiniteValueInt NonNegative
        {
            get => new FiniteValueInt
            {
                Value = 0,
                Max = 0,
                Min = 0,
                IsLimitMin = true,
                IsLimitMax = false,
            };
        }
    }

    [Serializable]
    public class FiniteValueFloat
    {
        [SerializeField]
        private float m_max;

        [SerializeField]
        private bool m_max_limit;

        [SerializeField]
        private float m_min;

        [SerializeField]
        private bool m_min_limit;

        [SerializeField]
        private float m_current;

        /// <summary>
        /// 当值被修改时触发, Invoke(修改前值, 修改后的值), 当前值为修改后的值
        /// </summary>
        public event Action<float, float> OnValueModify;

        /// <summary>
        /// 当值被修改时, 若越过上界触发, Invoke(当前值), 越界后自动修正为边界值
        /// </summary>
        public event Action<float> OnValueOverflowMax;

        /// <summary>
        /// 当值被修改时, 若越过下界触发, Invoke(当前值), 越界后自动修正为边界值
        /// </summary>
        public event Action<float> OnValueOverflowMin;

        /// <summary>
        /// 值上界
        /// </summary>
        public float Max { get => m_max; set => m_max = value; }

        /// <summary>
        /// 上界是否有效
        /// </summary>
        public bool IsLimitMax { get => m_max_limit; set => m_max_limit = value; }

        /// <summary>
        /// 下界
        /// </summary>
        public float Min { get => m_min; set => m_min = value; }

        /// <summary>
        /// 下界是否有效
        /// </summary>
        public bool IsLimitMin { get => m_min_limit; set => m_min_limit = value; }

        public float Value
        {
            get => m_current;
            set
            {
                float tmp = m_current;
                m_current = value;
                OnValueModify?.Invoke(tmp, value);
                if (IsLimitMax && m_current > Max)
                {
                    OnValueOverflowMax?.Invoke(m_current);
                    m_current = Max;
                }
                if (IsLimitMin && m_current < Min)
                {
                    OnValueOverflowMin?.Invoke(m_current);
                    m_current = Min;
                }
            }
        }

        /// <summary>
        /// 获取当前值占总值长度的百分比
        /// </summary>
        public float Percentage { get => (m_current - m_min) / (m_max - m_min); }

        public void ModfiyValue(float newValue, bool triggerEvent = true)
        {
            if (triggerEvent)
                Value = newValue;
            else
                m_current = newValue;
        }

        public static implicit operator float(FiniteValueFloat value)
        {
            return value.Value;
        }
    }
}