using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 指令字典
    /// </summary>
    public class CommandDictionary
    {
        private enum PartType
        {
            CommandHead,
            NormalParam,
            Enum,
        }

        private Dictionary<string, CommandArea> commands = new Dictionary<string, CommandArea>();

        /// <summary>
        /// 用于标记是否已经被指令解释器检查过, 如果没有检查过则会注入检查器的一些内置指令
        /// </summary>
        public bool isChecked = false;

        public void AddCommand(string commandFormat, Func<Parameters, Data> hanlde)
        {
#if false
battle player health {int,0}
{string,-}
{bool,false}
一般参数: #参数类型:默认值
    - #int: 0
    - #float: 0
    - #bool: false
    - #string: string.Empty
默认值可不填, 在必要时会填入系统默认值
    "#"用于标记参数, #之后的所有部分都会被视为参数, 如果后续参数以#开头, 那么#会被忽略
#endif
            //以空格分割, 且自动去无效元素
            AddCommand(null, commandFormat, hanlde);
        }

        public void AddCommand(CommandArea parent, string commandFormat, Func<Parameters, Data> hanlde)
        {
            string[] parts = commandFormat.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            bool isParam = false;
            int pindex = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                var str = parts[i];
                if (isParam)
                {
                    SetParam(str, parent, pindex++);
                    continue;
                }
                if (str.StartsWith('#'))//表明是参数
                {
                    isParam = true;
                    SetParam(str, parent, pindex++);
                }
                else
                {
                    parent = GetAreaOrCreateNew(str, parent);
                }
            }
            if (parent != null)
            {
                parent.isFinal = true;
                parent.commandHandle = hanlde;
                Debug.Log($"注册指令: {parent.GetFullName()}");
            }
        }

        /// <summary>
        /// 移除指定指令
        /// </summary>
        /// <param name="commandFormat"></param>
        public void RemoveCommand(string commandFormat)
        {
            //Debug.Log($"尝试移除指令: {commandFormat}");
            string[] parts = commandFormat.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            CommandArea node = GetArea(parts[0]);
            int i = 1;
            while (i < parts.Length)
            {
                if (node == null)
                    return;
                node = GetArea(parts[i++], node);
            }
            if (node != null)
            {
                Debug.Log($"移除指令: {node.GetFullName()} 父域指令: {node.parent != null}");
                if (node.parent == null)
                {
                    commands.Remove(node.areaName);
                }
                else
                {
                    node.parent.RemoveArea(node);
                }
            }
        }

        private void SetParam(string paramStr, CommandArea area, int index = 0)
        {
            paramStr = StartIgnore(paramStr, "#");
            string[] parseStr = paramStr.Split(':', 2);

            //取得参数限定类型
            DataType type = DataType.Unknown;
            switch (parseStr[0])
            {
                case "int":
                    type = DataType.Int;
                    break;

                case "float":
                    type = DataType.Float;
                    break;

                case "string":
                    type = DataType.String;
                    break;

                case "bool":
                    type = DataType.Bool;
                    break;
            }

            if (type == DataType.Unknown)
            {
                Debug.LogError($"解析出错, 未知参数类型: {parseStr[0]}");
                return;
            }

            CommandParam commandParam = new CommandParam();
            commandParam.type = type;
            string valueText = string.Empty;
            if (parseStr.Length > 1)
            {
                //Debug.Log($"参数默认值: {parseStr[1]}");
                valueText = parseStr[1];
            }
            switch (type)
            {
                case DataType.Int:
                    if (int.TryParse(valueText, out var dv))
                    {
                        commandParam.defaultValue = new Data(dv, type);
                        break;
                    }
                    commandParam.defaultValue = new Data(0, type);
                    break;

                case DataType.Float:
                    if (float.TryParse(valueText, out var dv2))
                    {
                        commandParam.defaultValue = new Data(dv2, type);
                        break;
                    }
                    commandParam.defaultValue = new Data(0f, type);
                    break;

                case DataType.Bool:
                    if (bool.TryParse(valueText, out var dv3))
                    {
                        commandParam.defaultValue = new Data(dv3, type);
                        break;
                    }
                    commandParam.defaultValue = new Data(false, type);
                    break;

                case DataType.String:
                    commandParam.defaultValue = new Data(valueText, type);
                    break;
            }
            area.SetParam(commandParam, index);
        }

        private string StartIgnore(string origin, string ignoreStr)
        {
            if (origin.StartsWith(ignoreStr))
            {
                return origin.Substring(ignoreStr.Length);
            }
            return origin;
        }

        public Data Execute(Data[] commandParts)
        {
            int index = 0;
            CommandArea area = null;
            while (index < commandParts.Length)
            {
                area = GetArea(commandParts[index].ToString(), area);//获取指令域
                if (area == null)
                {
                    Debug.LogWarning("未知指令");
                    return Data.ErrorLog("未知指令");
                }
                index++;
                if (area.isFinal)//如果匹配到执行指令
                {
                    Data[] cparams = new Data[commandParts.Length - index];//剩余参数被打包
                    Array.Copy(commandParts, index, cparams, 0, cparams.Length);
                    return area.Invoke(new Parameters(area, cparams));
                }
            }
            Debug.LogWarning("未知指令");
            return Data.ErrorLog("未知指令");
        }

        public CommandArea GetArea(string areaName, CommandArea parent = null)
        {
            if (parent != null)
            {
                if (!parent.Contains(areaName))
                {
                    return null;
                }
                return parent[areaName];
            }
            if (!commands.ContainsKey(areaName))
            {
                return null;
            }
            return commands[areaName];
        }

        public CommandArea GetAreaOrCreateNew(string areaName, CommandArea parent = null)
        {
            if (parent != null)
            {
                if (!parent.Contains(areaName))
                {
                    var newArea = new CommandArea(areaName);
                    parent[areaName] = newArea;
                    //Debug.Log($"创建指令域: {newArea.GetFullName()} 父指令域: {newArea.parent != null}");
                    return newArea;
                }
                return parent[areaName];
            }

            if (!commands.ContainsKey(areaName))
            {
                var newArea = new CommandArea(areaName);
                commands[areaName] = newArea;
                //Debug.Log($"创建指令域: {newArea.GetFullName()}");
                return newArea;
            }
            return commands[areaName];
        }

        public string[] GetAllCommandFormat()
        {
            LinkedList<CommandArea> areas = new LinkedList<CommandArea>();
            GetAllCommand(areas);
            string[] text = new string[areas.Count];
            int i = 0;
            foreach (var area in areas)
            {
                text[i] = area.GetFullName();
                i++;
            }
            return text;

            //LinkedList<string> list = new LinkedList<string>();
            //GetAllCommandFormat(list);
            //return list.ToArray();
        }

        /// <summary>
        /// 获取所有指令的格式串
        /// </summary>
        /// <param name="list"></param>
        public void GetAllCommandFormat(LinkedList<string> list)
        {
            foreach (var sub in commands.Values)
            {
                sub.GetAllCommandFormat(list);
            }
        }

        /// <summary>
        /// 获取所有指令对象
        /// </summary>
        /// <param name="list"></param>
        public void GetAllCommand(LinkedList<CommandArea> list)
        {
            foreach (var sub in commands.Values)
            {
                sub.GetAllCommand(list);
            }
        }

        /// <summary>
        /// 将制定字典的指令加入本字典
        /// </summary>
        /// <param name="newDictionary"></param>
        /// <param name="cover">新字典指令是否覆盖旧字典指令</param>
        public void Combine(CommandDictionary newDictionary, bool cover = true)
        {
            foreach (var sub in newDictionary.commands)
            {
                if (!cover && commands.ContainsKey(sub.Key)) continue;
                commands[sub.Key] = sub.Value;
            }
        }

        public CommandDictionary Copy()
        {
            CommandDictionary ncs = new CommandDictionary();
            LinkedList<CommandArea> cmds = new();
            GetAllCommand(cmds);
            foreach (var sub in cmds)
            {
                ncs.AddCommand(sub.GetFullName(), sub.commandHandle);
            }
            return ncs;
        }

        public void Modify<T>() where T : ICommandDictionaryModfier, new()
        {
            new T().Modify(this);
        }
    }

    public class Parameters
    {
        public CommandArea area;
        public Data[] parameters;
        public int index;//当前解析位置

        public bool isEnd
        {
            get => index >= parameters.Length;
        }

        public void MoveTo(int index)
        {
            this.index = index;
        }

        public void Move(int move)
        {
            index += move;
            index = Math.Max(index, 0);
        }

        public int NextInt(int defaultValue = 0)
        {
            int value = Data.GetIntData(parameters, index,
                area.GetCommandParam(index)?.defaultValue.Int ?? defaultValue);
            index++;
            return value;
        }

        public string NextText(string defaultValue = "")
        {
            string value = Data.GetTextData(parameters, index,
                area.GetCommandParam(index)?.defaultValue.String ?? defaultValue);
            index++;
            return value;
        }

        public bool NextBool(bool defaultValue = false)
        {
            //Debug.Log("类型: " + area.GetCommandParam(index).defaultValue.Type + "封装类型: " + area.GetCommandParam(index).type);
            //Debug.Log("默认值: " + area.GetCommandParam(index).defaultValue);
            bool value = Data.GetBoolData(parameters, index,
                area.GetCommandParam(index)?.defaultValue.Boolean ?? defaultValue);
            index++;
            return value;
        }

        public float NextFloat(float defaultValue = 0f)
        {
            float value = Data.GetFloatData(parameters, index,
                area.GetCommandParam(index)?.defaultValue.Float ?? defaultValue);
            index++;
            return value;
        }

        public Data NextData()
        {
            if (index < 0 || index >= parameters.Length)
                return Data.Error;
            return parameters[index++];
        }

        public Parameters(CommandArea area, Data[] parameters)
        {
            this.area = area;
            this.parameters = parameters;
            index = 0;
        }
    }

    public class CommandArea
    {
        /// <summary>
        /// 指令域名
        /// </summary>
        private string m_head = string.Empty;

        private bool m_isFinal = false;

        private CommandArea m_parent = null;

        /// <summary>
        /// 子指令域
        /// </summary>
        private Dictionary<string, CommandArea> m_subArea = null;

        /// <summary>
        /// 默认参数
        /// </summary>
        private List<CommandParam> m_params = null;

        private Func<Parameters, Data> m_commandHandle = null;

        /// <summary>
        /// 是否是执行指令(没有子指令域)
        /// </summary>
        public bool isFinal { get => m_isFinal; set => m_isFinal = value; }

        public CommandArea this[string subAreaName]
        {
            get
            {
                return GetSubArea(subAreaName);
            }
            set
            {
                AddSubArea(value);
            }
        }

        public CommandArea parent { get => m_parent; private set => m_parent = value; }
        public string areaName { get => m_head; set => m_head = value; }
        public Func<Parameters, Data> commandHandle { get => m_commandHandle; set => m_commandHandle = value; }

        public Dictionary<string, CommandArea> subArea
        {
            get
            {
                if (m_subArea == null)
                    m_subArea = new Dictionary<string, CommandArea>();
                return m_subArea;
            }
        }

        public List<CommandParam> @params
        {
            get
            {
                if (m_params == null)
                    m_params = new List<CommandParam>();
                return m_params;
            }
        }

        public CommandArea(string areaName)
        {
            this.areaName = areaName;
        }

        public CommandArea[] GetSubArea()
        {
            return subArea.Values.ToArray();
        }

        public CommandArea GetSubArea(string subName)
        {
            if (subArea.TryGetValue(subName, out var r)) return r;
            return null;
        }

        public void AddSubArea(CommandArea sub)
        {
            subArea[sub.areaName] = sub;
            sub.parent = this;
        }

        public void RemoveArea(CommandArea sub)
        {
            subArea.Remove(sub.areaName);
        }

        public bool Contains(string subAreaName)
        {
            return subArea.ContainsKey(subAreaName);
        }

        public void SetParam(CommandParam param, int index)
        {
            if (index < 0 || index >= @params.Count)
                @params.Add(param);
            @params[index] = param;
        }

        public Data Invoke(Parameters parameters)
        {
            if (commandHandle == null)
                return Data.Error;
            return commandHandle.Invoke(parameters);
        }

        public string[] GetAllCommandFormat()
        {
            LinkedList<string> list = new LinkedList<string>();
            GetAllCommandFormat(list);
            return list.ToArray();
        }

        public void GetAllCommandFormat(LinkedList<string> list)
        {
            if (isFinal)
            {
                list.AddLast(string.Join(' ', areaName, GetParamName()));
            }
            else
            {
                LinkedListNode<string> lastNode = list.Last;
                foreach (var sub in subArea.Values)
                {
                    sub.GetAllCommandFormat(list);
                }
                if (lastNode == null) return;
                //加上自己的指令域作为前缀
                var node = lastNode.Next;
                while (node != null)
                {
                    node.Value = string.Join(' ', areaName, node.Value);
                    node = node.Next;
                }
            }
        }

        public void GetAllCommand(LinkedList<CommandArea> list)
        {
            if (isFinal)
            {
                list.AddLast(this);
            }
            else
            {
                foreach (var sub in subArea.Values)
                {
                    sub.GetAllCommand(list);
                }
            }
        }

        public string GetParamName()
        {
            StringBuilder sb = new();
            sb.Append("#");
            foreach (var p in @params)
            {
                sb.Append(p.ToString());
                sb.Append(' ');
            }
            return sb.ToString();
        }

        public string GetFullName()
        {
            string name = string.Empty;
            if (parent == null)
            {
                name = areaName;
            }
            else
            {
                name = string.Join(' ', parent.GetFullName(), areaName);
            }
            if (isFinal)
            {
                return string.Join(' ', name, GetParamName());
            }
            return name;
        }

        public CommandParam GetCommandParam(int index)
        {
            if (index < 0 || index >= @params.Count)
                return null;
            return @params[index];
        }
    }

    public class CommandParam
    {
        public DataType type;//既定参数类型
        public Data defaultValue;//参数默认值

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            switch (type)
            {
                case DataType.Int:
                    sb.Append("int");
                    break;

                case DataType.Float:
                    sb.Append("float");
                    break;

                case DataType.Bool:
                    sb.Append("bool");
                    break;

                case DataType.String:
                    sb.Append("string");
                    break;

                default:
                    sb.Append("unknwon");
                    break;
            }
            sb.Append(':');
            sb.Append(defaultValue.ToString());
            return sb.ToString();
        }
    }
}