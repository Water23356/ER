using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using ER;
namespace Dev2
{
    /// <summary>
    /// 避障网格图
    /// </summary>
    public class STG_OA_Map : MonoSingleton<STG_OA_Map>
    {
        public GameObject prefab_text;
        public RectTransform canvas;

        [SerializeField]
        private Tilemap tilemap; // 需要填充的 Tilemap

        [SerializeField]
        private TileBase tile; // 你要填充的 Tile 资源

        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        /// <summary>
        /// 权重网格图
        /// </summary>
        private float[,] weightMap;

        private TMP_Text[,] texts;

        /// <summary>
        /// 单元大小
        /// </summary>
        [SerializeField]
        private float cellSize = 1;

        /// <summary>
        /// 实体代理
        /// </summary>
        [SerializeField]
        private List<STG_OA_Agent> agents = new List<STG_OA_Agent>();

        /// <summary>
        /// 预测倍率
        /// </summary>
        private float previewLength = 2f;

        private void Start()
        {
            Rebuild(width, height);
        }

        private void Rebuild(int width, int height)
        {
            weightMap = new float[width, height];
            texts = new TMP_Text[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var obj = GameObject.Instantiate(prefab_text, canvas);
                    obj.transform.position = GetCellCenter(x, y);
                    var text = obj.GetComponent<TMP_Text>();
                    texts[x, y] = text;
                }
            }

            tilemap.ClearAllTiles();
            DrawMap();
        }

        public float GetWeight(Vector2Int point)
        {
            if (IsInRange(point))
            {
                return weightMap[point.x, point.y];
            }
            return 0f;
        }

        public bool IsInRange(Vector2Int point)
        {
            return point.x >= 0 && point.x < width && point.y >= 0 && point.y < height;
        }

        public bool IsInRange(Vector3Int point)
        {
            return point.x >= 0 && point.x < width && point.y >= 0 && point.y < height;
        }

        public bool IsInRange(float x, float y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public float GetWeight(Vector3Int point)
        {
            if(IsInRange(point))
                return weightMap[point.x,point.y];
            return 9999f;
        }

        public Vector3Int GetGridPoint(Transform trf)
        {
            return tilemap.WorldToCell(trf.position);
        }

        public Vector3Int GetGridPoint(Vector2 worldPosition)
        {
            return tilemap.WorldToCell(worldPosition);
        }

        public void AddAgent(STG_OA_Agent agent)
        {
            if (agent == null) return;
            agents.Add(agent);
        }
        public void RemoveAgent(STG_OA_Agent agent)
        {
            agents.Remove(agent);
        }

        public void UpdateMap()
        {
            ClearWeight();
            var copy = agents.ToArray();
            foreach (var agent in copy)
            {
                ComputeAgentWeight(agent);
            }
            DrawMap();
        }

        private bool isUpdatingMap = false;

        public IEnumerator UpdateMapAsync()
        {
            if (isUpdatingMap) yield break;
            isUpdatingMap = true;
            ClearWeight();
            int counter = 0;
            var copy = agents.ToArray();
            foreach (var agent in copy)
            {
                ComputeAgentWeight(agent);
                counter += 1;
                if (counter % 30 == 0)//每检测30个单位就暂停 1 tick
                {
                    yield return new WaitForFixedUpdate();
                }
            }
            DrawMap();
            isUpdatingMap = false;
        }

        private void ComputeAgentWeight(STG_OA_Agent agent)
        {
            var start_point = GetGridPoint(agent.transform);
            var end_point = GetGridPoint((Vector2)agent.transform.position + agent.velocity);
            var radius = agent.radius;
            var weight = agent.velocity.magnitude;
            var dir = agent.velocity.normalized;
            Coordinate coordinate = new Coordinate(agent.transform.position, agent.velocity);

            Queue<Vector3Int> searchNode = new Queue<Vector3Int>();
            List<Vector3Int> searched = new List<Vector3Int>();
            searchNode.Enqueue(start_point);

            while (searchNode.Count > 0)
            {
                var node = searchNode.Dequeue();
                if (ComputeGridCellWeight(node, coordinate, radius, weight))
                {
                    AddSearchItem(dir, node, searchNode);
                }
                searched.Add(node);
            }

            //Debug.Log($"起点: {start_point} 终点: {end_point}");
            //对于起点和速度 建立坐标系

            Debug.DrawLine(GetCellCenter(start_point), GetCellCenter(end_point), Color.red);
            Debug.DrawLine(agent.transform.position, agent.transform.position + (Vector3)agent.velocity, Color.green);
        }

        private void AddSearchItem(Vector2 dir, Vector3Int point, Queue<Vector3Int> searchQueue)
        {
            float tolerance = 0.001f;

            int index = 5;
            if (dir.x > tolerance)
            {
                index = 6;
            }
            else if (dir.x < -tolerance)
            {
                index = 4;
            }

            if (dir.y > tolerance)
            {
                switch (index)
                {
                    case 5: index = 8; break;
                    case 4: index = 7; break;
                    case 6: index = 9; break;
                }
            }
            else if (dir.y < -tolerance)
            {
                switch (index)
                {
                    case 5: index = 2; break;
                    case 4: index = 1; break;
                    case 6: index = 3; break;
                }
            }

            switch (index)
            {
                case 1:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(-1, 0));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(0, -1));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(-1, -1));
                    break;

