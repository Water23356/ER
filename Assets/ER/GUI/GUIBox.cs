using System.Collections.Generic;
using ER.ForEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ER.GUI
{
    public class GUIBox : EGUIPanel
    {

        [DisplayLabel("选项卡")]
        public List<Button> buttons = new List<Button>();

        [DisplayLabel("子面板")]
        public List<SubGUIPanel> subPanels = new List<SubGUIPanel>();

        public override Dictionary<string, object> GetPanelInfo()
        {
            return null;
        }

        protected virtual void Awake()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                int k=i;
                buttons[i]
                    .onClick.AddListener(() =>
                    {
                        if(InputActive)
                            SelectSubPanel(k);
                    });
            }
            for (int i = 0; i < subPanels.Count; i++)
            {
                subPanels[i].Parent = this;
            }
        }

        public void SelectSubPanel(int index)
        {
            // Debug.Log($"选择面板{index}");
            for (int i = 0; i < subPanels.Count; i++)
            {
                subPanels[i].IsVisible = (i == index);
            }
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            SelectSubPanel(0);
        }
    }
}
