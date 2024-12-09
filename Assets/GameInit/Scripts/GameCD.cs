using ER;

/// <summary>
/// 游戏需要注册的指令在这里编写
/// </summary>
public class GameCD : ICommandDictionaryModfier
{
    public void Print(string text)
    {
        ConsolePanel.Print(text);
    }

    public void PrintError(string text)
    {
        ConsolePanel.PrintError(text);
    }

    public void Modify(CommandDictionary dic)
    {
    }
}