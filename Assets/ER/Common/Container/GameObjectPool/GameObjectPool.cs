using ER;
using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    public sealed class GameObjectPool : MonoBehaviour
    {
        /// <summary>
        /// 当存量不足时, 自动扩充的容量, 为0或者负数表示不自动扩充
        /// </summary>
        [DisplayLabel("自动拓展大小")]
        public int autoExpandSize = 10;
        [DisplayLabel("池的默认大小")]
        [SerializeField]
        private int defaultSize = 20;

        /// <summary>
        /// 池的大小
        /// </summary>
        public int PoolSize
        {
            get => pool.Count;
        }
        public GameObject originPrefab;
        private Queue<GameObject> pool = new Queue<GameObject>();


        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="status"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject GetObject(bool status = true, Transform parent = null)
        {
            GameObject obj;
            if (!pool.TryDequeue(out obj))
            {
                for (int i = 0; i < defaultSize; i++)
                {
                    var _obj = GameObject.Instantiate(originPrefab, transform);
                    _obj.SetActive(false);
                    ReturnObject(obj);
                }
                pool.Enqueue(obj);
            }
            obj.SetActive(status);
            obj.transform.SetParent(parent);
            var anchor = obj.GetComponent<ObjectPoolAnchor>();
            if (anchor != null)
            {
                anchor.ResetStatus();
            }
            return obj;

        }
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        private void Awake()
        {
            for (int i = 0; i < defaultSize; i++)
            {
                var obj = GameObject.Instantiate(originPrefab, transform);
                obj.SetActive(false);
                ReturnObject(obj);
            }
        }


    }
}
