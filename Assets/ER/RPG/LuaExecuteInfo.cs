using System;

namespace ER.RPG
{
    [GraphNodeInfo("执行信息")]
    [Serializable]
    public class LuaExecuteInfo
    {
        public string onLoading;
        public string onLocked;
        public string onEnable;
    }
}