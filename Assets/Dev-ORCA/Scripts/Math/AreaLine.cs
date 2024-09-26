using System.Collections.Generic;
using UnityEngine;

namespace Dev
{
    public struct AreaLine
    {
        public static float maxLength = 100f;

        public Vector2 dir;//线的延伸方向
        public Vector2 node;//参考点

        //直线 (x,y) = dir*t+node;
        // max 和 min 表示t的取值范围
        // state 表示这是什么类型的线
        public LineState state;

        public float max;
        public float min;

        public Vector2 GetPos(float t)
        {
            return node + dir * t;
        }

        public Vector2 MaxPos
        {
            get
            {
                return GetPos(max);
            }
        }

        public Vector2 MinPos
        {
            get
            {
                return GetPos(min);
            }
        }

        /// <summary>
        /// 计算两个 AreaLine 的交点, 如果有交点则返回 true
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="t1">line1 的t值</param>
        /// <param name="t2">line2 的t值</param>
        /// <returns></returns>
        public static bool GetIntersection(AreaLine line1, AreaLine line2, out float t1, out float t2)
        {
            // 计算线的方向分量
            float u1 = line1.dir.x;
            float v1 = line1.dir.y;
            float u2 = line2.dir.x;
            float v2 = line2.dir.y;

            // 设定线的参数方程
            float denom = u1 * v2 - v1 * u2;

            // 判断是否平行
            if (Mathf.Abs(denom) < float.Epsilon)
            {
                t1 = t2 = 0;
                return false; // 线平行或重合，返回无交点
            }

            // 解出t1和t2
            float t1Numerator = (line2.node.x - line1.node.x) * v2 - (line2.node.y - line1.node.y) * u2;
            float t2Numerator = (line2.node.x - line1.node.x) * v1 - (line2.node.y - line1.node.y) * u1;

            t1 = t1Numerator / denom;
            t2 = t2Numerator / denom;

            // 检查t1和t2是否在有效范围内
            if (t1 < line1.min || t1 > line1.max || t2 < line2.min || t2 > line2.max)
            {
                return false; // 不在有效范围，返回无交点
            }

            return true; // 找到交点
        }

        public static Vector2[] GetIntersection(AreaLine line, Vector2 circleCenter, float radius)
        {
            Vector2 u = line.dir.normalized;
            Vector2 w = line.node - circleCenter;

            float b = Vector2.Dot(u, w);
            float c = w.sqrMagnitude - radius * radius;

            float discriminant = b * b - c;

            if (discriminant < 0)
            {
                // 没有交点
                return new Vector2[0];
            }
            else
            {
                float t1 = -b - Mathf.Sqrt(discriminant);
                float t2 = -b + Mathf.Sqrt(discriminant);

                Vector2 intersection1 = line.node + t1 * u;
                Vector2 intersection2 = line.node + t2 * u;

                if (discriminant == 0)
                {
                    // 一个交点
                    if (line.InRange(t1))
                        return new Vector2[] { intersection1 };
                    return new Vector2[0];
                }
                else
                {
                    // 两个交点
                    List<Vector2> results = new List<Vector2>(2);
                    if (line.InRange(t1))
                        return new Vector2[] { intersection1 };
                    if (line.InRange(t2))
                        return new Vector2[] { intersection2 };
                    return results.ToArray();
                }
            }
        }

        /// <summary>
        /// 计算指定点对于 AreaLine 的垂足, 如果垂足在t范围内, 则返回true
        /// </summary>
        /// <param name="line"></param>
        /// <param name="pointA"></param>
        /// <param name="footOfPerpendicular"></param>
        /// <returns></returns>
        public static bool GetFootOfPerpendicular(AreaLine line, Vector2 pointA, out Vector2 footOfPerpendicular)
        {
            //Debug.Log("计算垂足");

            // 第一步: 计算 AreaLine 的方向单位向量
            Vector2 direction = line.dir.normalized;  // 单位方向向量

            // 第二步: 计算与该方向垂直的单位向量
            Vector2 perpendicularDir = new Vector2(-direction.y, direction.x); // 垂直方向向量

            // 第三步: 通过点 A 以及垂直方向，求出与 AreaLine 的交点
            // 求解 t: (pointA - node) 与 direction 之间的投影
            float t = Vector2.Dot(pointA - line.node, direction);

            // 第四步: 使用 t 值计算垂足的坐标
            footOfPerpendicular = line.node + direction * t;
            var state = line.InRange(t);
            //Debug.Log($"垂足是否有效: {state}");
            return state;
        }

