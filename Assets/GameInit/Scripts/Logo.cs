using System;
using UnityEngine;

/// <summary>
/// 游戏发行Logo显示
/// </summary>
public class Logo:MonoBehaviour
{
    public virtual void PlayLogo(Action callback)
    {
        callback?.Invoke();
    }
}