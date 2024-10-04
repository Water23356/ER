using MoonSharp.Interpreter;
using UnityEngine;

namespace STG
{
    public class EntityStateMachine : MonoBehaviour, IEntityCompenont
    {

        private void FixedUpdate()
        {
            
        }
    }

    public struct StateCell
    {
        private DynValue onEnter;
        private DynValue onUpdate;
        private DynValue onExit;

        public StateCell(DynValue originTable)
        {
            if (originTable.Type == DataType.Table)
            {
                Table table = originTable.Table;
                onEnter = table.Get("enter");
                onUpdate = table.Get("update");
                onExit = table.Get("exit");

                if (onEnter.Type != DataType.Function)
                    onEnter = null;
                if (onUpdate.Type != DataType.Function)
                    onUpdate = null;
                if (onExit.Type != DataType.Function)
                    onExit = null;
            }
            else
            {
                onEnter = null; onUpdate = null; onExit = null;
            }
        }
        public void CallEnter()
        {
            onEnter?.Function.Call();
        }

        public void CallUpdate()
        {
            onUpdate?.Function.Call();
        }
        public void CallExit()
        {
            onExit?.Function.Call();
        }
    }

    public struct EntityContext
    {
    }
}