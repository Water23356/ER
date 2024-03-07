using ER;
using ER.Parser;
using ER.Shikigami.Message;
using ER.UI;
using UnityEngine;

namespace 控制台解释器
{
    public class AInterpreter : DefaultInterpreter
    {
        public static DialogBubble dialogBubble;
        public static TxtBubble TxtBubble;
        public override Data EffectuateSuper(string commandName, Data[] parameters)
        {
            switch (commandName)
            {
                case "open_panel":
                    ConsolePanel.Print("打开对话泡面板");
                    dialogBubble.OpenPanel();
                    break;
                case "close_panel":
                    ConsolePanel.Print("关闭对话泡面板");
                    dialogBubble.ClosePanel();
                    break;
                case "append":
                    ConsolePanel.Print($"添加文本内容:{parameters[0].ToString()}");
                    dialogBubble.Append(parameters[0].ToString());
                    break;
                case "set_position":
                    Debug.Log(parameters[0].Value + " "+ parameters[0].Type);
                    float x = (float)parameters[0].ToDouble();
                    float y = (float)parameters[1].ToDouble();
                    ConsolePanel.Print($"设置位置:({x},{y})");
                    dialogBubble.transform.position = new Vector3(x,y,0);
                    break;
                case "textpanel:open":
                    TxtBubble.OpenPanel();
                    break;
                case "textpanel:close":
                    TxtBubble.ClosePanel();
                    break;
                case "textpanel:add":
                    TxtBubble.AddTxt(parameters[0].ToString());
                    break;
            }
            return Data.Error;
        }
    }
}
