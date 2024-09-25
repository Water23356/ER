using ER;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ER.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        private TMP_Text text;

        public TMP_Text Text
        {
            get
            {
                if (text == null)
                    text = GetComponent<TMP_Text>();
                return text;
            }
        }

        public string Content
        {
            get => Text.text;
        }

        public string key;

        private void Start()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            Text.text = GLL.GetText(key, "__LOSE_TEXT__");
        }

        public void UpdateText(Dictionary<string, object> replaceDic)
        {
            string text = GLL.GetText(key, "__LOSE_TEXT__");
            Text.text = Utils.ReplacePlaceholders(text, replaceDic);
        }

        public void UpdateText(string key, Dictionary<string, object> replaceDic)
        {
            this.key = key;
            string text = GLL.GetText(key, "__LOSE_TEXT__");
            Text.text = Utils.ReplacePlaceholders(text, replaceDic);
        }
    }
}