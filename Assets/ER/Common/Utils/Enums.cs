namespace ER
{
    /// <summary>
    /// 旋转方向
    /// </summary>
    public enum RotateDir
    {
        /// <summary>
        /// 逆时针
        /// </summary>
        Anticlockwise,

        /// <summary>
        /// 顺时针
        /// </summary>
        Clockwise,

        /// <summary>
        /// 平行
        /// </summary>
        Parallel
    }

    /// <summary>
    /// 4方向枚举
    /// </summary>
    public enum Dir4
    {
        /// <summary>
        /// 无方向(错误方向)
        /// </summary>
        None,

        /// <summary>
        /// 上
        /// </summary>
        Up,

        /// <summary>
        /// 下
        /// </summary>
        Down,

        /// <summary>
        /// 左
        /// </summary>
        Left,

        /// <summary>
        /// 右
        /// </summary>
        Right,
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 未知类型，默认类型
        /// </summary>
        Unknown,

        /// <summary>
        /// 整型
        /// </summary>
        Int,

        /// <summary>
        /// 浮点型
        /// </summary>
        Float,

        /// <summary>
        /// 布尔型
        /// </summary>
        Bool,

        /// <summary>
        /// 文本型
        /// </summary>
        String,

        /// <summary>
        /// 指令
        /// </summary>
        Command,
        /// <summary>
        /// 动态参数
        /// </summary>
        Prop,

        /// <summary>
        /// 错误类型
        /// </summary>
        Error,
    }
}