using ER.Shikigami;
using ER.Shikigami.Message;
using System.IO;
using UnityEngine;

namespace P1
{
    public class Test : MonoBehaviour
    {
        private Allocator allocator = new Allocator();

        private Spell[] spells;
        public BasicDialogPanel panel;
        public BasicMessagePanel messagePanel;
        public ConfirmMessagePanel confirmMessagePanel;

        private void Awake()
        {
            spells = SpellBook.ParseSpellFromFile(Path.Combine(Application.streamingAssetsPath, "spell/spell_1.ykr"));
            allocator.panel = panel;
        }

        [ContextMenu("开始符卡")]
        public void OpenSpell()
        {
            allocator.SpellCard = spells[0];
            allocator.Execute();
        }

        [ContextMenu("打开消息面板")]
        public void OpenMessage()
        {
            messagePanel.OpenPanel("警告", "一旦进入地牢,就无法随时返回, 仅有在地牢出口才能返回主城, 在地牢中死亡将会遗失所有物品和经验");
        }
        [ContextMenu("关闭消息面板")]
        public void CloseMessage()
        {
            messagePanel.ClosePanel();
        }
        [ContextMenu("打开确认面板")]
        public void Test1()
        {
            confirmMessagePanel.Title = "警告";
            confirmMessagePanel.Text = "存档一旦销毁就无法恢复, 确认删除存档?";
            confirmMessagePanel.OpenPanel(() => Debug.Log("确认"), () => Debug.Log("取消"));
        }
        [ContextMenu("关闭确认面板")]
        public void Test2()
        {
            confirmMessagePanel.ClosePanel();
        }
    }
}