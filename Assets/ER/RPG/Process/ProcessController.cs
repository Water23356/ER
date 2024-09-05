using System;
using System.Collections.Generic;

namespace ER.RPG
{
    /// <summary>
    /// 流程控制器(不使用单例为的是可以嵌入到其他模块当中)
    /// </summary>
    public class ProcessController
    {
        private Dictionary<string, Node> nodes;//节点池

        private List<string> root;//根节点(起始节点)

        public event Action<string> onNodeEnable;//当节点被完成时触发的事件 (节点名)

        public event Action<string> onNodeLoading;//当节点被加载时触发的事件 (节点名)

        public event Action<string, Node.Status> onNodeLocked;//当节点被锁定时触发的事件 (节点名,原先节点状态)

        public Node GetNode(string key)
        {
            if (nodes.TryGetValue(key, out var node))
            {
                return node;
            }
            return null;
        }

        public Node GetParentNode(string key)
        {
            var node = GetNode(key);
            if (node != null)
            {
                return GetNode(node.parent);
            }
            return null;
        }

        /// <summary>
        /// 获取指定节点状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsDone(string key)
        {
            if (nodes.TryGetValue(key, out Node node))
            {
                return node.IsDone;
            }
            return false;
        }

        /// <summary>
        /// 获取指定节点是否锁定
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsLocked(string key)
        {
            if (nodes.TryGetValue(key, out Node node))
            {
                return node.IsLocked;
            }
            return false;
        }

        /// <summary>
        /// 判断指定节点是否为加载节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsLoading(string key)
        {
            if (nodes.TryGetValue(key, out Node node))
            {
                return node.IsLoading;
            }
            return false;
        }

        /// <summary>
        /// 加载指定节点(只对满足条件的节点生效
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool LoadNode(string key)
        {
            if (nodes.TryGetValue(key, out Node node))
            {
                LoadNode(node);
            }
            return false;
        }

        private bool LoadNode(Node node)
        {
            if (node.IsLocked)//被锁定无法被加载
            {
                return false;
            }

            var parent = GetNode(node.parent);
            if (parent == null || parent.IsDone)//只有无源节点 或者 父节点已经完成后才能加载该节点
            {
                LoadNodeNoCheck(node);
                return true;
            }
            return false;
        }

        private void LoadNodeNoCheck(Node node)
        {
            node.status = Node.Status.Loading;
            onNodeLoading?.Invoke(node.name);
        }

        /// <summary>
        /// 完成指定节点(只对加载节点生效)(自动加载后续节点)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DoneNode(string key)
        {
            if (nodes.TryGetValue(key, out Node node))
            {
                if (node.IsLoading)
                {
                    node.status = Node.Status.Enable;
                    onNodeEnable?.Invoke(key);

                    //加载非锁定子节点
                    foreach (var sub in node.childs)
                    {
                        var subNode = GetNode(sub);
                        if (!subNode.IsLocked)
                        {
                            LoadNodeNoCheck(subNode);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 锁定指定节点(只对加载节点生效)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool LockNode(string key)
        {
            if (nodes.TryGetValue(key, out Node node))
            {
                if (node.IsLoading)
                {
                    var old = node.status;
                    node.status = Node.Status.Locked;
                    onNodeLocked?.Invoke(key, old);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 解锁指定节点(只能对锁定节点生效)(如果满足加载条件会自动加载)
        /// </summary>
        /// <returns></returns>
        public bool UnlockNode(string key)
        {
            if (nodes.TryGetValue(key, out Node node))
            {
                if (node.IsLocked)//被锁定
                {
                    node.status = Node.Status.Disable;
                    var parent = GetNode(node.parent);
                    if (parent.IsDone)
                    {
                        LoadNodeNoCheck(node);
                    }
                    return true;
                }
            }
            return false;
        }

        public void InitWithGraph(ProcessGraph graph)
        { 
            foreach(var node in graph)
            {
                nodes[node.name] = new Node(node);//加入节点池
                if(string.IsNullOrEmpty(node.parent))//为根节点
                {
                    root.Add(node.name);
                }
            }
        }

        /// <summary>
        /// 从根节点出发
        /// </summary>
        public void StartFromRoot()
        {
            foreach(var node in root)
            {
                LoadNode(node);
            }
        }

    }
}