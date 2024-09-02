using UnityEngine;
using UnityEngine.Diagnostics;

namespace ER
{
    public sealed class CameraFollower : MonoBehaviour
    {
        [Tooltip("镜头跟随目标")]
        public Transform target;
        [Tooltip("平滑系数")]
        public float smoothing = 5f;
        [Tooltip("初始偏移量")]
        [SerializeField]
        private Vector3 offset = new Vector3(0, 0, -10);
        public float smoothDistanceMax = 50;

        void FixedUpdate() // 使用FixedUpdate来保持与物理更新的同步
        {
            // 目标位置 = 角色位置 + 偏移量
            Vector3 targetCamPos = target.position + offset;
            // 线性插值平滑地移动到目标位置
            transform.position = Utils.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime, smoothDistanceMax * Time.deltaTime);
        }
    }
}