using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ER.Shikigami
{
    /// <summary>
    /// 指令解析器
    /// </summary>
    public class SpellParser
    {
        private StringBuilder tmp;//字符缓存
        private Instruct instruct;//指令缓存
        private List<Instruct> instructs;//指令组缓存
        private Spell spell;//符卡缓存
        private List<string> marks;//标记缓存
        private Func<char, State> operate;//字符处理委托: char: 当前传入字符 bool: 是否保留之前的字符缓存
        private List<Spell> book;//符卡书

        private bool spellEnable = false;//符卡是否激活

        private enum State//解析状态
        {
            /// <summary>
            /// 跳过
            /// </summary>
            Pass,
            /// <summary>
            /// //保留
            /// </summary>
            Reserve,
            /// <summary>
            /// //清除
            /// </summary>
            Clear,
            /// <summary>
            /// //跳过并清除
            /// </summary>
            End,
        }

        public SpellParser()
        {
            tmp = new StringBuilder();
            instructs = new List<Instruct>();
            marks = new List<string>();
            book = new List<Spell>();
        }
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        private void ClearBook()
        {
            book.Clear();
            marks.Clear();
            instructs.Clear();
            tmp.Clear(); ;
            instruct = null;
            spell = null;
            operate = WaitStart;
        }

        /// <summary>
        /// 解析符卡文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Spell[] ParseSpellFromFile(string path)
        {
            StreamReader rd = new StreamReader(path);
            string sp = rd.ReadToEnd();
            return ParseSpell(sp);
        }

        /// <summary>
        /// 解析符卡
        /// </summary>
        /// <returns></returns>
        public Spell[] ParseSpell(string text)
        {
            ClearBook();
            int index = 0;
            while (index < text.Length)
            {
                switch (operate(text[index]))
                {
                    case State.Pass:
                        break;
                    case State.Reserve:
                        tmp.Append(text[index]);
                        break;
                    case State.Clear:
                        tmp.Clear();
                        break;

                    case State.End:
                        tmp.Clear();
                        break;
                }
                index++;
            }
            if(!spell.IsEmpty())
            {
                spell.instructs = instructs.ToArray();
                book.Add(spell);
            }
            return book.ToArray();
        }

        /// <summary>
        /// 解析指令
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Instruct[] ParseInstruct(string text)
        {
            ClearBook() ;
            int index = 0;
            Spell dc = new Spell();
            dc.name = "default";
            book.Add(dc);
            while (index < text.Length)
            {
                switch (operate(text[index]))
                {
                    case State.Pass:
                        break;
                    case State.Reserve:
                        tmp.Append(text[index]);
                        break;
                    case State.Clear:
                        tmp.Clear();
                        break;

                    case State.End:
                        tmp.Clear();
                        break;
                }
                index++;
            }
            if (!spell.IsEmpty())
            {
                spell.instructs = instructs.ToArray();
                book.Add(spell);
            }
            return book[0].instructs;
        }

        /// <summary>
        /// 等待定义起始符号
        /// </summary>
        /// <returns></returns>
        private State WaitStart(char input)
        {
            switch (input)
            {
                case '[':
                    //Console.WriteLine("新的符卡定义");
                    if(spellEnable)//是否已经在符卡域内
                    {
                        //完成当前符卡封装
                        if(!spell.IsEmpty())
                        {
                            spell.instructs = instructs.ToArray();
                            instructs.Clear();
                            book.Add(spell);
                        }
                    }
                    //建立新的符卡
                    spell = new Spell();
                    spellEnable = true;
                    //封装符卡名称
                    operate = PackSpell;
                    return State.End;
                case '$':
                    if(spellEnable)
                    {
                        //创建新的指令
                        instruct = new Instruct();
                        marks.Clear();
                        operate = PackInstruct;
                        return State.End;
                    }
                    else
                    {
                        //Console.WriteLine($"出现未定义符指令, 这部分将会被自动省略->{input}");
                        return State.Pass;
                    }
                default:
                    return State.Pass;
            }
        }
        /// <summary>
        /// 等待指令结束
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private State WaitEnd(char input)
        {
            switch(input)
            {
                case '$':
                    //完成当前标记封装, 结束指令封装
                    marks.Add(ConvertChar(tmp.ToString()));
                    instruct.marks = marks.ToArray();
                    marks.Clear();
                    operate = WaitStart;

                    instructs.Add(instruct);
                    //Console.WriteLine("完成标记定义");
                    //Console.WriteLine("完成指令定义:");
                    return State.End;
                case ',':
                    //Console.WriteLine("完成标记定义");
                    marks.Add(ConvertChar(tmp.ToString()));
                    return State.End;
                default: 
                    return State.Reserve;
            }
        }
        /// <summary>
        /// 封装符卡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private State PackSpell(char input)
        {
            switch(input)
            {
                case ']':
                    //封装名称
                    spell.name = ConvertChar(tmp.ToString());

                    //Console.WriteLine("完成符卡名定义"+spell.name);

                    //等待开始
                    operate = WaitStart;
                    //清空缓存
                    return State.End;//完成名称封装
                default:
                    return State.Reserve;//保留缓存
            }
        }
        /// <summary>
        /// 封装指令
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private State PackInstruct(char input)
        {
            switch(input)
            {
                case ',':
                    //Console.WriteLine("完成指令名定义");
                    //完成名称封装 且 有后续标记
                    instruct.name = ConvertChar(tmp.ToString());
                    operate = WaitEnd;
                    return State.End;//清空缓存
                case '$':
                    //Console.WriteLine("完成指令名定义");
                    //Console.WriteLine("完成指令定义");
                    //完成名称封装 且 无后续标记, 直接结束指令封装
                    instruct.name = ConvertChar(tmp.ToString());
                    instructs.Add(instruct);
                    operate = WaitStart;
                    return State.End;
                default:
                    return State.Reserve;//保留缓存
            }
        }

        public static string ConvertChar(string raw)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            bool ct = false;
            while (index < raw.Length)
            {
                if (ct)//转义
                {
                    ct = false;
                    switch (raw[index])
                    {
                        case '\\':
                            sb.Append('\\');
                            break;
                        case 'd':
                            sb.Append(',');
                            break;
                        case 'n':
                            sb.Append("\n");
                            break;
                        case 't':
                            sb.Append("\t");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (raw[index])
                    {
                        case '\\':
                            ct = true;
                            break;
                        default:
                            sb.Append(raw[index]);
                            break;
                    }
                }
                index++;
            }
            return sb.ToString();
        }
    }
}