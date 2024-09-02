using ER.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 指令解释器
    /// </summary>
    public class CommandHandler
    {
        private static CommandBuilder builder;
        private static CommandExecuter executer;

        public static CommandBuilder Builder
        {
            get
            {
                if (builder == null)
                    builder = new CommandBuilder();
                return builder;
            }
        }

        public static CommandExecuter Executer
        {
            get
            {
                if (executer == null)
                    executer = new CommandExecuter();
                return executer;
            }
        }

        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Console.WriteLine(message);

#if false
        public static Data Parse(string txt, CommandDictionary commandDictionary)
        {
            if (commandDictionary == null)
            {
                return Data.Empty;
            }
            var parameters = Split(txt);
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Type == DataType.Command)//如果参数是一条指令，则运行该指令取其返回值作为参数
                {
                    parameters[i] = Parse(parameters[i].Value.ToString() + string.Empty, commandDictionary);
                }
            }
            return commandDictionary.Execute(parameters);
        }

        /// <summary>
        /// 将字符串切割为若干数据元(以空格切割单元)
        /// </summary>
        /// <param name="txt">原始字符串</param>
        /// <returns></returns>
        public static Data[] Split(string txt)
        {
            List<Data> list = new List<Data>();
            StringBuilder temp = new StringBuilder();
            int i = 0;

            bool trans = false;//转义状态
            int instruct = 0;//指令状态
            int quote = 0;//引用状态

            int mode = 0;//0正常封装 1指令封装 2字符串封装
#if Test

            Console.WriteLine($"正在解析:{txt}");
#endif
            while (i < txt.Length)
            {
                char c = txt[i++];
                if (trans)
                {
                    switch (c)
                    {
                        case '\\':
                            temp.Append(c);
                            break;

                        case 'r':
                            temp.Append('\r');
                            break;

                        case 'n':
                            temp.Append('\n');
                            break;

                        case 't':
                            temp.Append('\r');
                            break;

                        case '<':
                            temp.Append('<');
                            break;

                        case '>':
                            temp.Append('>');
                            break;
                    }
                    trans = false;
                }
                else
                {
                    switch (c)
                    {
                        case '\t'://忽略
                        case '\r'://忽略
                        case '\n'://忽略
                            break;

                        case ' '://分割符号

                            #region 分割

                            if (instruct > 0 || quote > 0)
                            {
                                temp.Append(c);
                            }
                            else if (temp.Length > 0)//缓存不为空
                            {
                                string s = temp.ToString();
                                switch (mode)
                                {
                                    case 2:
                                        list.Add(Data.ParseTo(s, DataType.String));
                                        break;

                                    case 1:
                                        list.Add(Data.ParseTo(s, DataType.Command));
                                        break;

                                    case 0:
                                        list.Add(Data.ParseTo(s));
                                        break;
                                }
                                mode = 0;
                                temp.Clear();
                            }

                            #endregion 分割

                            break;

                        case '\\'://开启转义
                            if (instruct > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                trans = true;
                            }
                            break;

                        case '['://指令头
                            if (quote > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                if (instruct > 0)
                                {
                                    temp.Append(c);
                                }
                                else
                                {
                                    mode = 1;
                                }
                                instruct++;
                            }
                            break;

                        case ']'://指令尾
                            if (quote > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                instruct--;
                                if (instruct > 0)
                                {
                                    temp.Append(c);
                                }
                            }
                            break;

                        case '<'://引用头
                            if (instruct > 0 || quote > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                mode = 2;
                            }
                            quote++;
                            break;

                        case '>'://引用尾
                            quote--;
                            if (instruct > 0 || quote > 0)
                            {
                                temp.Append(c);
                            }
                            break;

                        default:
                            temp.Append(c);
                            break;
                    }
                }
            }

            #region 分割

            if (temp.Length > 0)//缓存不为空
            {
                string s = temp.ToString();
                switch (mode)
                {
                    case 2:
                        list.Add(Data.ParseTo(s, DataType.String));
                        break;

                    case 1:
                        list.Add(Data.ParseTo(s, DataType.Command));
                        break;

                    case 0:
                        list.Add(Data.ParseTo(s));
                        break;
                }
                temp.Clear();
            }

            #endregion 分割

            return list.ToArray();
        }
