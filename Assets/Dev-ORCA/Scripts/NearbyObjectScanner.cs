using System.Collections.Generic;
using UnityEngine;

namespace Dev
{
    public class NearbyObjectScanner : MonoBehaviour
    {
        public float scanRadius = 5f; // 扫描半径
        public LayerMask detectionLayers; // 参与检测的层

        // 获取周围物体的方法
        public Collider2D[] ScanNearbyObjects()
        {
            // 使用 Physics2D.OverlapCircle 方法获取指定半径范围内的所有碰撞体
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, scanRadius, detectionLayers);
            List<Collider2D> results = new List<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == gameObject) continue;
                results.Add(collider);
            }
            return results.ToArray();
        }

        // 可选: 可视化扫描范围（调试用）
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, scanRadius); // 在选中的物体上绘制一个红色的线框
        }
    }
}