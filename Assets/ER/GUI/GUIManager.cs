using ER.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.GUI
{
    /// <summary>
    /// GUI管理器
    /// </summary>
    public sealed class GUIManager : MonoSingleton<GUIManager>
    {
        #region 组件

        [SerializeField]
        private Canvas worldCanvas;

        [SerializeField]
        private Canvas normalCanvas;

        private ControlSorter normalSorter;

        [SerializeField]
        private Canvas noCastCanvas;

        private ControlSorter noCastSorter;

        [SerializeField]
        private Canvas topCanvas;


        private ControlSorter topSorter;

        #endregion 组件

        private Dictionary<string, GUIPanel> panels = new Dictionary<string, GUIPanel>(); //控件目录
        private LinkedList<GUIPanel> activeControls = new LinkedList<GUIPanel>(); //活跃中的控件

        public Canvas WorldCanvas{ get => worldCanvas; }
        public Canvas NormalCanvas { get => normalCanvas; }
        public Canvas NoCastCanvas { get => noCastCanvas; }
        public Canvas TopCanvas { get => topCanvas; }

        protected override void Awake()
        {
            base.Awake();
            noCastSorter = NoCastCanvas.GetComponent<ControlSorter>();
            normalSorter = NormalCanvas.GetComponent<ControlSorter>();
            topSorter = TopCanvas.GetComponent<ControlSorter>();
        } 

        /// <summary>
        /// 当有键盘按下时触发的事件
        /// </summary>
        public event Action OnKeyPressed;

        /// <summary>
        /// 从目录中加载面板预制体
        /// </summary>
        /// <param name="registryName"></param>
        private void Load(string registryName)
        {
            var res = GR.Get<GUIPanelResource>(registryName);
            var obj = res?.Copy ?? null;
            var gui = obj?.GetComponent<GUIPanel>() ?? null;
            if (gui == null)
            {
                Debug.LogError("GUI面板预制体出错: 资源不存在 或 缺失'GUIPanel'组件");
                return;
            }
            obj.name = registryName;
            gui.Layer = res.layer;
            gui.RegistryName = registryName;
            var rectt = gui.GetComponent<RectTransform>();

            switch (gui.Layer)
            {
                case GUIPanel.GUILayer.Normal:
                    rectt.SetParent(NormalCanvas.transform);
                    normalSorter.AddControl(gui);
                    //normalSorter.UpdateSort(); 
                    break;

                case GUIPanel.GUILayer.NoCast:
                    rectt.SetParent(NoCastCanvas.transform);
                    noCastSorter.AddControl(gui);
                    //noCastSorter.UpdateSort();
                    break;

                case GUIPanel.GUILayer.Top:
                    rectt.SetParent(TopCanvas.transform);
                    topSorter.AddControl(gui);
                    //topSorter.UpdateSort();
                    break;

                default:
                    Debug.LogWarning($"未知画布层: {gui.Layer}");
                    rectt.SetParent(NormalCanvas.transform);
                    normalSorter.AddControl(gui);
                    //normalSorter.UpdateSort();
                    break;
            }
            //由于重设sort会触发重新排序, 所以上方switch就不手动调用 UpdateSort()
            gui.Sort = res.sort;

            rectt.anchoredPosition = res.anchoredPosition;
            rectt.offsetMin = res.offsetMin;
            rectt.offsetMax = res.offsetMax;
            rectt.anchorMax = res.anchorMax;
            rectt.anchorMin = res.anchorMin;
            rectt.localScale = Vector3.one;
            panels[registryName] = gui;

            obj.SetActive(false);
        }

        /// <summary>
        /// 避免重复加载, 从目录中加载面板预制体
        /// </summary>
        /// <param name="registryName"></param>
        public void ELoad(string registryName)
        {
            if(!Contains(registryName))
                Load(registryName);
        }

        public void UpdateSort(GUIPanel.GUILayer layer)
        {
            ControlSorter sorter = null;
            switch (layer)
            {
                case GUIPanel.GUILayer.Normal:
                    sorter = normalSorter;

                    break;

                case GUIPanel.GUILayer.NoCast:
                    sorter = noCastSorter;
                    break;

                case GUIPanel.GUILayer.Top:
                    sorter = topSorter;
                    break;

                default:
                    Debug.LogWarning($"未知画布层: {layer}");
                    break;
            }
            if (sorter != null)
            {
                sorter.Refresh();
                sorter.UpdateSort();
            }
        }

        public GUIPanel GetPanel(string registryName)
        {
            if (panels.TryGetValue(registryName, out var obj))
            {
                return obj;
            }
            return null;
        }

        public bool Contains(string registryName)
        {
            if (panels.TryGetValue(registryName, out var obj))
            {
                return obj != null;
            }
            return false;
        }


        public void Display(string registryName)
        {
            var ctl = GetPanel(registryName);
            if (ctl != null)
            {
                ctl.IsVisible = true;
            }
        }

        public void Hide(string registryName)
        {
            var ctl = GetPanel(registryName);
            if (ctl != null)
            {
                ctl.IsVisible = false;
            }
        }

        public void SetWithStack(GUIPanel panel)
        {
            if (panel.WithStack)
            {
                if (activeControls.Last != null)
                {
                    activeControls.Last.Value.InputActive = false;
                }
                activeControls.AddLast(panel);
            }
            panel.InputActive = true;
        }

        public void OffWithStack(GUIPanel panel)
        {
            if (panel.WithStack)
            {
                activeControls.Remove(panel);
                if (activeControls.Last != null)
                {
                    activeControls.Last.Value.InputActive = true;
                }
            }
            panel.InputActive = false;
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                OnKeyPressed?.Invoke();
            }
        }
    }
}