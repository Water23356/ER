
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 复合检测区域;
    /// 复合区域的标签过滤, 会同步到所有子区域检测
    /// </summary>
    public sealed class CompositeRegion : MonoAttribute, IRegion
    {
        [SerializeField]
        private GameObject[] RegionObjects;

        private IRegion[] regions;

        [SerializeField]
        private TagFilter tagFilter;

        private Dictionary<Collider2D, int> counter = new Dictionary<Collider2D, int>();//计数器, 用于复合判断物体与自身的接触事件

        private IRegionOwner regionOwner;

        public TagFilter TagFilter
        {
            get => tagFilter;
            set
            {
                tagFilter = value;
                UpdateFilters();
            }
        }

        IRegionOwner IRegion.Owner { get => regionOwner; set => regionOwner=value; }

        public Vector2 Position => transform.position;

        private void UpdateFilters()
        {
            for (int i = 0; i < regions.Length; i++)
            {
                regions[i].TagFilter = tagFilter;
            }
        }

        public event Action<IRegion,Collider2D> EnterEvent;

        public event Action<IRegion, Collider2D> StayEvent;

        public event Action<IRegion, Collider2D> ExitEvent;


        public override void Initialize()
        {
            List<IRegion> rs = new List<IRegion>();
            for(int i=0;i< RegionObjects.Length;i++)
            {
                IRegion r = RegionObjects[i].GetComponent<IRegion>();
                if (r != null)
                {
                    rs.Add(r);
                    r.TagFilter = tagFilter;
                    r.EnterEvent += Enter;
                    r.ExitEvent += Exit;
                }
            }
            regions = rs.ToArray();
            regionOwner = owner;
        }

        private void Enter(IRegion r, Collider2D c)
        {
            if (counter.TryGetValue(c, out int count))
            {
                counter[c] = count + 1;
                if (count == 0)//初次进入
                {
                    EnterEvent?.Invoke(this,c);
                }
            }
        }

        private void Exit(IRegion r, Collider2D c)
        {
            if (counter.TryGetValue(c, out int count))
            {
                counter[c] = count - 1;
                if (count == 1)//最后一次离开
                {
                    ExitEvent?.Invoke(this,c);
                }
            }
        }

        private void Update()
        {
            foreach (var pair in counter)
            {
                if(pair.Value > 1)
                {
                    StayEvent?.Invoke(this,pair.Key);
                }
            }
        }
    }
}