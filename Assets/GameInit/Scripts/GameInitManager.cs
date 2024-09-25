using Dev;
using Dev2;
using ER;
using ER.ForEditor;
using GameSetting;
using System.Collections;
using UnityEngine;

/// <summary>
/// 游戏初始化管理器
/// </summary>
public class GameInitManager : MonoBehaviour
{
    /*
     * 1. 设置读取和应用更改
     * 2. 读取初始化目录 (init) 和 外部初始化目录 (init)
     * 3. 加载初始化脚本 (lua)
     */
    /* 初始化顺序:
     * 1. 设置加载和应用
     * 2. 组织 LOGO
     * 3. 初始化资源字典
     * 4. 初始化 必须资源目录 中的资源
     * 5. 装载指令字典
     * 6. 跳转至主场景 (由场景跳转负责大部分资源加载)
     */

    #region 属性

    [Header("配置")]
    [DisplayLabel("跳过设置加载")]
    [SerializeField]
    private bool skipSettingLoad = false;

    [DisplayLabel("默认索引表")]
    [SerializeField]
    private MetaIndexDic defaultIndexer;

    [DisplayLabel("初始化包")]
    [SerializeField]
    private MetaDic initDic;

    [DisplayLabel("游戏 Logo 显示器")]
    [SerializeField]
    private Logo logo;

    [DisplayLabel("目标场景")]
    [SerializeField]
    private MetaSceneJump aimScene;

    #endregion 属性

    private void Start()
    {
        StartCoroutine(GameInit());
    }

    private IEnumerator GameInit()
    {
        LoadAndApplySettings();
        yield return PlayLogo();
        LoadIndexer();
        yield return LoadInitResource();
        ModifyCommandDictionary();
        SkipScene();

        //销毁自身
        //Destroy(gameObject);
    }

    private void LoadAndApplySettings()
    {
        if (!skipSettingLoad)
        {
            SettingManager.Instance.Init();
            SettingManager.Instance.LoadFromFile();
        }
    }

    private IEnumerator PlayLogo()
    {
        if (logo != null)//播放 logo 并等待播放完毕
        {
            bool wait = true;
            logo.PlayLogo(() => wait = false);
            yield return new WaitWhile(() => wait);
        }
    }

    private void ModifyCommandDictionary()
    {
        ConsolePanel.dictionary.Modify<GameCD>();
        ConsolePanel.dictionary.Modify<MathCD>();
        ConsolePanel.dictionary.Modify<TestCD>();
    }

    private void LoadIndexer()
    {
        ResourceIndexer.Instance.Modify(defaultIndexer);
    }

    private IEnumerator LoadInitResource()
    {
        bool wait = true;
        GameResource.Instance.OnTaskListDone += () => wait = false;
        GameResource.Instance.AddLoadDic(initDic);
        GameResource.Instance.StartLoadTask();
        yield return new WaitWhile(() => wait);
    }

    private void SkipScene()
    {
        if (aimScene != null)
            SceneJumper.Instance.ToScene(aimScene.GetInfo());
    }
}