using System;
using UnityEngine;

namespace ER
{
    public class MathUtils
    {
        /// <summary>
        /// 求交点
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="r1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="r2"></param>
        public static Vector2[] FindIntersections(Vector2 pos1, float r1, Vector2 pos2, float r2)
        {
            var delta = pos2 - pos1;
            float d = delta.magnitude;

            // 判断圆的位置关系
            if (d > r1 + r2 || d < Math.Abs(r1 - r2))
            {
                return new Vector2[0];//没有交点
            }
            else if (d == r1 + r2 || d == Math.Abs(r1 - r2))
            {
                return new Vector2[]//仅一个交点
                {
                    Vector2.Lerp(pos1,pos2,r1/(r1+r2))
                };
            }

            // 计算交点
            float a = (Mathf.Pow(r1, 2) - Mathf.Pow(r2, 2) + Mathf.Pow(d, 2)) / (2 * d);
            float h = Mathf.Sqrt(Mathf.Pow(r1, 2) - Mathf.Pow(a, 2));

            float x0 = pos1.x + a / d * delta.x;
            float y0 = pos1.y + a / d * delta.y;

            // 第一交点
            return new Vector2[]
            {
                new Vector2(x0 + h / d * delta.y,y0-h/d *delta.x),
                new Vector2(x0 - h / d * delta.y,y0+h/d*delta.x)
            };
        }
    }
}