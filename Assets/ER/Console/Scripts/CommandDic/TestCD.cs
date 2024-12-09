using ER.GUI;
using ER.Localization;
using ER.ResourceManager;

namespace ER
{
    public class TestCD : ICommandDictionaryModfier
    {
        public void Modify(CommandDictionary dic)
        {
            dic.AddCommand("test gui #string bool:true", d =>
            {
                string guiName = d.NextText();
                GUIManager.Instance.ELoad(guiName);
                var panel = GUIManager.Instance.GetPanel(guiName);
                if (panel != null)
                {
                    panel.IsVisible = d.NextBool();
                }
                return Data.Empty;
            });

            //打印指定所有资源头的注册名(已加载)
            dic.AddCommand("test res print all #string", d =>
            {
                string head = d.NextText();
                var res = GR.GetAll(head);
                int i = 0;
                foreach (var r in res)
                {
                    i++;
                    ConsolePanel.Print($"[{i}]: {r?.registryName} [notNull: {r != null}]");
                }
                return Data.Empty;
            });
            //获取指定资源
            dic.AddCommand("test res get #string", d =>
            {
                string path = d.NextText();
                var res = GR.Get(path);
                ConsolePanel.Print($"获取: {path} -> rname: {res?.registryName} notnull: {res != null}");
                return Data.Empty;
            });

            dic.AddCommand("test add command", d =>
            {
                dic.AddCommand("erinbone print", d =>
                {
                    ConsolePanel.Print("ERinbone");
                    return Data.Empty;
                });
                return Data.Empty;
            });

            dic.AddCommand("test remove command", d =>
            {
                dic.RemoveCommand("erinbone print");
                return Data.Empty;
            });

            dic.AddCommand("test lang #string", d =>
            {
                var path = d.NextText();
                var lang = GLL.GetText(path, "__ERRO__");
                ConsolePanel.Print(lang);
                return Data.Empty;
            });
        }
    }
}