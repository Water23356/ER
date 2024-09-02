using System;
using System.Collections.Generic;

namespace ER.Extender
{
    [Obsolete("不建议使用")]
    public class DecoratorManager : Singleton<DecoratorManager>
    {
        private Dictionary<IDecoratorOwner, Decorator> decorators = new Dictionary<IDecoratorOwner, Decorator>();

        /// <summary>
        /// 给一个对象注册装饰器
        /// </summary>
        /// <param name="obj"></param>
        public Decorator Register(IDecoratorOwner obj)
        {
            return decorators[obj] = new Decorator();
        }

        /// <summary>
        /// 注销一个对象的装饰器
        /// </summary>
        /// <param name="obj"></param>
        public void Unregister(IDecoratorOwner obj)
        {
            if (Contains(obj))
                decorators.Remove(obj);
        }

        /// <summary>
        /// 判断该对象是否注册了装饰器
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Contains(IDecoratorOwner obj)
        {
            return decorators.ContainsKey(obj);
        }

        /// <summary>
        /// 获取指定对象的装饰器
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Decorator Get(IDecoratorOwner obj)
        {
            if (decorators.TryGetValue(obj, out Decorator decorator))
            {
                return decorator;
            }
            return null;
        }
    }

    public static class DMExpand
    {
#if false
        /// <summary>
        /// 获取装饰器中的对象(需要注册进 DecoratorManager)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="don"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T DMGet<T>(this IDecoratorOwner don, string key, T defaultValue = default(T))
        {
            Decorator dt = DecoratorManager.Instance.Get(don);
            if (dt == null) return defaultValue;
            if (dt.Contains(key))
            {
                return dt.Get<T>(key);
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置装饰器中的对象(需要注册进 DecoratorManager)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="don"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void DMSet<T>(this IDecoratorOwner don, string key, T value)
        {
            Decorator dt = DecoratorManager.Instance.Get(don);
            if (dt == null)
            {
                dt = DecoratorManager.Instance.Register(don);
            }
            dt.Set(key, value);
        }

        /// <summary>
        /// 销毁装饰器; 请在不再使用对象时移除其装饰器(需要注册进 DecoratorManager)
        /// </summary>
        /// <param name="don"></param>
        public static void DMDispose(this IDecoratorOwner don)
        {
            DecoratorManager.Instance.Unregister(don);
        }

        /// <summary>
        /// 获取自身的装饰中的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="don"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T DGet<T>(this IDecoratorOwner don, string key, T defaultValue = default(T))
        {
            Decorator dt = don.DData;
            if (dt == null) return defaultValue;
            if (dt.Contains(key))
            {
                return dt.Get<T>(key);
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置自身装饰器中的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="don"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void DSet<T>(this IDecoratorOwner don, string key, T value)
        {
            Decorator dt = don.DData;
            if (dt == null)
            {
                dt = DecoratorManager.Instance.Register(don);
            }
            dt.Set(key, value);
        }
    }
#endif
    }
}