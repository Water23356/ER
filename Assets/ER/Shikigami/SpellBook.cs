using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ER;
namespace ER.Shikigami
{
    /// <summary>
    /// 符卡仓库(缓存系统)
    /// </summary>
    public sealed class SpellBook:Singleton<SpellBook>
    {
        private Dictionary<string, Spell> dic;//符卡缓存库

        private static SpellParser parser = new SpellParser();

        public SpellBook()
        {
            dic = new Dictionary<string, Spell>();
        }

        /// <summary>
        /// 获取指定符卡
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns></returns>
        public Spell GetSpell(string spellName)
        {
            return new Spell();
        }
        /// <summary>
        /// 从指定文件冲读取符卡
        /// </summary>
        /// <param name="path"></param>
        public void LoadSpell(string path)
        {
            Spell[] spells = ParseSpellFromFile(path);
            foreach (Spell spell in spells)
            {
                dic[spell.name] = spell;
            }
        }
        /// <summary>
        /// 清除符卡缓存
        /// </summary>
        public void Clear()
        {
            dic.Clear();
        }
        /// <summary>
        /// 删除指定符卡缓存
        /// </summary>
        /// <param name="spellName"></param>
        public void ReleaseSpell(string spellName)
        {
            dic.Remove(spellName);
        }
        /// <summary>
        /// 判断指定符卡是否存在
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns></returns>
        public bool Contains(string spellName)
        {
            return dic.ContainsKey(spellName);
        }
        /// <summary>
        /// 获取指定符卡
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns></returns>
        public Spell this[string spellName]
        {
            get=>GetSpell(spellName);
        }

        /// <summary>
        /// 解析指定符卡字符串
        /// </summary>
        /// <param name="spellText"></param>
        /// <returns></returns>
        public static Spell[] ParseSpell(string spellText)
        {
            return parser.ParseSpell(spellText);
        }
        /// <summary>
        /// 解析指定符卡文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Spell[] ParseSpellFromFile(string path)
        {
            return parser.ParseSpellFromFile(path);
        }
        /// <summary>
        /// 解析指定指令字符串
        /// </summary>
        /// <param instructText="">指令字符串</param>
        /// <returns></returns>
        public static Instruct[] ParseInstruct(string instructText)
        {
            return parser.ParseInstruct(instructText);
        }

    }
}
