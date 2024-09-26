using UnityEngine;

namespace Dev
{
    [RequireComponent(typeof(ORCA))]
    public class ORCASteer : MonoBehaviour
    {
        private ORCA orca;

        [SerializeField]
        private Transform aimPos;

        public ORCA agent
        {
            get
            {
                if (orca == null)
                    orca = GetComponent<ORCA>();
                return orca;
            }
        }

        private void Update()
        {
            if (agent == null) return;
            if (aimPos == null) return;
            var dir = aimPos.position - transform.position;
            if (dir.magnitude < 0.001f)
            { 
                agent.PreferredVelocity = Vector2.zero; 
            }
            else
            {
                agent.PreferredVelocity = (aimPos.position - transform.position)*agent.MaxSpeed;
            }
        }
    }
}