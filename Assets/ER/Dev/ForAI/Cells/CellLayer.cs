using System;

namespace Dev.AI
{
    [Serializable]
    public class CellLayer
    {
        public Cell[] cells;

        public int size=>cells.Length;

        /// <summary>
        /// 激活函数
        /// </summary>
        private Func<double, double> actFunc;

        public CellLayer()
        { }

        public CellLayer(int inputCount, int cellCount)
        { Init(inputCount, cellCount); }

        public CellLayer(int inputCount, int cellCount, Func<double, double> actFunc)
        { Init(inputCount, cellCount, actFunc); }

        /// <summary>
        /// 初始化: 定义输入端口数量 和 细胞数量(输出值数量), 默认使用 ReLU 作为激活函数
        /// </summary>
        /// <param name="inputCount"></param>
        /// <param name="cellCount"></param>
        public void Init(int inputCount, int cellCount)
        {
            cells = new Cell[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                cells[i] = new Cell(inputCount);
            }
            actFunc = ReLU;
        }

        /// <summary>
        /// 初始化: 定义输入端口数量 和 细胞数量(输出值数量), 并设定激活函数
        /// </summary>
        /// <param name="inputCount"></param>
        /// <param name="cellCount"></param>
        public void Init(int inputCount, int cellCount, Func<double, double> actFunc)
        {
            cells = new Cell[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                cells[i] = new Cell(inputCount);
            }
            this.actFunc = actFunc;
        }

        /// <summary>
        /// 更新细胞层状态, 并返回更新后的细胞层输出值
        /// </summary>
        /// <param name="inputValues"></param>
        public double[] UpdateState(double[] inputValues)
        {
            foreach (var cell in cells)
            {
                cell.UpdateState(inputValues, actFunc);
            }
            return GetValues();
        }

        /// <summary>
        /// 获取细胞层输出值
        /// </summary>
        /// <returns></returns>
        public double[] GetValues()
        {
            double[] values = new double[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                values[i] = cells[i].c;
            }
            return values;
        }

        public static double ReLU(double input)
        {
            return Math.Max(input, 0);
        }

        public void Reset()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].c = 0;
            }
        }

        public CellLayer Copy()
        {
            var cellsCopy = new Cell[cells.Length];
            for(int i=0;i<cellsCopy.Length;i++)
            {
                cellsCopy[i] = cells[i].Copy();
            }
            return new CellLayer
            {
                cells = cellsCopy,
                actFunc = actFunc
            };
        }
    }

    public class CellLayerControlAgent
    {
    }
}