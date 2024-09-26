using UnityEngine;

namespace Dev
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RigidbodySpeedDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Color color = new Color(0.8f, 0.6f, 0f, 1f);


        private Rigidbody2D m_rigidbody;

        public Rigidbody2D rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>(); 
                return m_rigidbody;
            }
        }

        private void Update()
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3)rigidbody.velocity, color);
        }
    }
}