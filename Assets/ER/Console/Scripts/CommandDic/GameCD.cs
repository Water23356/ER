using ER;

namespace ER
{


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