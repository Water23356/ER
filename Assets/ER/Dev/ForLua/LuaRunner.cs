
using Neo.IronLua;
using System;
using UnityEngine;

namespace ER.Dev
{
    public class LuaRunner
    {
        private Lua lua;
        private LuaGlobal envm;

        void Test()
        {
            lua = new Lua();
            envm = lua.CreateEnvironment();
            Action action = () =>
            {
                Debug.Log("114514");
            };
            envm["test"] = action;
        }
    }
}