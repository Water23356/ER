using UnityEngine;

namespace ER
{
    /// <summary>
    /// 元素行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayMapRow<T>
    {
        public enum RowType
        { Row, Column }

        private RowType rowType;

        /// <summary>
        /// 列表模式:行/列
        /// </summary>
        public RowType Type => rowType;

        private ArrayMap<T> map;
        private int lay;//所在行/列

        private int[] array;//元素索引组
        private int length;//行长度

        /// <summary>
        /// 索引指针
        /// </summary>
        public int index_hand;

        /// <summary>
        /// 行长度
        /// </summary>
        public int Length => length;

        /// <summary>
        ///
        /// </summary>
        /// <param name="map">所属地图对象</param>
        /// <param name="rowType">列表模式:行/列</param>
        /// <param name="lay">所在行/列号</param>
        /// <param name="length">行/列的元素个数</param>
        public ArrayMapRow(ArrayMap<T> map, RowType rowType, int lay, int length)
        {
            this.length = length;
            array = new int[length];
            index_hand = -1;
            this.rowType = rowType;
            this.lay = lay;
        }

        /// <summary>
        /// 访问指定元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                switch (rowType)
                {
                    case RowType.Row:
                        return map[array[index], lay];

                    case RowType.Column:
                        return map[lay, array[index]];
                }
                Debug.Log("元素行模式异常!");
                return default(T);
            }
            set
            {
                switch (rowType)
                {
                    case RowType.Row:
                        map[array[index], lay] = value;
                        break;

                    case RowType.Column:
                        map[lay, array[index]] = value;
                        break;
                }
            }
        }

        /// <summary>
        /// 获取上一个元素(索引指针前移)
        /// </summary>
        /// <returns></returns>
        public T GetPreviousValue()
        {
            index_hand--;
            return this[index_hand];
        }

        /// <summary>
        /// 获取当前索引指针指向的元素
        /// </summary>
        /// <returns></returns>
        public T GetThisValue()
        {
            return this[index_hand];
        }

        /// <summary>
        /// 获取下一个元素(索引指针后移)
        /// </summary>
        /// <returns></returns>
        public T GetNextValue()
        {
            index_hand++;
            return this[index_hand];
        }

        /// <summary>
        /// 尝试获取上一个元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetPreviousValue(out T value)
        {
            index_hand--;
            if (IsIndexInRange())
            {
                value = this[index_hand];
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// 尝试获取当前元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetThisValue(out T value)
        {
            if (IsIndexInRange())
            {
                value = this[index_hand];
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// 尝试获取下一个元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetNextValue(out T value)
        {
            index_hand++;
            if (IsIndexInRange())
            {
                value = this[index_hand];
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// 判断当前索引指针是否在有效范围内
        /// </summary>
        /// <returns></returns>
        public bool IsIndexInRange()
        {
            return index_hand > -1 && index_hand < length;
        }

        /// <summary>
        /// 将索引指针设为0(开头元素)
        /// </summary>
        public void SetIndexFirst()
        {
            index_hand = 0;
        }

        /// <summary>
        /// 将索引指针设为length-1(末尾元素)
        /// </summary>
        public void SetIndexLast()
        {
            index_hand = length - 1;
        }

        /// <summary>
        /// 获取一个空行对象
        /// </summary>
        public static ArrayMapRow<T> Empty
        {
            get
            {
                ArrayMapRow<T> row = new ArrayMapRow<T>(null, 0, 0, 0);
                return row;
            }
        }

        /// <summary>
        /// 判断自身是否为空行
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return length <= 0;
        }
    }

    /// <summary>
    /// 二维地图
    /// </summary>
    public class ArrayMap<T>
    {
        protected T[] array;//元素组
        protected int width;//地图宽度
        protected int height;//地图长度

        /// <summary>
        /// 总元素个数
        /// </summary>
        public int Size => array.Length;

        /// <summary>
        /// 地图宽度
        /// </summary>
        public int Width => width;

        /// <summary>
        /// 地图长度
        /// </summary>
        public int Height => height;

        /// <summary>
        /// x索引指针(列索引)
        /// </summary>
        public int index_x_hand;

        /// <summary>
        /// y索引指针(行索引)
        /// </summary>
        public int index_y_hand;

        public ArrayMap(int width, int height)
        {
            this.array = new T[width * height];
            this.width = width;
            this.height = height;
            index_x_hand = -1;
            index_y_hand = -1;
        }

        #region 行处理

        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <returns></returns>
        public ArrayMapRow<T> GetRow(int y)
        {
            ArrayMapRow<T> row = new ArrayMapRow<T>(this, ArrayMapRow<T>.RowType.Row, y, width);
            for (int i = 0; i < width; i++)
            {
                row[i] = this[i, y];
            }
            return row;
        }

        /// <summary>
        /// 获取上一行
        /// </summary>
        /// <returns></returns>
        public ArrayMapRow<T> GetPreviousRow()
        {
            index_y_hand--;
            return GetRow(index_y_hand);
        }

        /// <summary>
        /// 获取当前行
        /// </summary>
        /// <returns></returns>
        public ArrayMapRow<T> GetThisRow()
        {
            return GetRow(index_y_hand);
        }

        /// <summary>
        /// 获取下一行
        /// </summary>
        /// <returns></returns>
        public ArrayMapRow<T> GetNextRow()
        {
            index_y_hand++;
            return GetRow(index_y_hand);
        }

        /// <summary>
        /// 尝试获取上一行
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool TryGetPreviousRow(out ArrayMapRow<T> row)
        {
            index_y_hand--;
            if (IsInRangeY(index_y_hand))
            {
                row = GetRow(index_y_hand);
                return true;
            }
            row = ArrayMapRow<T>.Empty;
            return false;
        }

        /// <summary>
        /// 尝试获取当前行
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool TryGetThisRow(out ArrayMapRow<T> row)
        {
            if (IsInRangeY(index_y_hand))
            {
                row = GetRow(index_y_hand);
                return true;
            }
            row = ArrayMapRow<T>.Empty;
            return false;
        }

        /// <summary>
        /// 尝试获取下一行
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool TryGetNextRow(out ArrayMapRow<T> row)
        {
            index_y_hand++;
            if (IsInRangeY(index_y_hand))
            {
                row = GetRow(index_y_hand);
                return true;
            }
            row = ArrayMapRow<T>.Empty;
            return false;
        }

        /// <summary>
        /// 判断指定行是否存在
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInRangeY(int y)
        {
            return y > -1 && y < this.height;
        }

        /// <summary>
        /// 设置行索引为头行
        /// </summary>
        public void SetIndexFirstY()
        {
            index_y_hand = 0;
        }

        /// <summary>
        /// 设置行索引为尾行
        /// </summary>
        public void SetIndexLastY()
        {
            index_y_hand = height - 1;
        }

        #endregion 行处理

        #region 列处理

        /// <summary>
        /// 获取指定列
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public ArrayMapRow<T> GetColumn(int x)
        {
            ArrayMapRow<T> row = new ArrayMapRow<T>(this, ArrayMapRow<T>.RowType.Column, x, height);
            for (int i = 0; i < height; i++)
            {
                row[i] = this[x, i];
            }
            return row;
        }

        /// <summary>
        /// 获取上一列
        /// </summary>
        /// <returns></returns>
        public ArrayMapRow<T> GetPreviousColumn()
        {
            index_x_hand--;
            return GetColumn(index_x_hand);
        }

        /// <summary>
        /// 获取当前列
        /// </summary>
        /// <returns></returns>
        public ArrayMapRow<T> GetThisColumn()
        {
            return GetColumn(index_x_hand);
        }

        /// <summary>
        /// 获取下一列
        /// </summary>
        /// <returns></returns>
        public ArrayMapRow<T> GetNextColumn()
        {
            index_x_hand++;
            return GetColumn(index_x_hand);
        }

        /// <summary>
        /// 尝试获取上一列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool TryGetPreviousColumn(out ArrayMapRow<T> row)
        {
            index_x_hand--;
            if (IsInRangeX(index_x_hand))
            {
                row = GetColumn(index_x_hand);
                return true;
            }
            row = ArrayMapRow<T>.Empty;
            return false;
        }

        /// <summary>
        /// 尝试获取当前列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool TryGetThisColumn(out ArrayMapRow<T> row)
        {
            if (IsInRangeX(index_x_hand))
            {
                row = GetColumn(index_x_hand);
                return true;
            }
            row = ArrayMapRow<T>.Empty;
            return false;
        }

        /// <summary>
        /// 尝试获取下一列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool TryGetNextColumn(out ArrayMapRow<T> row)
        {
            index_x_hand++;
            if (IsInRangeX(index_x_hand))
            {
                row = GetColumn(index_x_hand);
                return true;
            }
            row = ArrayMapRow<T>.Empty;
            return false;
        }

        /// <summary>
        /// 判断指定列是否存在
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool IsInRangeX(int x)
        {
            return x > -1 && x < this.width;
        }

        /// <summary>
        /// 设置列索引为头列
        /// </summary>
        public void SetIndexFirstX()
        {
            index_x_hand = 0;
        }

        /// <summary>
        /// 设置列索引为尾列
        /// </summary>
        public void SetIndexLastX()
        {
            index_x_hand = width - 1;
        }

        #endregion 列处理

        /// <summary>
        /// 判断点是否在地图范围内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsPointInRange(int x, int y)
        {
            return x > -1 && x < width && y > -1 && y < height;
        }

        /// <summary>
        /// 获取指定元素
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T this[int x, int y]
        {
            get => array[y * width + x];
            set => array[y * width + x] = value;
        }
    }
}