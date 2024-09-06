using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    /// <summary>
    /// 弹幕打印机(打印图形)
    /// </summary>

    public class ProjectilePrinterForTick : RoleAction
    {
        [DisplayLabel("弹幕初速度")]
        [SerializeField]
        private float speed;

        [SerializeField]
        private LineRenderer m_line;

        [Range(1, 30)]
        [Tooltip("该值越高, 绘制的约稀疏")]
        [DisplayLabel("绘制跳过数")]
        [SerializeField]
        private int skipCount = 3;

        private int tickCounter;
        private int layer;

        [SerializeField]
        private int shootCD = 3;

        private List<Vector2[]> printPos = new List<Vector2[]>();

        public Vector2 Position { get => Owner.transform.position; }
        public LineRenderer Line { get => m_line; set => m_line = value; }

        public ProjectilePrinterForTick() : base()
        {
            Timer.limitTick = 50;
        }

        private void Start()
        {
            GetShootPoints();
        }

        [ContextMenu("重新计算会绘制点")]
        private void GetShootPoints()
        {
            if (Line == null || Line.positionCount < 2) return;

            float maxDistance = 0;
            Vector2[] pos = new Vector2[Line.positionCount];
            for (int i = 0; i < Line.positionCount; i++)
            {
                pos[i] = Line.GetPosition(i) + Line.transform.position;
                maxDistance = Mathf.Max(maxDistance, Vector2.Distance(pos[i], Position));
            }

            float cell = Time.fixedDeltaTime * speed * skipCount;//单位距离
            int times = Mathf.FloorToInt(maxDistance / cell);//计算次数

            printPos.Clear();
            int k = times;
            while (k >= 0)
            {
                float r = cell * k;
                List<Vector2> layerPos = new List<Vector2>();
                for (int i = 1; i < pos.Length; i++)
                {
                    Vector2 pa = pos[i - 1];
                    Vector2 pb = pos[i];
                    Vector2[] result = Utils.FindPointsOnLineSegment(pa, pb, Position, r);
                    layerPos.Add(result);
                }
                if (Line.loop)
                {
                    Vector2 pa = pos[pos.Length - 1];
                    Vector2 pb = pos[0];
                    Vector2[] result = Utils.FindPointsOnLineSegment(pa, pb, Position, r);
                    layerPos.Add(result);
                }
                printPos.Add(layerPos.ToArray());
                k--;
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
                var moveSpeed = (pos[i] - Position).normalized * speed;
                Shoot(moveSpeed, Color.white);
                i++;
            }
        }

        public override void ActionEffect()
        {
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

            if(layer >= printPos.Count)
            {
                enabled = false;
            }
        }
    }
}