using ER.ForEditor;
using UnityEngine;

namespace ER.Entity2D.Components
{
    /// <summary>
    /// 自动根据速度调整方向; 需要携带 IVelocityObject 脚本
    /// </summary>
    public class AutoFixDir:MonoBehaviour
    {
        [DisplayLabel("默认朝向")]
        [SerializeField]
        private Vector2 defaultDir;
        private IVelocityObject owner;
        private void Awake()
        {
            owner = GetComponent<IVelocityObject>();
            if(owner==null)
            {
                Debug.LogWarning("AutoFixDir 缺失 IVelocityObject 主体组件无法工作");
                enabled = false;
            }
        }
        private void Update()
        {
            var dir = owner.velocity;
            if (dir != Vector2.zero)
            {
                var angle = defaultDir.ClockAngle(dir);
                transform.localEulerAngles = new Vector3(0, 0, angle);
            }
        }
    }
    public interface IVelocityObject
    {
        public Vector2 velocity { get; set; }
    }
}