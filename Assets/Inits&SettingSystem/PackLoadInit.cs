using ER.ForEditor;
using ER.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初始化器, 初始化顺序: 0
/// </summary>
public class PackLoadInit : MonoBehaviour
{
    [DisplayLabel("初始化加载包路径")]
    public string initPath = "pack:origin:init";

    private Action callback;
    private LoadTask task;

    public void Init(Action callback)
    {
        this.callback = callback;
        var loaders = GetComponents<IResourceLoader>();
        //装载所有资源加载器
        for (int i = 0; i < loaders.Length; i++)
        {
            Debug.Log($"装载资源加载器:{loaders[i].Head}");
            GR.AddLoader(loaders[i]);
        }

        //加载初始化包
        GR.Load(StartLoad, false, initPath);
    }

    #region 加载初始化包中的资源

    private void StartLoad()
    {
        task = GR.Get<LoadPackResource>(initPath).task;
        if (task == null)
        {
            Debug.LogError("获取初始化加载包失败, 资源初始化终止!");
            return;
        }
        GR.AddLoadTask(task);
    }

    private void Done()
    {
        callback?.Invoke();
        InitMonitor.InitCallback();
        enabled = false;
    }

    private void Update()
    {
        if (task != null)
        {
            if (task.progress_load.done && task.progress_load_force.done)
            {
                Done();
            }
        }
    }

    #endregion 加载初始化包中的资源
}