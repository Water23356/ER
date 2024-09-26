#define CUSTOM_TEST_

using ER;
using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Dev
{
    /// <summary>
    /// 动态避障
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(NearbyObjectScanner))]
    public class ORCA : MonoBehaviour
    {
        private readonly Color color_barrier = new Color(0.3f, 0.1f, 0.1f, 1f);

        [SerializeField]
        private float timeScale = 1f;

        [Tooltip("这个值越大越不容易相撞")]
        [DisplayLabel("冗余宽度")]
        [SerializeField]
        private float width_ORCA = 0.1f;

        private Rigidbody2D m_rigidbody;
        private CircleCollider2D m_circleCollider;
        private NearbyObjectScanner m_scanner;

        [SerializeField]
        private float maxSpeed = 10f;

        [SerializeField]
        private Vector2 preferredVelocity = Vector2.one;

        public Rigidbody2D rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>();
                return m_rigidbody;
            }
        }

        public CircleCollider2D collider
        {
            get
            {
                if (m_circleCollider == null)
                    m_circleCollider = GetComponent<CircleCollider2D>();
                return m_circleCollider;
            }
        }

        public NearbyObjectScanner scanner
        {
            get
            {
                if (m_scanner == null)
                    m_scanner = GetComponent<NearbyObjectScanner>();
                return m_scanner;
            }
        }

        public Vector2 PreferredVelocity { get => preferredVelocity; set => preferredVelocity = value; }
        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

        private void ORCAModifySpeed()
        {
            Vector2 speed = rigidbody.velocity;
            var objs = scanner.ScanNearbyObjects();
            List<AreaLine> lines = new List<AreaLine>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (HandleObjectORCA(objs[i], out var orca))
                {
                    lines.Add(orca);
                }
            }
            bool error = false;
            //计算区域节点
            for (int i = 0; i < lines.Count; i++)
            {
                for (int k = i; k < lines.Count; k++)
                {
                    float t1, t2;
                    AreaLine.GetIntersection(lines[i], lines[k], out t1, out t2);//无论交点是否在范围内斗保留, 因为需要保留 Error 线段

                    if (Utils.GetRotateDir(lines[i].dir, lines[k].dir) == RotateDir.Anticlockwise)
                    {
                        lines[i].SetMin(t1);
                        lines[k].SetMax(t2);
                    }
                    else
                    {
                        lines[i].SetMax(t1);
                        lines[k].SetMin(t2);
                    }
                }
                if (lines[i].state == AreaLine.LineState.Error)
                    error = true;
            }

            //Debug.Log($"边界数量: {lines.Count}");

            if (lines.Count == 0)//没有边界, 则意味着无需调整
            {
                SetVelocity(PreferredVelocity);
                return;
            }

            List<ORCAPoint> points = new List<ORCAPoint>();

            //遍历边界
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].Draw(transform.position);
                //计算最基本的有效点
                switch (lines[i].state)
                {
                    case AreaLine.LineState.None:
                        break;

                    case AreaLine.LineState.StartMin:
                        AddORCAPos(points, lines[i].MinPos);
                        break;

                    case AreaLine.LineState.StartMax:
                        AddORCAPos(points, lines[i].MaxPos);
                        break;

                    case AreaLine.LineState.Segment:
                        AddORCAPos(points, lines[i].MinPos);
                        AddORCAPos(points, lines[i].MaxPos);
                        break;

                    case AreaLine.LineState.Error:
                        AddORCAPos(points, lines[i].MinPos);
                        break;
                }

                //求垂足有效点
                if (AreaLine.GetFootOfPerpendicular(lines[i], speed, out var orcap))
                {
                    //Debug.Log($"添加垂足: {orcap}");
                    AddORCAPos(points, orcap);
                }
            }

            if (points.Count > 0)
            {
                if (!points[0].inRange) error = true;
            }
            else
            {
                //Debug.Log("无点限制");
                SetVelocity(PreferredVelocity);
                return;
            }

            //无解的情况
            if (error)
            {
                //取最近的点, 取最大速度
                Vector2 newSpeed = points[0].pos;

                for (int i = 1; i < points.Count; i++)
                {
                    if (points[i].pos.magnitude < newSpeed.magnitude)
                    {
                        newSpeed = points[i].pos;
                    }
                }
                //Debug.Log("无解");
                SetVelocity(newSpeed);
            }
            else//有解的情况
            {
                bool preferredInRange = false;
                for (int i = 0; i < lines.Count; i++)
                {
                    var delta = PreferredVelocity - lines[i].node;
                    if (Utils.GetRotateDir(delta, lines[i].dir) != RotateDir.Anticlockwise)//如果是逆时针则表明在范围内的一侧
                    {
                        preferredInRange = false;
                    }
                }

                if (preferredInRange)//如果期望速度在可选范围内
                {
                    //Debug.Log("期望可选");
                    SetVelocity(PreferredVelocity);
                }
                else
                {
                    Vector2 velocity = points[0].pos;
                    float minDis = Vector2.Distance(points[0].pos, speed);
                    for (int i = 1; i < points.Count; i++)
                    {
                        var dis = Vector2.Distance(points[i].pos, speed);
                        if (dis < minDis)
                        {
                            minDis = dis;
                            velocity = points[i].pos;
                        }
                    }
                    SetVelocity(velocity);
                }
            }
        }

        private void SetVelocity(Vector2 velocity)
        {
            rigidbody.velocity = velocity.normalized * Mathf.Min(velocity.magnitude, MaxSpeed);
        }

        private void AddORCAPos(List<ORCAPoint> list, Vector2 pos, ORCAPoint.Tag tag = ORCAPoint.Tag.Line)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (PosEquals(pos, list[i].pos))//位置近似相等就直接退出
                {
                    return;
                }
            }
            list.Add(new ORCAPoint
            {
                pos = pos,
                inRange = pos.magnitude < MaxSpeed,
                tag = tag
            });
        }

        private bool PosEquals(Vector2 vectorA, Vector2 vectorB)
        {
            float epsilon = 0.0001f; // 容忍度
            return (Mathf.Abs(vectorA.x - vectorB.x) < epsilon &&
                Mathf.Abs(vectorA.y - vectorB.y) < epsilon);
        }

        private bool HandleObjectORCA(Collider2D obj, out AreaLine orca)
        {
            var speedB = obj.GetComponent<Rigidbody2D>().velocity;
            var speedA = rigidbody.velocity;
            var collider = obj.GetComponent<CircleCollider2D>();

            var AB = (Vector2)obj.transform.position - (Vector2)transform.position;
            var r = collider.radius + this.collider.radius + width_ORCA;//把A看作点, B对A的半径
            var spAB = (speedA - speedB) * timeScale;//A对B的相对速度

            // 处理速度非常接近的情况
            if (Vector2.Distance(speedA, speedB) < 0.001f)
            {
                orca = new AreaLine();
                return false;
            }

            bool inCircle = (spAB - AB).magnitude < r;

            //这里r-width_ORCA的目的是, 为了让圆的边界小于投影边界, 否则容易出现临界抖动的问题
            float rad_AB_v = Mathf.Asin(r - width_ORCA / AB.magnitude);//AB 到 切线的角度大小
            float rad_AB_sp = AB.ClockAngle(spAB) * Mathf.Deg2Rad;//AB 到 sp 的角度

#if CUSTOM_TEST
            //绘制速度线
            DrawFromSelf(spAB, Color.green);
            //绘制障碍边界线
            DrawCircle(obj.transform.position, r, color_barrier);
            DrawFromSelf(AB.RotateRad(rad_AB_v), color_barrier);
            DrawFromSelf(AB.RotateRad(-rad_AB_v), color_barrier);
#endif

            if (AB.magnitude < r)
            {
                var change = -AB;
                //change 速度改变方向
                orca = new AreaLine()
                {
                    dir = change.normalized.Rotate(90),
                    node = change + speedA,
                    state = AreaLine.LineState.None
                };
                return true;
            }

            if (Mathf.Abs(rad_AB_sp) > rad_AB_v)//在投影三角外
            {
                orca = new AreaLine();
                return false;
            }

            var s_AB = AB.magnitude * Mathf.Cos(rad_AB_v);//AB 到 切线的投影长度
            var s_sp = spAB.magnitude * Mathf.Cos(rad_AB_v - Mathf.Abs(rad_AB_sp));// sp 到切线的投影长度

            bool inTriangle = s_sp < s_AB;//在三角区域内

            if (inTriangle)
            {
                if (inCircle)//在扇形范围内
                {
                    var delta = spAB - AB;
                    var change = delta.normalized * (r - delta.magnitude);
#if CUSTOM_TEST
                Debug.DrawLine(transform.position + (Vector3)spAB, transform.position + (Vector3)spAB + (Vector3)change, Color.red);
                Debug.DrawLine(transform.position, transform.position + (Vector3)speedA, Color.blue);

                var k = new AreaLine()
                {
                    dir = change.normalized,
                    node = speedA,
                    state = AreaLine.LineState.None
                };
                //k.Draw(transform.position, Color.red);
                Debug.DrawLine(transform.position + (Vector3)speedA, transform.position + (Vector3)speedA + (Vector3)change, Color.red);
#endif
                    //change 速度改变方向
                    orca = new AreaLine()
                    {
                        dir = change.normalized.Rotate(90),
                        node = change + speedA,
                        state = AreaLine.LineState.None
                    };
                    return true;
                }
                else
                {
                    orca = new AreaLine();
                    return false;
                }
            }
            else//在背后投影范围内
            {
                var rotateRad = rad_AB_v + Mathf.PI / 2;
                if (rad_AB_sp < 0)
                {
                    rotateRad = -rotateRad;
                }
                var dir = AB.RotateRad(rotateRad).normalized;//取得半平面的垂直向量
                var change = dir * s_sp * Mathf.Tan(rad_AB_v - Mathf.Abs(rad_AB_sp));

#if CUSTOM_TEST
                Debug.DrawLine(transform.position + (Vector3)spAB, transform.position + (Vector3)spAB + (Vector3)change, Color.red);
                Debug.DrawLine(transform.position, transform.position + (Vector3)speedA, Color.blue);

                var k = new AreaLine()
                {
                    dir = change.normalized,
                    node = speedA,
                    state = AreaLine.LineState.None
                };
                //k.Draw(transform.position, Color.red);
                Debug.DrawLine(transform.position + (Vector3)speedA, transform.position + (Vector3)speedA + (Vector3)change, Color.red);
#endif
                //change 速度改变方向
                orca = new AreaLine()
                {
                    dir = change.normalized.Rotate(90),
                    node = change + speedA,
                    state = AreaLine.LineState.None
                };
                return true;
            }
        }

        private void DrawCircle(Vector2 pos, float r, Color color)
        {
            Vector2 point = Vector2.down * r;
            int count = 36;
            for (int i = 0; i <= count; i++)
            {
                var next = point.RotateRad(Mathf.PI * 2 / count);
                Debug.DrawLine(pos + point, pos + next, color, checkCd * Time.fixedDeltaTime);
                point = next;
            }
        }

        private void DrawFromSelf(Vector3 dir, Color color)
        {
            Debug.DrawLine(transform.position, transform.position + dir, color, checkCd * Time.fixedDeltaTime);
        }

        private int checkCd = 2;//3tick检查一次
        private int ticker = 0;

        private void FixedUpdate()
        {
            ticker++;
            ticker %= checkCd;
            if (ticker == 0)
            {
                ORCAModifySpeed();
            }
        }
    }

    public struct ORCAPoint
    {
        public Vector2 pos;
        public Tag tag;
        public bool inRange;

        public enum Tag
        {
            /// <summary>
            /// 无效点
            /// </summary>
            None,

            /// <summary>
            /// 线上的点
            /// </summary>
            Line,

            /// <summary>
            /// 射线的点
            /// </summary>
            Shoot,

            /// <summary>
            /// 圆弧上的点
            /// </summary>
            Circle
        }
    }
}