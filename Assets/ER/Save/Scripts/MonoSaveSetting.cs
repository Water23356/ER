
using System;
using UnityEngine;

namespace ER.Save
{
    /// <summary>
    /// 用于在 检查器快速配置 存档系统(一次性, start之后自动销毁)
    /// </summary>
    public class MonoSaveSetting:MonoBrief
    {
        [Tooltip("存档文件夹预设路径")]
        public string SavePackPath;

        public override void Init(Action callback)
        {
            SaveManager.Instance.savePackPath = SavePackPath;
            callback();
        }
    }
}