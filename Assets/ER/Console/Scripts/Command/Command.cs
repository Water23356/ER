using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace ER
{
    /// <summary>
    /// 指令
    /// </summary>
    public class Command : IEnumerable<Data>
    {
        private Data[] datas;
        private int index = 0;

        public Command(Data[] datas)
        {
            this.datas = datas;
        }

        /// <summary>
        /// 获取下一个字段的名称
        /// </summary>
        /// <returns></returns>
        public string NextFieldName
        {
            get => datas[index++].ToString();
        }
        public bool isEmpty
        {
            get
            {
                return datas == null || datas.Length == 0;
            }
        }

        public bool isEnd
        {
            get => index >= datas.Length;
        }

        public void MoveTo(int index)
        {
            this.index = index;
        }

        public void Move(int step)
        {
            index += step;
            index = Math.Max(index, 0);
        }

        public int NextInt(int defaultValue = 0)
        {
            int value = Data.GetIntData(datas, index, defaultValue);
            index++;
            return value;
        }

        public string NextText(string defaultValue = "")
        {
            string value = Data.GetTextData(datas, index, defaultValue);
            index++;
            return value;
        }

        public bool NextBool(bool defaultValue = false)
        {
            bool value = Data.GetBoolData(datas, index, defaultValue);
            index++;
            return value;
        }

        public float NextFloat(float defaultValue = 0f)
        {
            float value = Data.GetFloatData(datas, index, defaultValue);
            index++;
            return value;
        }

        public Data NextData()
        {
            if (index < 0 || index >= datas.Length)
                return Data.Error;
            return datas[index++];
        }
   
        /// <summary>
        /// 当前索引及之后的字段被切割出来作为指令参数
        /// </summary>
        /// <returns></returns>
        public Data[] SplitParams()
        {
            Data[] cparams = new Data[datas.Length - index];//剩余参数被打包
            Array.Copy(datas, index, cparams, 0, cparams.Length);
            return cparams;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (var d in datas)
            {
                sb.Append(d.ToString());
                sb.Append(' ');
            }
            sb.Append("]");
            return sb.ToString();
        }

        public IEnumerator<Data> GetEnumerator()
        {
            foreach (var d in datas)
            {
                yield return d;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var d in datas)
            {
                yield return d;
            }
        }

        public Command Copy()
        {
            return new Command(datas);
        }
    }

}