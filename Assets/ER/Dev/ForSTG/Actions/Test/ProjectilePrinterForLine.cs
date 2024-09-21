using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    /// <summary>
    /// 线绘制器
    /// </summary>
    public class ProjectilePrinterForLine : RoleAction, ILineGetter
    {
        [DisplayLabel("弹幕初速度")]
        [SerializeField]
        private float speed;

        [DisplayLabel("单元距离")]
        [SerializeField]
        private float cellDistance;

        [SerializeField]
        private int shootCD = 3;

        [SerializeField]
        private List<Line> lines = new List<Line>();

        [SerializeField]
        private LineSetter lineSetter;

        private int tickCounter;
        private int maxLayer;
        private int layer;

        private List<Vector2[]> printPos = new List<Vector2[]>();

        public Vector2 Position { get => Owner.transform.position; }
        private PriorityQueue<Vector2> m_queue;

        private PriorityQueue<Vector2> Queue
        {
            get
            {
                if (m_queue == null)
                    m_queue = new PriorityQueue<Vector2>(Comaperer);
                return m_queue;
            }
        }

        public ProjectilePrinterForLine() : base()
        {
            Timer.limitTick = 50;
        }

        private void Start()
        {
            lineSetter.Getter = this;
        }

        private bool Comaperer(Vector2 a, Vector2 b)
        {
            return (a).magnitude > (b).magnitude; // 距离远的放前面
        }

        private void AddLineNode(Vector2 pa, Vector2 pb)
        {
            int count = Mathf.RoundToInt(Vector2.Distance(pa, pb) / cellDistance);
            for (int i = 0; i <= count; i++)
            {
                Queue.Add(Vector2.Lerp(pa, pb, (float)i / count));
            }
        }

        [ContextMenu("重新计算会绘制点")]
        private void GetShootPoints()
        {
            Queue.Clear();
            GetMaxLayer();
            foreach (var line in lines)
            {
                GetNodeFromLine(line);
            }

            // 分层
            printPos.Clear();
            var node = Queue.FirtNode;
            var cellDis = shootCD * Time.fixedDeltaTime * speed; // 单元绘制层长度
            for (int i = maxLayer - 1; i > 0; i--)
            {
                float limitDis = cellDis * i; // 获取限定长度

                List<Vector2> layerPos = new List<Vector2>();
                if (node == null) break;
                var dis = node.Value.magnitude;
                // 把该层的点加入
                while (dis >= limitDis)
                {
                    layerPos.Add(node.Value);
                    node = node.Next;
                    if (node == null) break;
                    dis = node.Value.magnitude;
                }
                printPos.Add(layerPos.ToArray());
            }
        }

        private void GetMaxLayer()
        {
            float maxDistance = 0;
            foreach (var line in lines)
            {
                foreach (var point in line)
                {
                    maxDistance = Mathf.Max(maxDistance, point.magnitude);
                }
            }
            maxLayer = Mathf.CeilToInt(maxDistance / (shootCD * Time.fixedDeltaTime * speed)); // 最大层数
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="line"></param>
        private void GetNodeFromLine(Line line)
        {
            if (line.Count < 2) return;

            // 先获取全部要绘制的点
            for (int i = 1; i < line.Count; i++)
            {
                AddLineNode(line[i - 1], line[i]);
            }
            if (line.loop)
            {
                AddLineNode(line[0], line[line.Count - 1]);
            }
        }

        private void Print(int layer)
        {
            if (layer < 0 || layer >= printPos.Count)
            {
                return;
            }
            var pos = printPos[layer];

            int i = 0;
            while (i < pos.Length)
            {
                var moveSpeed = pos[i].normalized * speed;
                Shoot(moveSpeed, Color.white);
                i++;
            }
        }

        public override void ActionEffect()
        {
            lineSetter.UpdateSetLine(); // 更新绘制点
            GetShootPoints(); // 更新绘制方法

            tickCounter = 0;
            layer = 0;
            enabled = true;
        }

        private void FixedUpdate()
        {
            if (tickCounter % shootCD == 0)
            {
                Print(layer);
                layer++;
            }
            tickCounter++;

            if (layer >= printPos.Count)
            {
                enabled = false;
            }
        }

        public void ClearAllLine()
        {
            lines.Clear();
        }

        public void AddLine(Line newLine)
        {
            Debug.Log($"添加新的绘制线 [{lines.Count}]");
            lines.Add(newLine);
        }

        public void SetLines(Line[] lines)
        {
            this.lines.AddRange(lines);
            GetShootPoints();
        }
    }
}