        public bool InRange(float t)
        {
            //Debug.Log($"是否在区域内: 模式: {state} 最小: {min} 最大: {max} 点: {t}");
            switch (state)
            {
                case LineState.None:
                    return true;
                case LineState.StartMin:
                    return t >= min;

                case LineState.StartMax:
                    return t <= max;

                case LineState.Segment:
                    return t >= min && t <= max;

                case LineState.Error:
                    return t >= min || t <= max;

                default:
                    return false;
            }
        }

        public void SetMax(float value)
        {
            switch (state)
            {
                case LineState.None:
                    max = value;
                    state = LineState.StartMax;
                    break;

                case LineState.StartMin:
                    max = Mathf.Min(max, value);
                    if (max < min)
                    {
                        state = LineState.Error;
                    }
                    else
                    {
                        state = LineState.Segment;
                    }
                    break;

                case LineState.StartMax:
                    max = Mathf.Min(max, value);
                    break;

                case LineState.Segment:
                    max = Mathf.Min(max, value);
                    if (max < min)
                    {
                        state = LineState.Error;
                    }
                    break;

                case LineState.Error:
                    max = Mathf.Min(max, value);
                    break;
            }
        }

        public void SetMin(float value)
        {
            switch (state)
            {
                case LineState.None:
                    min = value;
                    state = LineState.StartMin;
                    break;

                case LineState.StartMin:
                    min = Mathf.Max(min, value);
                    if (max < min)
                    {
                        state = LineState.Error;
                    }
                    break;

                case LineState.StartMax:
                    min = Mathf.Max(min, value);
                    if (max < min)
                    {
                        state = LineState.Error;
                    }
                    else
                    {
                        state = LineState.Segment;
                    }
                    break;

                case LineState.Segment:
                    min = Mathf.Max(min, value);
                    if (max < min)
                    {
                        state = LineState.Error;
                    }
                    break;

                case LineState.Error:
                    min = Mathf.Max(min, value);
                    if (max < min)
                    {
                        state = LineState.Error;
                    }
                    break;
            }
        }

        public enum LineState
        {
            /// <summary>
            /// 直线
            /// </summary>
            None,

            /// <summary>
            /// 限定 t 最大值的射线
            /// </summary>
            StartMax,

            /// <summary>
            /// 限定 t 最小值的射线
            /// </summary>
            StartMin,

            /// <summary>
            /// 限定 t 范围的线段
            /// </summary>
            Segment,

            /// <summary>
            /// 错误线段(max 小于 min)
            /// </summary>
            Error,
        }

        public void Draw(Vector2 offset)
        {
            switch (state)
            {
                case LineState.None:
                    Debug.DrawLine(node+ offset, node + dir * maxLength + offset, Color.white);
                    Debug.DrawLine(node+offset, node - dir * maxLength + offset, Color.white);
                    break;

                case LineState.StartMin:
                    Debug.DrawLine(MinPos + offset, MinPos + dir * maxLength + offset, Color.white);
                    break;

                case LineState.StartMax:
                    Debug.DrawLine(MaxPos + offset, MaxPos - dir * maxLength + offset, Color.white);
                    break;

                case LineState.Segment:
                    Debug.DrawLine(MinPos + offset, MaxPos + offset, Color.white);

                    break;

                case LineState.Error:
                    Debug.DrawLine(MinPos + offset, MinPos + dir * maxLength + offset, Color.white);
                    Debug.DrawLine(MaxPos + offset, MaxPos - dir * maxLength + offset, Color.white);

                    break;
            }
        }
        public void Draw(Vector2 offset, Color color)
        {
            switch (state)
            {
                case LineState.None:
                    Debug.DrawLine(node + offset, node + dir * maxLength + offset, color);
                    Debug.DrawLine(node + offset, node - dir * maxLength + offset, color);
                    break;

                case LineState.StartMin:
                    Debug.DrawLine(MinPos + offset, MinPos + dir * maxLength + offset, color);
                    break;

                case LineState.StartMax:
                    Debug.DrawLine(MaxPos + offset, MaxPos - dir * maxLength + offset, color);
                    break;

                case LineState.Segment:
                    Debug.DrawLine(MinPos + offset, MaxPos + offset, color);

                    break;

                case LineState.Error:
                    Debug.DrawLine(MinPos + offset, MinPos + dir * maxLength + offset, color);
                    Debug.DrawLine(MaxPos + offset, MaxPos - dir * maxLength + offset, color);

                    break;
            }
        }
    }
}