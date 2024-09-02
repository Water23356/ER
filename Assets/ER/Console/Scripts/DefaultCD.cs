using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    public class DefaultCD : ICommandDictionaryModfier
    {

        public void Modify(CommandDictionary dic)
        {
            dic.AddCommand("print", (d) =>
            {
                int i = 1;
                while (!d.isEnd)
                {
                    ConsolePanel.Print(d.NextText());
                    i++;
                }
                return Data.Empty;
            });
            dic.AddCommand("exit", (d) =>
            {
                Application.Quit();
                return Data.Empty;
            });
            dic.AddCommand("clear", (d) =>
            {
                ConsolePanel.Instance?.Clear();
                return Data.Empty;
            });
            dic.AddCommand("command all", (d) =>
            {
                var commands = ConsolePanel.dictionary.GetAllCommandFormat();

                for(int k=0;k<commands.Length;k++)
                {
                    ConsolePanel.Print($" [{k}] {commands[k]}");

                }
                ConsolePanel.Print($">总计 {commands.Length} 条指令");
                return Data.Empty;
            });
            dic.AddCommand("command test", (d) =>
            {
                int i = 1;
                ConsolePanel.Print(">");
                while (!d.isEnd)
                {
                    var data = d.NextData();
                    ConsolePanel.Print($"参数 {i} : [{data.Type}] : {data.ToString()}");
                    i++;
                }
                ConsolePanel.Print(">");
                return Data.Empty;
            });
        }
    }
}