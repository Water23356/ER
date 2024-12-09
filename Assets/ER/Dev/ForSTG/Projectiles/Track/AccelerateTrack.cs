using System.Collections.Generic;
using UnityEngine;

namespace ER.STG.Tracks
{
    public class AccelerateTrack : ProjectileTrack
    {
        private Vector2 accelerate;

        public override void Set(Dictionary<string, object> props)
        {
            accelerate = (Vector2)props["accelerate"];
        }

        private void FixedUpdate()
        {
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            owner.velocity += accelerate;
        }
    }
}