#endif

        /// <summary>
        /// 解析为指令集
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CommandRunes ParseToRunes(string text)
        {
            return Builder.Parse(text);
        }

        /// <summary>
        /// 执行指令集
        /// </summary>
        /// <param name="runes"></param>
        /// <param name="dic"></param>
        public static void Execute(CommandRunes runes, CommandDictionary dic, bool autoClearProp = false)
        {
            //Debug.Log("执行指令集");
            if (runes == null) return;
            Executer.Execute(runes, dic);
            if (autoClearProp)
            {
                Executer.ClearDynamicProperty();
            }
        }

        public static Data Execute(Command command, CommandDictionary dic)
        {
            if (command == null) return Data.Empty;
            return Executer.Execute(command,dic);
        }
    }

    /// <summary>
    /// 指令执行机
    /// </summary>
    public class CommandExecuter : WithDynamicProperties<Data>, ICommandDictionaryModfier
    {
        public void Modify(CommandDictionary dic)
        {
            dic.AddCommand("var set #string", (d) =>
            {
                string key = d.NextText("__error_key__");
                Data data = d.NextData();
                SetProp(key, data);
                ConsolePanel.Print($">设置变量: {key}={data.ToString()}");
                return data;
            });

            dic.AddCommand("var clear", (d) =>
            {
                ClearDynamicProperty();
                return Data.Empty;
            });

            dic.AddCommand("var all", (d) =>
            {
                int index = 0;
                ConsolePanel.Print(">已申请的变量: ");
                foreach (KeyValuePair<string, Data> kp in dynamicProperties)
                {
                    index++;
                    ConsolePanel.Print($"[{index}] {kp.Key} : {kp.Value.ToString()}");
                }
                ConsolePanel.Print(">End");
                return Data.Empty;
            });

            dic.AddCommand("if #bool string string", d =>
            {
                bool ift = d.NextBool();
                var cmd1 = CommandHandler.ParseToRunes(d.NextText());
                var cmd2 = CommandHandler.ParseToRunes(d.NextText());
                if (ift)
                {
                    CommandHandler.Execute(cmd1, dic, false);
                }
                else
                {
                    CommandHandler.Execute(cmd2, dic, false);
                }
                return Data.Empty;
            });

            
            dic.AddCommand("execute command #string", (d) =>
            {
                var cmd1 = CommandHandler.ParseToRunes(d.NextText());
                CommandHandler.Execute(cmd1, dic, false);
                return Data.Empty;
            });

            dic.AddCommand("load file #string", (d) =>
            {
                FileInfo fileInfo = new FileInfo(d.NextText());
                if(fileInfo.Exists)
                {
                    string text = File.ReadAllText(fileInfo.FullName);
                    Debug.Log(text);
                    return new Data(text,DataType.String);
                }
                ConsolePanel.PrintError($"文件不存在: {fileInfo.FullName}");
                return Data.Empty;
            });

            dic.AddCommand("execute file #string bool", d =>
            {
                FileInfo fileInfo = new FileInfo(d.NextText());
                if (fileInfo.Exists)
                {
                    string text = File.ReadAllText(fileInfo.FullName);
                    bool autoCleaer = d.NextBool(true);
                    var runes = CommandHandler.ParseToRunes(text);
                    CommandHandler.Execute(runes, dic, autoCleaer);
                    return Data.Empty;
                }
                ConsolePanel.PrintError($"文件不存在: {fileInfo.FullName}");
                return Data.Empty;
            });

            dic.AddCommand("var select #bool", d =>
            {
                bool status = d.NextBool();
                Data d1 = d.NextData();
                if (d1.isError()) d1 = Data.Empty;
                Data d2 = d.NextData();
                if (d2.isError()) d2 = Data.Empty;
                if(status)
                {
                    return d1;
                }
                return d2;
            });
        }

        public void Execute(CommandRunes _runes, CommandDictionary dictionary)
        {
            var runes = _runes.Copy();//获取指令集的拷贝, 之后会涉及更改指令的参数内容
            //Debug.Log($"字典是否已检查?: {dictionary.isChecked}");
            if (!dictionary.isChecked)
            {
                Modify(dictionary);//嵌入对局部变量注册的指令
                dictionary.isChecked = true;
            }
            runes.MoveTo(0);
            Command cmd = null;
            while (!runes.isEnd)
            {
                cmd = runes.Next();
                Execute(cmd, dictionary);
            }
        }

        /// <summary>
        /// 创建变量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetProp(string key, Data value)
        {
            SetDynamicProperty(key, value);
        }

        public Data Execute(Command cmd, CommandDictionary dic)
        {
            if (cmd == null) return Data.Error;
            cmd.MoveTo(0);
            Debug.Log($"执行指令: {cmd}");
            CommandArea area = null;
            while (!cmd.isEnd)
            {
                string key = cmd.NextFieldName.Trim();
                area = dic.GetArea(key, area);
                if (area == null)
                {
                    Debug.LogWarning($"未知指令: {cmd} at {key}");
                    return Data.ErrorLog("未知指令");
                }

                //Debug.Log($"检查指令域: {area.areaName}, isFinal: {area.isFinal}");
                if (area.isFinal)
                {
                    Data[] cparams = cmd.SplitParams();
                    ExecuteParamCommand(ref cparams, dic);//在正式执行指令前, 需要对参数部分进行替换处理
                    return area.Invoke(new Parameters(area, cparams));//封装参数包, 执行指令
                }
            }
            Debug.LogWarning($"未知指令: {cmd}");
            return Data.ErrorLog("未知指令");
        }

        /// <summary>
        /// 执行指令参数中的指令
        /// </summary>
        private void ExecuteParamCommand(ref Data[] cparams, CommandDictionary dic)
        {
            for (int i = 0; i < cparams.Length; i++)
            {
                switch (cparams[i].Type)
                {
                    case DataType.Command:
                        cparams[i] = Execute(cparams[i].ToCommand(), dic);
                        break;

                    case DataType.Prop:
                        string key = cparams[i].ToString();
                        if (ContainsDynamicProperty(key))
                        {
                            cparams[i] = GetDynamicProperty(key);
                            ConsolePanel.Print($">取得变量: {key}={cparams[i].ToString()}");
                        }
                        else
                        {
                            cparams[i] = Data.Error;
                            ConsolePanel.PrintError($">无法取得变量: {key}");
                        }

                        break;
                }
            }
        }
    }
}