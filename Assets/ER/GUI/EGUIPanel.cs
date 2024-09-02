using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.GUI
{
    public class EGUIPanel : GUIPanel
    {
        [DisplayLabel("保持有效")]
        public bool alwaysActive = false;

        [SerializeField]
        protected string registryName;

        [SerializeField]
        protected bool withStack;

        [SerializeField]
        protected GUILayer guiLayer;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (alwaysActive)
            {
                InputActive = true;
            }
        }

        public override string RegistryName { get => registryName; set => registryName = value; }

        public override bool WithStack => withStack;

        public override GUILayer Layer { get => guiLayer; set => guiLayer = value; }

        public override Dictionary<string, object> GetPanelInfo()
        {
            return null;
        }
    }
}