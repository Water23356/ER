using UnityEngine;

namespace ER.STG
{
    public class ProjectileManager : MonoSingletonAutoCreate<ProjectileManager>
    {
        private readonly string poolName = "pool:projectile";

        public void Shoot(Vector2 startPos, Vector2 speed)
        {
            Shoot(startPos, speed, Color.white);
        }

        public void Shoot(Vector2 startPos, Vector2 speed, Color color)
        {
            var obj = WaterPoolManager.Instance.GetObject(poolName);
            var prjt = obj as Projectile;
            prjt.transform.position = startPos;
            prjt.Speed = speed;
            prjt.Color = color;
        }
    }
}