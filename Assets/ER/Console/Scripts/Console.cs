namespace ER
{
    public partial class ConsolePanel
    {
        private static CommandDictionary m_dictionary;

        public static CommandDictionary dictionary
        {
            get
            {
                if (m_dictionary == null)
                {
                    m_dictionary = new CommandDictionary();
                }
                return m_dictionary;
            }
            set
            {
                m_dictionary = value;
            }
        }

        public ConsolePanel()
        {
            registryName = "gui:origin:console";
        }

        #region 静态方法

        /// <summary>
        /// 向控制台输出消息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="txt"></param>
        public static void Print(string txt, bool newline = true)
        {
            ConsolePanel.Instance?._Print(txt, newline);
        }

        /// <summary>
        /// 向控制台输出消息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="txt"></param>
        public static void PrintError(string txt, bool newline = true)
        {
            ConsolePanel.Instance?._PrintError(txt, newline);
        }

        #endregion 静态方法

        /// <summary>
        /// 解释指定指令(不会自动清除变量缓存)
        /// </summary>
        /// <param name="commandText">指令内容</param>
        /// <returns>是否是一个有效指令</returns>
        private void ExecuteCommand(string commandText)
        {
            var runes = CommandHandler.ParseToRunes(commandText);
            CommandHandler.Execute(runes, dictionary,false);
            /*
            if (CommandHandler.Parse(commandText, dictionary).isError())
            {
                return false;
            }
            return true;*/
        }
        /// <summary>
        /// 解释指定指令集
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteCommandRunes(string commandText,bool autoClear = true)
        {
            var runes = CommandHandler.ParseToRunes(commandText);
            CommandHandler.Execute(runes, dictionary,autoClear);
        }
    }
}