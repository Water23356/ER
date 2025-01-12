using ER;
using UnityEngine;

namespace Dev.AI
{
    public class Foam:Water
    {
        [SerializeField]
        private Health health;
        [SerializeField]
        private CellMachine cellMachine;

        public override void OnGetFormPool()
        {
            health.health = health.healthMax * 2 / 3;
        }
        public override void ResetState()
        {
            health.health = health.healthMax * 2 / 3;
        }
    }
}