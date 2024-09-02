using ER.StateMachine;
using System.Collections.Generic;
using System.Text;
using System;

namespace ER
{
    public class CommandBuilder
    {
        private enum ParseMode
        {
            /// <summary>
            /// 无
            /// </summary>
            None,

            /// <summary>
            /// 一般
            /// </summary>
            Normal,

            /// <summary>
            /// 解析为文本
            /// </summary>
            Text,

            /// <summary>
            /// 解析为指令
            /// </summary>
            Command,

            /// <summary>
            /// 动态参数
            /// </summary>
            Prop,

            /// <summary>
            /// 转义
            /// </summary>
            Escape,
        }

        private CommandBuilder m_subBuilder;

        private CommandBuilder subBuilder
        {
            get
            {
                if (m_subBuilder == null)
                    m_subBuilder = new CommandBuilder();
                return m_subBuilder;
            }
        }

        private CommandRunes parsed = null;

        private List<Command> commands = new List<Command>();
        private List<Data> datas = new List<Data>();
        private StringBuilder colt = new StringBuilder();

        private bool onlyParseOne = false;
        private string originText = string.Empty;
        private int index = 0;
        private bool isOver = false;

        private StateCellMachine scm = new StateCellMachine();
        private ParseMode lastMode = ParseMode.None;
        private int deepCount;

        public char NextChar()
        {
            if (index < 0 || index >= originText.Length)
            {
                isOver = true;
                return '\0';
            }
            char c = originText[index++];
            return c;
        }

        public CommandBuilder()
        {
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            scm.CreateStates(ParseMode.None);

            var state = scm.GetState(ParseMode.Normal);
            state.OnUpdate = () =>
            {
                char c = NextChar();
                if (isOver)
                {
                    parsed.AddCommand(PushOver());
                    scm.ChangeState(ParseMode.None);
                    return;
                }
                if (c == ' ')
                {
                    BuildField();
                }
                else if (c == ';')
                {
                    BuildCommand();
                    if (onlyParseOne && commands.Count > 1)
                    {
                        isOver = true;
                    }
                }
                else if (c == '\\')
                {
                    scm.ChangeState(ParseMode.Escape);
                }
                else if (c == '<')
                {
                    scm.ChangeState(ParseMode.Text);
                }
                else if (c == '[')
                {
                    scm.ChangeState(ParseMode.Command);
                }
                else if (c == '{')
                {
                    scm.ChangeState(ParseMode.Prop);
                }
                else
                {
                    colt.Append(c);
                }
            };

            state = scm.GetState(ParseMode.Escape);
            state.OnEnter = RecardParseMode;
            state.OnUpdate = () =>
            {
                char c = NextChar();
                if (!isOver)
                {
                    if (c == '\\')
                    {
                        colt.Append('\\');
                    }
                    else if (c == 'n')
                    {
                        colt.Append('\n');
                    }
                    else if (c == 't')
                    {
                        colt.Append('\t');
                    }
                    else if (c == '<' || c == '>' || c == '[' || c == ']' || c == '{' || c == '｝')
                    {
                        colt.Append(c);
                    }
                }
                scm.ChangeState(lastMode);
            };

            state = scm.GetState(ParseMode.Text);
            state.OnEnter = s =>
            {
                deepCount = 0;
            };
            state.OnUpdate = () =>
            {
                char c = NextChar();
                if (!isOver)
                {
                    if (c == '\\')
                    {
                        scm.ChangeState(ParseMode.Escape);
                    }
                    else if (c == '<')
                    {
                        colt.Append(c);
                        deepCount++;
                    }
                    else if (c == '>')
                    {
                        if (deepCount == 0)
                        {
                            BuildField(DataType.String);
                            scm.ChangeState(ParseMode.Normal);
                        }
                        else
                        {
                            colt.Append(c);
                            deepCount--;
                        }
                    }
                    else
                    {
                        colt.Append(c);
                    }
                    return;
                }
                scm.ChangeState(ParseMode.Normal);
            };

            state = scm.GetState(ParseMode.Command);
            state.OnEnter = s =>
            {
                deepCount = 0;
            };
            state.OnUpdate = () =>
            {
                char c = NextChar();
                if (!isOver)
                {
                    if (c == '\\')
                    {
                        scm.ChangeState(ParseMode.Escape);
                    }
                    else if (c == '[')
                    {
                        colt.Append(c);
                        deepCount++;
                    }
                    else if (c == ']')
                    {
                        if (deepCount == 0)
                        {
                            BuildCommandField();
                            scm.ChangeState(ParseMode.Normal);
                        }
                        else
                        {
                            colt.Append(c);
                            deepCount--;
                        }
                    }
                    else
                    {
                        colt.Append(c);
                    }
                    return;
                }
                scm.ChangeState(ParseMode.Normal);
            };

            state = scm.GetState(ParseMode.Prop);
            state.OnEnter = s =>
            {
                Console.WriteLine("变量解析");
            };
            state.OnUpdate = () =>
            {
                char c = NextChar();
                if (!isOver)
                {
                    if (c == '\\')
                    {
                        scm.ChangeState(ParseMode.Escape);
                    }
                    else if (c == '}')
                    {
                        BuildField(DataType.Prop);
                        scm.ChangeState(ParseMode.Normal);
                    }
                    else
                    {
                        colt.Append(c);
                    }
                    return;
                }
                scm.ChangeState(ParseMode.Normal);
            };
        }

