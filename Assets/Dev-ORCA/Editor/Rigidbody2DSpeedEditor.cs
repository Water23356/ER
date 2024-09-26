using UnityEditor;
using UnityEngine;

namespace Dev
{
    [CustomEditor(typeof(Rigidbody2D))]
    public class Rigidbody2DSpeedEditor : Editor
    {
        private Rigidbody2D rb;
        private const float handleSize = 0.3f;  // 控制点大小
        private const float moveHandleSize = 0.1f;  // 拖动速度控制点的大小

        private void OnEnable()
        {
            rb = (Rigidbody2D)target;
        }

        private void OnSceneGUI()
        {
            if (rb != null)
            {
                // 获取当前刚体的位置和速度
                Vector2 position = rb.position;
                Vector2 velocity = rb.velocity;

                // 绘制速度向量
                Handles.color = Color.red;
                Handles.DrawLine(position, position + velocity, 2f); // 绘制速度线段

                // 绘制控制点
                Handles.color = Color.green;
                var fmh_32_83_638628780908752669 = Quaternion.identity; Vector2 newVelocity = Handles.FreeMoveHandle(position + velocity, handleSize, Vector3.zero, Handles.SphereHandleCap);

                // 计算新的速度
                Vector2 direction = (newVelocity - position).normalized;  // 计算方向
                float magnitude = (newVelocity - position).magnitude;  // 计算新的幅值
                rb.velocity = direction * magnitude;  // 更新刚体速度

                // 显示当前速度
                Handles.Label(newVelocity, "Velocity: " + rb.velocity.ToString("F2"));
            }
        }
    }
}