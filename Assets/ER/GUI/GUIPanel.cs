using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.GUI
{
    public abstract class GUIPanel : MonoBehaviour, IControlSortItem
    {
        public enum GUILayer
        {
            /// <summary>
            /// 一般GUI层级
            /// </summary>
            Normal,

            /// <summary>
            /// 无碰撞的顶层层级
            /// </summary>
            NoCast,

            /// <summary>
            /// 顶层GUI层级
            /// </summary>
            Top,
        }

        //控件注册名(用于区分不同控件)
        public abstract string RegistryName { get; set; }

        //控制权是否入栈?
        public abstract bool WithStack { get; }

        /// <summary>
        /// 面板的画布层级
        /// </summary>
        public abstract GUILayer Layer { get; set; }

        [SerializeField]
        [DisplayLabel("排序层级")]
        protected int sort;

        //输入是否有效?
        public virtual bool InputActive { get; set; }

        protected bool m_isVisible = false;

        //是否显示
        public virtual bool IsVisible
        {
            get { return m_isVisible; }
            set
            {
                if ((m_isVisible && !value) || (!m_isVisible && value))
                {
                    m_isVisible = value;
                    if (m_isVisible)
                    {
                        GUIManager.Instance?.SetWithStack(this);
                    }
                    else
                    {
                        GUIManager.Instance?.OffWithStack(this);
                    }
                    gameObject.SetActive(m_isVisible);
                }
            }
        }

        public int Sort
        {
            get => sort;
            set
            {
                sort = value;
                GUIManager.Instance?.UpdateSort(Layer);
            }
        }

        public Transform Transform => transform;

        /// <summary>
        /// 获取面板提交的信息
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetPanelInfo();

        /// <summary>
        /// 当面板显示时触发的事件
        /// </summary>
        public event Action OnVisibleEvent;

        /// <summary>
        /// 当面板不可视时触发的事件
        /// </summary>
        public event Action OnInvisibleEvent;

        public void OnVisibleEventClear()
        {
            OnVisibleEvent = null;
        }
        public void OnInvisibleEventClear()
        {
            OnInvisibleEvent = null;
        }


        /// <summary>
        /// 当面板提交时触发的事件
        /// </summary>
        //public event Action OnConfirmEvent;

        [ContextMenu("显示面板")]
        private void _Test_SetVisible()
        {
            IsVisible = true;
        }

        [ContextMenu("隐藏面板")]
        private void _Test_SetInvisible()
        {
            IsVisible = false;
        }

        protected virtual void OnEnable()
        {
            //Debug.Log("面板激活");
            OnVisible();
        }

        protected virtual void OnDisable()
        {
            OnInvisible();
        }

        protected virtual void OnVisible()
        {
            OnVisibleEvent?.Invoke();
        }

        protected virtual void OnInvisible()
        {
            OnInvisibleEvent?.Invoke();
        }
    }
}