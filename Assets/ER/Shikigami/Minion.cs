using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Shikigami
{
    /// <summary>
    /// 式神接口
    /// </summary>
    public interface Minion
    {
        /// <summary>
        /// 式神执行指令的接口
        /// </summary>
        /// <param name="ist">原初指令</param>
        /// <param name="callback">回调函数</param>
        /// <returns>是否继续执行指令, 如果执行该指令会发生阻塞需要返回 false, 并且在执行完毕后调用 回调函数</returns>
        public bool Execute(Instruct ist,Action<Instruct> callback=null);
    }
}
