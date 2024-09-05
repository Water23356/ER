using System;

namespace ER.RPG
{
    [GraphNodeInfo("对话演出信息")]
    [Serializable]
    public class DialogueInfo
    {
        public string originText;
        public string key_title;
        public string key_content;
    }
}