using ER.Shikigami;
using ER.Shikigami.Message;
using UnityEngine;

namespace P1
{
    public class Allocator: InstructionAllocator
    {
        public BasicDialogPanel panel;

        protected override bool MinionTask(Instruct ist)
        {
            switch (ist.name)
            {
                case "OpenDialogPanel":
                case "CloseDialogPanel":
                case "SetDialogName":
                case "SetDialogText":
                case "AppendDialogText":
                    return panel.Execute(ist, base.AssignTasks);
                default:
                    Debug.LogError("未知指令:" + ist.name);
                    return true;
            }
        }

    }
}
