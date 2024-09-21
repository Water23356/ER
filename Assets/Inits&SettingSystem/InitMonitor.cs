using Dev;
using ER;
using ER.ForEditor;
using ER.GUI;
using ER.Resource;
using ER.SceneManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameResource = Dev.GameResource;

/// <summary>
/// 游戏初始化总监视器, 单例(勿重复创建实例)
/// </summary>
public class InitMonitor : MonoBehaviour
{
    #region 定义

    [Serializable]
    public struct LoadStep
    {
        public string displayText;
        public string path;
    }

    #endregion 定义

    #region 单例相关

    private static InitMonitor instance;

    public static void InitCallback()
    {
        instance?.AddProgress();
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion 单例相关

    #region 组件

    public LoadProgressPanel progressPanel;

    public SettingGroupResource settingGroup;

    [Header("初始化器")]
    [DisplayLabel("游戏资源管理器")]
    public GameResource gameResource;

    [DisplayLabel("资源初始化")]
    public ResourceLoadInit resourceLoadInit;

    #endregion 组件

    #region 启动选项

    [Header("启动选项")]
    [SerializeField]
    [DisplayLabel("跳过控制台加载")]
    private bool skipConsole = false;

    [SerializeField]
    [DisplayLabel("跳过设置加载")]
    private bool skipSettings = false;

    [SerializeField]
    [DisplayLabel("加载步骤")]
    private List<LoadStep> loadSteps = new List<LoadStep>();

    [SerializeField]
    [DisplayLabel("目标跳转场景")]
    private MonoSceneConfigure aimScene;

    [SerializeField]
    [DisplayLabel("场景过渡器")]
    private SceneTransition sceneTransition;

    #endregion 启动选项

    #region 调试方法

    [ContextMenu("读取设置")]
    private void _UpdateSettings()
    {
        GameSettings.Instance.UpdateSettings();
    }

    [ContextMenu("写入设置")]
    private void _SaveSettings()
    {
        GameSettings.Instance.Save();
    }

    #endregion 调试方法

    private int progress = 0; //当前进度

    private void Start()
    {
        ZeroInit();
    }

    private void ZeroInit()
    {
        if (!skipSettings)
        {
            //全局设置
            GameSettings.settingGroup = settingGroup;
            //显示设置注入
            GameSettings.Instance.OnSettingChanged += DisplaySettingMonitor.Instance.PullSettings;
            //控制设置注入
            GameSettings.Instance.OnSettingChanged += ControlSettingMonitor.Instance.PullSettings;
            //音乐设置注入
            GameSettings.Instance.OnSettingChanged += AudioSettingMonitor.Instance.PullSettings;
            //性能设置注入
            GameSettings.Instance.OnSettingChanged += PerformanceSettingMonitor.Instance.PullSettings;
            //测试设置注入
            GameSettings.Instance.OnSettingChanged += DebugSettingMonitor.Instance.PullSettings;
            //更新全局设置
            GameSettings.Instance.UpdateSettings();
            //保存设置更改
            GameSettings.Instance.Save();
        }
        AddProgress();
    }

    private void AddProgress()
    {
        progress++;
        progressPanel.SetProgressSum((float)progress / (loadSteps.Count + 3));

        if (progress == 1)
        {
            progressPanel.SetStatusTxt("Initializing Resource Loader");
            gameResource.FixedAllResourceLoader();
        }
        else if (progress == 2)
        {
            //设置控制台的解析器
            if (!skipConsole)
            {
                ConsolePanel.dictionary.Modify<GameCD>();
                ConsolePanel.dictionary.Modify<MathCD>();
                ConsolePanel.dictionary.Modify<TestCD>();
                //ConsolePanel.interpreter = new CInterpreter();
                GUIManager.Instance.ELoad("gui:origin:console");
            }
            InitMonitor.InitCallback();
        }
        else if (progress > loadSteps.Count + 2)
        {
            progressPanel.SetStatusTxt("Done");
            sceneTransition.OnEnterKeepState += LoadEnd;
            sceneTransition.EnterTransition();//先启动过渡动画, 然后延迟1s加载场景, 不然场景加载太快, 进入过渡动画都还没播完就跳转了:P
            sceneTransition.OnExitKeepState += () =>
            {
                progressPanel.IsVisible = false;
            };
        }
        else
        {
            progressPanel.SetStatusTxt(loadSteps[progress - 3].displayText);
            resourceLoadInit.Load(loadSteps[progress - 3].path);
        }
    }

    private void LoadEnd()
    {
        if (aimScene == null)
        {
            Debug.LogError("未配置目标跳转场景!");
            return;
        }
        SceneManager.Instance.LoadScene(aimScene, sceneTransition, true);

#if false
        GUIManager.Instance.ELoad("gui:origin:test_control_panel");
        GUIManager.Instance.GetPanel("gui:origin:test_control_panel").IsVisible = true;
#endif
    }
}