                case 2:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(0, -1));
                    break;

                case 3:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(1, 0));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(0, -1));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(1, -1));
                    break;

                case 4:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(-1, 0));
                    break;

                case 5:
                    break;

                case 6:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(1, 0));
                    break;

                case 7:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(-1, 1));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(-1, 0));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(0, 1));
                    break;

                case 8:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(1, 0));
                    break;

                case 9:
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(1, 1));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(1, 0));
                    AddQueueItemNoRepeat(searchQueue, point + new Vector3Int(0, 1));
                    break;
            }
        }

        private void AddQueueItemNoRepeat(Queue<Vector3Int> queue, params Vector3Int[] points)
        {
            foreach (var point in points)
            {
                if (!queue.Contains(point)) queue.Enqueue(point);
            }
        }

        private float ComputeHeightWeight(float heightDistance)
        {
            return 1 / (heightDistance + cellSize / (cellSize + 1)) - 1 / cellSize;
        }

        //计算指定单元格的权重, 如果该单元格权重无关则返回false
        private bool ComputeGridCellWeight(Vector3Int point, Coordinate coordinate, float radius, float inputWeight)
        {
            if (!IsInRange(point)) return false;

            var center = GetCellCenter(point);
            var point_w = coordinate.GetWeight(center);

            var heightWeight = ComputeHeightWeight(Mathf.Abs(point_w.y));
            if (point_w.x < 0)
            {
                weightMap[point.x, point.y] += inputWeight;
                return true;
            }
            var weight = (cellSize + inputWeight - Mathf.Abs(point_w.x)) * 10;

            if (heightWeight <= 0 || weight <= 0)//表示在可作用域外
            {
                return false;
            }
            weightMap[point.x, point.y] += weight * heightWeight;
            return true;
        }

        public Vector2 GetCellCenter(int x, int y)
        {
            return tilemap.GetCellCenterWorld(new Vector3Int(x, y));
        }

        public Vector2 GetCellCenter(Vector3Int pos)
        {
            return tilemap.GetCellCenterWorld(pos);
        }

        private void ClearWeight()
        {
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    weightMap[i, k] = 0f;
                }
            }
        }


        public void DrawMap()
        {
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    tilemap.SetTile(new Vector3Int(i, k), tile);
                    tilemap.SetColor(new Vector3Int(i, k), Color.Lerp(Color.white, Color.red, weightMap[i, k]));
                    texts[i, k].text = weightMap[i, k] + "";
                }
            }
        }

        [ContextMenu("重构地图")]
        private void _Rebuild()
        {
            Rebuild(width, height);
        }

        private void FixedUpdate()
        {
            if (!isUpdatingMap)
            {
                StartCoroutine(UpdateMapAsync());
            }
        }

        private class Coordinate
        {
            public Vector2 origin;
            public Vector2 x_dir;
            public Vector2 y_dir;

            public Coordinate(Vector2 origin, Vector2 defaultX)
            {
                this.origin = origin;
                x_dir = defaultX.normalized;
                y_dir = new Vector2(x_dir.y, -x_dir.x);
            }

            /// <summary>
            /// 取得指定坐标在该坐标系下的分量权重
            /// </summary>
            /// <param name="otherPos"></param>
            /// <returns></returns>
            public Vector2 GetWeight(Vector2 otherPos)
            {
                return new Vector2(Vector2.Dot(otherPos - origin, x_dir), Vector2.Dot(otherPos - origin, y_dir));
            }

            public Vector2 GetWeightAbs(Vector2 otherPos)
            {
                return new Vector2(Mathf.Abs(Vector2.Dot(otherPos - origin, x_dir)), Mathf.Abs(Vector2.Dot(otherPos - origin, y_dir)));
            }
        }
    }
}