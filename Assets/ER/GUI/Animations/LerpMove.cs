using ER.ForEditor;
using UnityEngine;

namespace ER.GUI.Animations
{
    /// <summary>
    /// 平滑移动
    /// </summary>
    public class LerpMove : MonoBehaviour
    {
        [SerializeField]
        private Vector2 aimPos;

        [SerializeField]
        private float lerpSpeed;

        [SerializeField]
        private float maxLerpSpeed;

        [Tooltip("启用后: 在抵达归为点后自动失活")]
        [DisplayLabel("自动失活")]
        [SerializeField]
        private bool autoDisable = false;

        public Vector2 AimPos { get => aimPos; set => aimPos = value; }
        public float LerpSpeed { get => lerpSpeed; set => lerpSpeed = value; }
        public float MaxLerpSpeed { get => maxLerpSpeed; set => maxLerpSpeed = value; }
        public bool AutoDisable { get => autoDisable; set => autoDisable = value; }

        public void Update()
        {
            bool catched = false;
            transform.position = Utils.LerpForTime(transform.position, AimPos, LerpSpeed, MaxLerpSpeed, out catched);
            if (catched)
            {
                enabled = false;
            }
        }

        public float GetDistanceToAim()
        {
            return Vector2.Distance(AimPos, transform.position);
        }

        public Vector2 GetDiractionToAim()
        {
            return (AimPos - (Vector2)transform.position).normalized;
        }
    }
}