using System.Collections.Generic;
using UnityEngine;

namespace ER.GUI
{
    public class SubGUIPanel : GUIPanel
    {
        [SerializeField]
        protected string registryName;

        [SerializeField]
        protected bool withStack;

        [SerializeField]
        protected GUILayer guiLayer;

        public override string RegistryName { get => registryName; set => registryName = value; }


        public override bool WithStack => withStack;

        public override GUILayer Layer { get => guiLayer; set => guiLayer = value; }

        public override bool InputActive
        {
            get => Parent?.InputActive ?? false;
            set { }
        }

        //父面板
        public GUIPanel Parent { get; set; }

        public override Dictionary<string, object> GetPanelInfo()
        {
            return null;
        }
    }
}