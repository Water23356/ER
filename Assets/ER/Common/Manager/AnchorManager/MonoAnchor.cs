using UnityEngine;

namespace ER
{
    /// <summary>
    /// 组件钩子
    /// </summary>
    public  class MonoAnchor : MonoBehaviour, Anchor
    {
        public string _tag;

        [SerializeField]
        private MonoBehaviour owner;

        public string AnchorTag { get => _tag; set => _tag = value; }
        public Vector3 Point { get => transform.position; set => transform.position = value; }

        public object Owner
        {
            get
            {
                if (owner != null)
                {
                    return owner;
                }
                return gameObject;
            }
        }

        public void Destroy()
        {
            Destroy(this);
        }

        protected virtual void Awake()
        {
            AM.AddAnchor(this);
        }
    }
}