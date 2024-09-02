namespace ER
{
    public interface ICommandDictionaryModfier
    {
        public void Modify(CommandDictionary dic);
    }

    public abstract class CommandDictionaryModfier
    {
        public abstract void Modify(CommandDictionary dic);
        public void Print(string text)
        {
            ConsolePanel.Print(text);
        }
        public void PrintError(string text)
        {
            ConsolePanel.PrintError(text);
        }
    }
}