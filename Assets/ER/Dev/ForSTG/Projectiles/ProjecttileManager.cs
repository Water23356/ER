
using UnityEngine;

namespace ER.STG
{
    public class ProjectileManager : MonoSingletonAutoCreate<ProjectileManager>
    {
        private readonly string pool_normal = "pool:projectile/normal";
        private readonly string pool_sin = "pool:projectile/sin";

        public void Shoot(Vector2 startPos, Vector2 speed)
        {
            Shoot(startPos, speed, Color.white);
        }

        public void Shoot(Vector2 startPos, Vector2 speed, Color color)
        {
            var obj = WaterPoolManager.Instance.GetObject(pool_normal);
            var prjt = obj as Projectile;
            prjt.transform.position = startPos;
            prjt.Speed = speed;
            prjt.Color = color;
            prjt.enabled = true;

        }

        public void ShootSin(Vector2 startPos, Vector2 speed, Color color)
        {
            var obj = WaterPoolManager.Instance.GetObject(pool_sin);
            var prjt = obj as ProjectileSin;
            prjt.transform.position = startPos;

            prjt.SetMoveInfo(new ProjectileSin.Setting
            {
                angleRange = 120f,
                changeCD = 10,
                maxTimes = 20,
                startOffset = 0.25f,
                speed = speed,
            });

            prjt.enabled = true;
        }
    }
}