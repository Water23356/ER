using ER.STG.Tracks;
using System.Collections.Generic;
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

        public void Shoot(ShootProperty property, string tag)
        {
            var obj = WaterPoolManager.Instance.GetObject(pool_normal);
            var prjt = obj as Projectile;
            prjt.transform.position = property.startPos;
            prjt.Speed = property.speed;
            prjt.Color = Color.white;
            prjt.enabled = true;

            prjt.DisableAllTack();
            switch (property.track)
            {
                case ShootProperty.TrackType.None:
                    break;

                case ShootProperty.TrackType.Sin:
                    var sin = prjt.GetOrCreateTrack<SinTrack>();
                    sin.Set(property.props);
                    sin.enabled = true;
                    break;

                case ShootProperty.TrackType.Accelerate:
                    var accelerate = prjt.GetOrCreateTrack<AccelerateTrack>();
                    accelerate.Set(property.props);
                    accelerate.enabled = true;
                    break;
            }
        }
    }

    public struct ShootProperty
    {
        public enum TrackType
        {
            /// <summary>
            /// 一般轨道
            /// </summary>
            None,

            /// <summary>
            /// Sin 周期
            /// </summary>
            Sin,

            /// <summary>
            /// 固定加速度
            /// </summary>
            Accelerate,
        }

        public Vector2 startPos;
        public Vector2 speed;

        public TrackType track;

        public Dictionary<string, object> props;
    }
}