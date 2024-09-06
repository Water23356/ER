using ER.ForEditor;
using UnityEngine;

namespace ER.STG
{
    /// <summary>
    /// 自动根据速度修正方向
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class AutoFixDir : MonoBehaviour
    {
        [DisplayLabel("默认朝向")]
        [SerializeField]
        private Vector2 defaultDir;

        private Rigidbody2D m_rigidbody;

        public Rigidbody2D Rigidbody
        { get { if (m_rigidbody == null) m_rigidbody = GetComponent<Rigidbody2D>(); return m_rigidbody; } }

        private void Update()
        {
            var dir = Rigidbody.velocity;
            if (dir != Vector2.zero)
            {
                var angle = defaultDir.ClockAngle(dir);
                transform.localEulerAngles = new Vector3(0,0, angle);
            }
        }
    }
}