        private void BuildField()
        {
            string field = colt.ToString();
            colt.Clear();
            if (!string.IsNullOrWhiteSpace(field))
            {
                Console.WriteLine($"构建字段: {field}");

                datas.Add(Data.ParseTo(field));
            }
        }

        private void BuildField(DataType type)
        {
            string field = colt.ToString();
            colt.Clear();
            if (!string.IsNullOrWhiteSpace(field))
            {
                Console.WriteLine($"构建字段: {field}");
                datas.Add(Data.ParseTo(field, type));
            }
        }

        private void BuildCommandField()
        {
            string field = colt.ToString();
            colt.Clear();
            if (!string.IsNullOrWhiteSpace(field))
            {
                Command newCmd = subBuilder.ParseOne(field);
                datas.Add(new Data(newCmd, DataType.Command));
            }
        }

        private void BuildPropField()
        {
            string field = colt.ToString();
            colt.Clear();
            if (!string.IsNullOrWhiteSpace(field))
            {
                datas.Add(new Data(field.Trim(), DataType.Prop));
            }
        }

        private void BuildCommand()
        {
            BuildField();
            if (datas.Count == 0)
            {
                return;
            }
            Command cmd = new Command(datas.ToArray());
            if (!cmd.isEmpty)
                commands.Add(cmd);
            Console.WriteLine($"构建指令: {cmd.ToString()}");
            datas.Clear();
        }

        private Command[] PushOver()
        {
            BuildCommand();
            var commandList = commands.ToArray();
            Console.WriteLine($"缓存指令个数: {commandList.Length}");
            commands.Clear();
            return commandList;
        }

        private void RecardParseMode(StateCell s)
        {
            lastMode = (ParseMode)s.StateIndex;
        }

        /// <summary>
        /// 解析为指令集
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public CommandRunes Parse(string text)
        {
            if(string.IsNullOrEmpty(text))
                return null;
            isOver = false;
            onlyParseOne = false;
            index = 0;
            originText = text;
            parsed = new CommandRunes();
            scm.ChangeState(ParseMode.Normal);
            while (!isOver)
            {
                scm.Update();
            }
            return parsed;
        }

        public Command ParseOne(string text)
        {
            isOver = false;
            onlyParseOne = true;
            index = 0;
            originText = text;
            parsed = new CommandRunes();
            scm.ChangeState(ParseMode.Normal);
            while (!isOver)
            {
                scm.Update();
            }
            return parsed.First();
        }

        public void ClearToken()
        {
            colt.Clear();
        }

        public void ClearData()
        {
            datas.Clear();
        }

        public void ClearCommand()
        {
            commands.Clear();
        }
    }
}