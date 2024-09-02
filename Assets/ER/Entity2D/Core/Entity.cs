// Ignore Spelling: Rigidbody

using System;
using System.Collections.Generic;
using UnityEngine;

namespace  ER.Entity2D
{
    /// <summary>
    /// 带有区域判定的对象，简称为 实体(Entity)；
    /// Entity 类表示的是一个对象整体，可以保证各 特征组件 有顺序的初始化
    /// 通常来说 一个2D对象可能由很多 GameObject 和 Component 组成，需要有一个 代表对象 来表示这个游戏对象；
    /// 也就是说 Entity 必须是一个 通用的 描述广泛的 对象代表类；
    /// 需要保证可以 以 Entity 为媒介 便捷地传递消息；
    /// </summary>
    public class Entity : MonoBehaviour, IRegionOwner
    {
        public enum FaceDir { Left,Right}
        #region 属性

        [Tooltip("预加载特征列表")]
        public List<MonoAttribute> AttributesLoader;
        [SerializeField]
        private EntityConfigurator configurator;

        protected Dictionary<System.Type,IAttribute> attributes = new();

        protected FaceDir faceDir = FaceDir.Right;
        /// <summary>
        /// 实体朝向(手向, 默认右手向, 脸朝右)
        /// </summary>
        public FaceDir Direction
        {
            get => faceDir;
            set
            {
                if(faceDir != value)//每次改变值, 翻转y轴
                {
                    transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,0);
                }
                faceDir = value;
            }
        }

        private new Rigidbody2D rigidbody;

        public Rigidbody2D Rigidbody => rigidbody;

        #endregion 属性


        #region 特征管理

        /// <summary>
        /// 向该实体添加新的特征对象，并初始化特征；
        /// 如果新填特征的名称为null，添加失败并返回 true；
        /// 如果发生名称冲突，返回 true；
        /// </summary>
        /// <param name="attribute">特征对象</param>
        /// <param name="cover">发生名称冲突时是否强制覆盖</param>
        /// <returns>添加时是否存在冲突，</returns>
        public bool Add(IAttribute attribute, bool cover = false)
        {
            System.Type type = attribute.GetType();
            if (attributes.ContainsKey(type) && !cover) { return true; }
            attributes[type] = attribute;
            attribute.Owner = this;
            attribute.Initialize();
            return false;
        }

        /// <summary>
        /// 移除指定特征
        /// </summary>
        /// <param name="attributeName">特征名称</param>
        /// <returns>是否移除成功</returns>
        public bool Remove<T>()
        {
            if (Exist<T>())
            {
                attributes.Remove(typeof(T));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该实体是否拥有指定特征
        /// </summary>
        /// <param name="attributeName">特征名称</param>
        /// <returns></returns>
        public bool Exist<T>()
        {
            return attributes.ContainsKey(typeof(T));
        }

 

        /// <summary>
        /// 获取指定特征，返回找到的第一个特征对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAttribute<T>() where T : class,IAttribute
        {
            System.Type type = typeof(T);
            if(attributes.TryGetValue(type, out var attribute))
            {
                return attribute as T;
            }

            return default(T);
        }

        #endregion 特征管理

        #region Unity

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            configurator?.Install(this);

            if (AttributesLoader != null && AttributesLoader.Count > 0)
            {
                foreach (var attribute in AttributesLoader)
                {
                    if (Add(attribute, false))
                    {
                        Debug.LogWarning($"预加载特性发生成冲突:{attribute.GetType().Name}");
                    }
                }
            }
            Initialized();
        }

        protected virtual void Initialized()
        {
        }

        #endregion Unity
    }
}