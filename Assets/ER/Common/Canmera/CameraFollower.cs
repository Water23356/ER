using ER.ForEditor;
using UnityEngine;

namespace ER
{
    public sealed class CameraFollower : MonoBehaviour
    {
        [DisplayLabel("镜头跟随目标")]
        public Transform target;

        [DisplayLabel("初始偏移量")]
        public Vector3 offset = new Vector3(0, 0, -10);

        [DisplayLabel("平滑系数")]
        public float lerpSpeed = 5f;

        [DisplayLabel("最大速度")]
        public float maxLerpSpeed = 200f;

        private void LateUpdate()
        {
            if (target != null)
            {
                // 线性插值平滑地移动到目标位置
                Vector3 newPos = Utils.LerpForTime(transform.position, target.position + offset, lerpSpeed, maxLerpSpeed, out bool catched);
                newPos.z = offset.z;
                transform.position = newPos;
            }
        }
    }
}