using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.GUI
{
    /// <summary>
    /// 基于 Image 填充的进度条显示
    /// </summary>
    public class ImageProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Image fillContent;

        [SerializeField]
        private TMP_Text txt_amount;

        [Header("动画配置")]
        [SerializeField]
        private float lerpSpeed = 6f;

        [SerializeField]
        private float maxSpeed = 3f;

        private float progress = 0f;

        public float Progress
        {
            get => progress; set
            {
                progress = Mathf.Clamp01(value);
                enabled = true;
            }
        }

        public string ProgressText
        {
            get => txt_amount?.text;
            set
            {
                if (txt_amount != null) txt_amount.text = value;
            }
        }

        private void Update()
        {
            bool catched;
            fillContent.fillAmount = Utils.LerpForTime(fillContent.fillAmount, Progress, lerpSpeed, maxSpeed, out catched);
            if (catched)
                enabled = false;
        }

        public void SetAmount(int amount, int total)
        {
            if (txt_amount != null)
                txt_amount.text = $"{amount}/{total}";
            Progress = (float)amount / total;
        }
    }
}