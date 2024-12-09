using System;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 解析数据，包含一个数据本体(object)，以及它的真实数据类型
    /// </summary>
    public struct Data
    {
        #region 属性

        public object Value { get; private set; }
        public DataType Type { get; private set; }

        #endregion 属性

        #region 静态

        /// <summary>
        /// 通知委托
        /// </summary>
        public static event Action<string> Output = message => Console.WriteLine(message);

        /// <summary>
        /// 获取指定字符串的数据解析
        /// </summary>
        /// <param name="dataString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Data ParseTo(string dataString, DataType type = DataType.Unknown)
        {
            Data data = new Data();
            data.Parse(dataString, type);
            return data;
        }

        /// <summary>
        /// 获取一个表示空的解析数据
        /// </summary>
        public static Data Empty => new Data(null, DataType.Unknown);

        /// <summary>
        /// 获取一个表示错误的解析数据
        /// </summary>
        public static Data Error => new Data(null, DataType.Error);

        public static DataType Parse(string dataString, out object Value, DataType type = DataType.Unknown)
        {
            switch (type)
            {
                case DataType.Unknown:
                    if (dataString.TryParseInt(out int iv))
                    {
                        Value = iv;
                        return DataType.Int;
                    }
                    else if (dataString.TryParseFloat(out float fv))
                    {
                        Value = fv;
                        return DataType.Float;
                    }
                    else if (dataString.TryParseBoolean(out bool bv))
                    {
                        Value = bv;
                        return DataType.Bool;
                    }
                    Value = dataString;
                    return DataType.String;

                case DataType.Int:
                    if (dataString.TryParseInt(out int iv0))
                    {
                        Value = iv0;
                        return DataType.Int;
                    }
                    Output("格式错误，转换Int类型失败");
                    Value = null;
                    return DataType.Error;

                case DataType.Float:
                    if (dataString.TryParseFloat(out float fv0))
                    {
                        Value = fv0;
                        return DataType.Int;
                    }
                    Output("格式错误，转换Int类型失败");
                    Value = null;
                    return DataType.Error;

                case DataType.Bool:
                    if (dataString.TryParseBoolean(out bool bv0))
                    {
                        Value = bv0;
                        return DataType.Bool;
                    }
                    Output("格式错误，转换Boolean类型失败");
                    Value = null;
                    return DataType.Error;

                case DataType.String:
                    Value = dataString;
                    return DataType.String;

                case DataType.Prop:
                    Value = dataString;
                    return DataType.Prop;

                case DataType.Command:
                    Value = dataString;
                    return DataType.Command;

                default:
                    Output("类型枚举出错！转化失败！");
                    Value = null;
                    return DataType.Error;
            }
        }

        #endregion 静态

        public static Data ErrorLog(string message)
        {
            return new Data(message, DataType.Error);
        }

        public Data(object value, DataType type)
        {
            Value = value;
            Type = type;
        }

        public Data(object value)
        {
            Value = value;
            if (Value is int)
            {
                Type = DataType.Int;
            }
            else if (Value is float)
            {
                Type = DataType.Float;
            }
            else if (Value is bool)
            {
                Type = DataType.Bool;
            }
            else if (Value is string)
            {
                Type = DataType.String;
            }
            else
            {
                Type = DataType.Unknown;
            }
        }

        #region 方法

        public int Int
        {
            get
            {
                switch (Type)
                {
                    case DataType.Int:
                        return (int)Value;

                    case DataType.Float:
                        return (int)(float)Value;

                    default:
                        Debug.LogError("数据转化出错");
                        return 0;
                }
            }
        }

        public float Float
        {
            get
            {
                switch (Type)
                {
                    case DataType.Int:
                        return (int)Value;

                    case DataType.Float:
                        return (float)Value;

                    default:
                        Debug.LogError("数据转化出错");
                        return 0;
                }
            }
        }

        public double Double
        {
            get
            {
                switch (Type)
                {
                    case DataType.Int:
                        return (int)Value;

                    case DataType.Float:
                        return (float)Value;

                    default:
                        //Debug.LogError("数据转化出错");
                        return 0;
                }
            }
        }

        public string String
        {
            get
            {
                return Value.ToString();
            }
        }
        public bool Boolean
        {
            get { return (bool)Value; }
        }

        public Command Command
        {
            get
            {
                if (Type == DataType.Command)
                    return (Command)Value;
                return null;
            }
        }
        public new string ToString() => Value.ToString();



        /// <summary>
        /// 此数据是否为空数据
        /// </summary>
        /// <returns></returns>
        public bool isEmpty
        {
            get => Value == null;
        }

        /// <summary>
        /// 此数据是否为错误数据
        /// </summary>
        /// <returns></returns>
        public bool isError
        {
            get => Type == DataType.Error;
        }

        /// <summary>
        /// 解析字符串，更新为解析数据
        /// </summary>
        /// <param name="dataString"></param>
        /// <param name="type"></param>
        /// <returns>知否解析成功</returns>
        public bool Parse(string dataString, DataType type = DataType.Unknown)
        {
            object v;
            Type = Parse(dataString, out v, type);
            if (isError)
            {
                Value = null;
                return false;
            }
            Value = v;
            return true;
        }

        /// <summary>
        /// 输出解析数据信息
        /// </summary>
        public void Print()
        {
            if (Value != null)
            {
                Output($"[{Type}]: {Value}");
            }
            else
            {
                Output($"[{Type}]:");
            }
        }

        #endregion 方法

        public static string GetTextData(Data[] datas, int index, string defaultValue)
        {
            if (index.InRange(0, datas.Length - 1))
            {
                return datas[index].ToString();
            }
            return defaultValue;
        }

        public static int GetIntData(Data[] datas, int index, int defaultValue)
        {
            if (index.InRange(0, datas.Length - 1))
            {
                if (datas[index].Type == DataType.Int || datas[index].Type == DataType.Float || datas[index].Type == DataType.Float)
                {
                    return datas[index].Int;
                }
            }
            return defaultValue;
        }

        public static float GetFloatData(Data[] datas, int index, float defaultValue)
        {
            if (index.InRange(0, datas.Length - 1))
            {
                if (datas[index].Type == DataType.Int || datas[index].Type == DataType.Float || datas[index].Type == DataType.Float)
                {
                    return datas[index].Float;
                }
            }
            return defaultValue;
        }

        public static bool GetBoolData(Data[] datas, int index, bool defaultValue)
        {
            if (index.InRange(0, datas.Length - 1))
            {
                if (datas[index].Type == DataType.Bool)
                {
                    return datas[index].Boolean;
                }
            }
            return defaultValue;
        }
    }
}