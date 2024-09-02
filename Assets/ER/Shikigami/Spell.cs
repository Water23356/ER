// Ignore Spelling: Shikigami

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Shikigami
{
    /// <summary>
    /// 指令符
    /// </summary>
    public class Spell
    {
        /// <summary>
        /// 符名
        /// </summary>
        public string name;
        /// <summary>
        /// 指令组
        /// </summary>
        public Instruct[] instructs;

        public Spell()
        {
            name = string.Empty;
            instructs = new Instruct[0];
        }
        /// <summary>
        /// 是否为空符
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return instructs.Length == 0 && name == string.Empty;
        }

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"符卡名:{name}");
            Console.ForegroundColor = ConsoleColor.White;
            foreach ( var item in instructs )
            {
                item.Print();
            }
            Console.WriteLine();
        }
    }
}

