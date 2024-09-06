using UnityEngine;

namespace ER
{
    /// <summary>
    /// 单例模式基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// 组件单例模式基类，过场不销毁, 需要手动创建单例对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : class, new()
    {
        private static T instance;
        protected bool dontDestroyed = true;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //Debug.LogWarning($"单例对象不存在:{typeof(T)}");
                }
                return instance;
            }
        }

        /// <summary>
        /// 替换单例对象为自身，如果已存在则销毁自身
        /// </summary>
        protected void PasteInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                if (dontDestroyed)
                    DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(gameObject);
        }

        protected virtual void Awake()
        {
            PasteInstance();
        }
    }

    /// <summary>
    /// 组件单例模式基类, 过场不销毁, 自动创建单例对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingletonAutoCreate<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static bool isShuttingDown = false; // 标记应用是否在退出

        public static T Instance
        {
            get
            {
                if (isShuttingDown)
                {
                    Debug.Log($"{nameof(T)} 已经被销毁, 不再支持访问");
                    return null; // 避免在应用关闭时创建新实例
                }

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
                return instance;
            }
        }

        /// <summary>
        /// 替换单例对象为自身，如果已存在则销毁自身
        /// </summary>
        protected void PasteInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject); // 销毁重复的实例
            }
        }

        protected virtual void Awake()
        {
            PasteInstance();
        }

        private void OnApplicationQuit()
        {
            isShuttingDown = true; // 标记应用关闭，避免创建新对象
        }
    }
}