using System.Collections.Generic;
using UnityEngine;

namespace ER.STG.Tracks
{
    [RequireComponent(typeof(Projectile))]
    public abstract class ProjectileTrack : MonoBehaviour
    {
        private Projectile m_owner;

        public Projectile owner
        {
            get
            {
                if (m_owner == null)
                    m_owner = GetComponent<Projectile>();
                return m_owner;
            }
        }

        public abstract void Set(Dictionary<string, object> props);
    }
}