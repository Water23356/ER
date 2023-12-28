using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Shikigami
{
    /// <summary>
    /// 指令分配器
    /// </summary>
    public class InstructionAllocator
    {
        private Spell spell;//当前执行的的符卡
        private int index = -1;//当前执行的指令索引
        /// <summary>
        /// 当前执行的的符卡
        /// </summary>
        public Spell SpellCard { get => spell; set => spell = value; }
        /// <summary>
        /// 当前执行的指令索引
        /// </summary>
        public int Index { get => index; set => index = value; }
        /// <summary>
        /// 执行当前符卡
        /// </summary>
        public void Execute()
        {
            index = 0;
            AssignTasks();
        }
        /// <summary>
        /// 分配指令
        /// </summary>
        /// <param name="init">前置指令(优先执行)</param>
        protected void AssignTasks(Instruct init = null)
        {
            if(init!=null)
            {
                switch (init.name)
                {
                    case "Start"://指令开始标记(无实际意义)
                        break;
                    case "End"://指令终止执行
                        return;
                    case "Skip"://跳转至指定指令 <目标指令索引,目标符卡名称(可选)>
                        Skip(init);
                        break;
                    default:
                        if (MinionTask(init))
                            break;
                        else
                            return;
                }
            }
            if (index < 0 || index >= spell.instructs.Length) return;
            bool loop = false;
            do
            {
                Instruct ist = spell.instructs[index];
                switch (ist.name)
                {
                    case "Start"://指令开始标记(无实际意义)
                        loop = true;
                        break;
                    case "End"://指令终止执行
                        loop = false;
                        break;
                    case "Skip"://跳转至指定指令 <目标指令索引,目标符卡名称(可选)>
                        Skip(ist);
                        loop = true;
                        break;
                    default:
                        if (MinionTask(ist))
                            loop = true;
                        else
                            loop = false;
                            
                        break;
                }
                index++;
            }
            while (loop && index < spell.instructs.Length);
        }
        /// <summary>
        /// 其他指令分配
        /// </summary>
        /// <param name="ist"></param>
        /// <returns>是否无等待继续执行下一条指令</returns>
        protected virtual bool MinionTask(Instruct ist)
        {
            return true;
        }
        /// <summary>
        /// 跳转至指定条指令
        /// </summary>
        /// <param name="ist"></param>
        /// <returns></returns>
        private void Skip(Instruct ist)
        {
            if(ist.marks.Length==1)
            {
                int skip_index = ist.marks[0].ToInt();//跳转的指令索引
                index = skip_index;
            }
            else if(ist.marks.Length>=2)
            {
                int skip_index = ist.marks[0].ToInt();//跳转的指令索引
                string cardName = ist.marks[1];
                spell = SpellBook.Instance.GetSpell(cardName);
                index = skip_index;
            }
        }
    }
}
