using UnityEngine;

namespace ER.Entity2D
{
    public class ColliderFlag : MonoBehaviour
    {

        public ColliderGroupArea group;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (group == null) return;
            var flag = collision.collider.GetComponent<ColliderFlag>();
            if (flag != null && flag.enabled && flag.group != null)
            {
                group.OnEnter(flag.group);
            }
            else
            {
                group.OnEnter(collision.collider);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (group == null) return;
            var flag = collision.collider.GetComponent<ColliderFlag>();
            if (flag != null && flag.enabled && flag.group != null)
            {
                group.OnExit(flag.group);
            }
            else
            {
                group.OnExit(collision.collider);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (group == null) return;
            var flag = collision.GetComponent<ColliderFlag>();
            if (flag != null && flag.enabled && flag.group != null)
            {
                group.OnEnter(flag.group);
            }
            else
            {
                group.OnEnter(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (group == null) return;
            var flag = collision.GetComponent<ColliderFlag>();
            if (flag != null && flag.enabled && flag.group != null)
            {
                group.OnExit(flag.group);
            }
            else
            {
                group.OnExit(collision);
            }
        }
    }
}