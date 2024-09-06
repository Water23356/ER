using UnityEngine;

namespace ER.STG.Actions
{
    public class Shoot : RoleAction
    {
        private readonly string poolName = "pool:projectile";

        [SerializeField]
        private float speed = 3f;


        public override void ActionEffect()
        {
            var obj = WaterPoolManager.Instance.GetObject(poolName);
            var prjt = obj as Projectile;
            prjt.transform.position = Owner.transform.position;
            prjt.Speed = Vector2.up * speed;
        }
    }
}