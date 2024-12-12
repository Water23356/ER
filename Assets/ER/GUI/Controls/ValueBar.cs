using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.GUI
{
    /// <summary>
    /// 值条: 样式1
    /// </summary>
    public class ValueBar_1 : MonoBehaviour
    {
        [Header("组件")]
        [SerializeField]
        private Image img_bar;

        [SerializeField]
        private TMP_Text txt_value;

        [Header("属性")]
        [SerializeField]
        [Range(0, 1)]
        protected float m_current = 0.1f;

        public float current
        {
            get => m_current;
            set
            {
                m_current = Mathf.Clamp01(value);
                if (img_bar != null)
                    img_bar.fillAmount = m_current;
                if (txt_value != null)
                    txt_value.text = $"{m_current * 100f}%";
            }
        }

        public void SetValue(float value, float max)
        {
            m_current = value / max;
            if (float.IsNaN(m_current))
            {
                m_current = 1f;
                if (txt_value != null)
                    txt_value.text = $"NaN";
            }
            else
            {
                if (txt_value != null)
                    txt_value.text = $"{value}/{max}";
            }
            if (img_bar != null)
                img_bar.fillAmount = m_current;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            current = m_current;
        }

#endif
    }
}