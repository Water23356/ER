
using ER.Entity2D.Enum;
using System.Collections.Generic;

namespace ER.Entity2D
{
    /// <summary>
    /// 状态表
    /// </summary>
    public class ActionStateTable
    {
        private Dictionary<ActionName,bool> states = new Dictionary<ActionName, bool>();
        
        public bool this[ActionName key]
        {
            get
            {
                if(states.TryGetValue(key, out bool result))return result;
                return false;
            }
            set
            {
                states[key] = value;
            }
        }
    }
}