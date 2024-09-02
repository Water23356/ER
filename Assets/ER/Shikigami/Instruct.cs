using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Shikigami
{
    /// <summary>
    /// 指令:
    /// 指令头建议以 大写字母开头 的英文单词组成
    /// </summary>
    public class Instruct
    {
        /// <summary>
        /// 指令名
        /// </summary>
        public string name;
        /// <summary>
        /// 标记组(参数组)
        /// </summary>
        public string[] marks;

        public Instruct()
        {
            name = string.Empty;
            marks = new string[0];
        }

        public bool IsEmpty()
        {
            return name.Length == 0 && name == string.Empty;
        }

        public void Print()
        {
            Console.Write(name);
            Console.Write(':');
            foreach (var mks in marks)
            {
                Console.Write('\"');
                Console.Write(mks);
                Console.Write('\"');
                Console.Write(',');
            }
            Console.WriteLine();
        }

    }
}
