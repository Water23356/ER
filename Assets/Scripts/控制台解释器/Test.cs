using ER;
using ER.Control;
using ER.Shikigami.Message;
using ER.UI;
using System;
using UnityEngine;

namespace 控制台解释器
{
    public class Test:MonoBehaviour
    {
        public DialogBubble DialogBubble;
        public TxtBubble TxtBubble;
        private void Awake()
        {
            AInterpreter interpreter = new AInterpreter();
            AInterpreter.dialogBubble = DialogBubble;
            AInterpreter.TxtBubble = TxtBubble;
            ConsolePanel.interpreter = interpreter;
        }
    }
}