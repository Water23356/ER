﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// AnchorManager 锚点管理类（非组件单例模式）, 简写为AM
    /// 注意: 当为一个对象注册锚点时, 需要在对象销毁时注销锚点
    /// </summary>
    public static class AM
    {
        #region 属性

        /// <summary>
        /// 当有新的锚点注册时触发的事件
        /// </summary>
        public static event Action<Anchor> OnAnchorAdd;

        /// <summary>
        /// 当锚点注销时触发的事件
        /// </summary>
        public static event Action<Anchor> OnAnchorRemove;

        /// <summary>
        /// 当该锚点被添加时自动处理的委托列表
        /// </summary>
        private static List<Func<Anchor, bool>> OnAnchorAddHandle = new();

        /// <summary>
        /// 锚点列表
        /// </summary>
        private static Dictionary<string, Anchor> anchors = new Dictionary<string, Anchor>();

        #endregion 属性

        #region 功能函数

        private static void HandleAddAnchor(Anchor anchor)
        {
            for (int i = OnAnchorAddHandle.Count - 1; i >= 0; i--)
            {
                if (OnAnchorAddHandle[i]?.Invoke(anchor) ?? true)
                {
                    OnAnchorAddHandle.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 添加锚点
        /// </summary>
        /// <param name="anchor">锚点对象</param>
        public static void AddAnchor(Anchor anchor)
        {
            if (anchors.ContainsKey(anchor.AnchorTag))
            {
                anchors[anchor.AnchorTag] = anchor;
            }
            else
            {
                anchors.Add(anchor.AnchorTag, anchor);
            }
            OnAnchorAdd?.Invoke(anchor);
            HandleAddAnchor(anchor);
        }

        /// <summary>
        /// 移动锚点位置，若锚点不存在则新建锚点
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <param name="x">锚点x位置</param>
        /// <param name="y">锚点y位置</param>
        public static void MoveAnchor(string tag, int x, int y)
        {
            if (!anchors.Keys.Contains(tag))
            {
                VirtualAnchor anchor = new VirtualAnchor(tag, x, y);
                anchors.Add(tag, anchor);
                OnAnchorAdd?.Invoke(anchor);
                HandleAddAnchor(anchor);

                return;
            }
            anchors[tag].Point = new Vector2(x, y);
        }

        /// <summary>
        /// 移动锚点位置，若锚点不存在则新建锚点
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <param name="position">锚点位置</param>
        public static void MoveAnchor(string tag, Vector2 position)
        {
            if (!anchors.Keys.Contains(tag))
            {
                VirtualAnchor anchor = new VirtualAnchor(tag, position.x, position.y);
                anchors.Add(tag, anchor);
                OnAnchorAdd?.Invoke(anchor);
                HandleAddAnchor(anchor);

                return;
            }
            anchors[tag].Point = position;
        }

        /// <summary>
        /// 删除指定锚点
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <returns>操作是否成功</returns>
        public static bool RemoveAnchor(string tag)
        {
            if (anchors.Keys.Contains(tag))
            {
                Anchor anchor = anchors[tag];
                anchors[tag].Destroy();
                anchors.Remove(tag);
                OnAnchorRemove?.Invoke(anchor);
                
                return true;
            }
            return false;
        }
        public static bool RemoveAnchor(Anchor anchor)
        {
            if (anchor == null) return false;
            if (anchors.Keys.Contains(anchor.AnchorTag))
            {
                anchor.Destroy();
                anchors.Remove(anchor.AnchorTag);
                OnAnchorRemove?.Invoke(anchor);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定锚点位置，若锚点不存在则返回null
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <returns></returns>
        public static Vector2? GetAnchorPoint(string tag)
        {
            if (anchors.Keys.Contains(tag))
            {
                return anchors[tag].Point;
            }
            return null;
        }

        /// <summary>
        /// 获取锚点对象
        /// </summary>
        /// <returns></returns>
        public static Anchor GetAnchor(string tag)
        {
            if (anchors.Keys.Contains(tag))
            {
                return anchors[tag];
            }
            return null;
        }

        /// <summary>
        /// 对锚点进行操作, 返回该锚点, 如果锚点不存在则返回null, 并且会等待该锚点注册后自动执行处理函数
        /// </summary>
        /// <returns></returns>
        public static Anchor AnchorApply(string tag, Action<Anchor> handle)
        {
            if (anchors.Keys.Contains(tag))
            {
                var ac = anchors[tag];
                handle?.Invoke(ac);
                return ac;
            }
            if (handle == null) return null;
            OnAnchorAddHandle.Add((anchor) =>
            {
                if (anchor.AnchorTag == tag)
                {
                    handle.Invoke(anchor);
                }
                return false;
            });

            return null;
        }

        #endregion 功能函数
    }
}