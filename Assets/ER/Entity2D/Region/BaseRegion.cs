
using System;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 基础触发器区域封装属性;
    /// 要求依附物体必须包含一个 Collider2D 同时设置为 Triigger;
    /// </summary>
    public class BaseRegion : MonoAttribute, IRegion
    {
        [Tooltip("标签过滤器")]
        [SerializeField]
        private TagFilter tagFilter;

        private IRegionOwner regionOwner;

        public TagFilter TagFilter { get => tagFilter; set => tagFilter = value; }
        IRegionOwner IRegion.Owner { get => regionOwner; set => regionOwner=value; }

        public Vector2 Position => transform.position;



        #region 事件

        public event Action<IRegion,Collider2D> EnterEvent;

        public event Action<IRegion, Collider2D> StayEvent;

        public event Action<IRegion, Collider2D> ExitEvent;

        #endregion 事件

        public BaseRegion()
        {
            
        }

        public override void Initialize()
        {
            regionOwner = owner;
        }

        protected virtual void VirtualEnter(Collider2D collider)
        {
            if (tagFilter == null)
            {
                EnterEvent?.Invoke(this,collider);
                return;
            }
            if (tagFilter.Filter(collider.tag))
                EnterEvent?.Invoke(this, collider);
        }

        protected virtual void VirtualStay(Collider2D collider)
        {
            if (tagFilter == null)
            {
                StayEvent?.Invoke(this, collider);
                return;
            }
            if (tagFilter.Filter(collider.tag))
                StayEvent?.Invoke(this, collider);
        }

        protected virtual void VirtualExit(Collider2D collider)
        {
            if (tagFilter == null)
            {
                ExitEvent?.Invoke(this, collider);
                return;
            }
            if (tagFilter.Filter(collider.tag))
                ExitEvent?.Invoke(this, collider);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log("collision接触:" + collision.gameObject.tag);
            VirtualEnter(collision.collider);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            VirtualStay(collision.collider);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            VirtualExit(collision.collider);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            VirtualEnter(collider);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            VirtualStay(collider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            VirtualExit(collider);
        }
    }
}