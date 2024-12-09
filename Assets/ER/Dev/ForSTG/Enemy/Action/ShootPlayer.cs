using ER.ForEditor;
using UnityEngine;

namespace ER.STG.Actions
{
    /// <summary>
    /// 连射自机狙
    /// </summary>
    public class ShootPlayer : EntityAction
    {
        [SerializeField]
        [DisplayLabel("弹幕速度")]
        private float speed = 5f;

        [SerializeField]
        [DisplayLabel("射击间隔")]
        private int shootCD = 5;

        [SerializeField]
        [DisplayLabel("射击次数")]
        private int shootTimes = 5;

        private int _counter = 0;
        private int _timer;

        public override void ActionEffect()
        {
            _counter = shootTimes;
            _timer = 0;
            enabled = true;
        }

        private void ShootToPlayer()
        {
            var player = AM.GetAnchor("player").Owner as Entity;
            var dir = Vector2.down;
            if (player != null)
            {
                dir = (player.transform.position - transform.position).normalized;
            }
            Shoot(dir * speed, Color.white);
            _counter -= 1;
        }

        private void FixedUpdate()
        {
            if (_counter <= 0)
            {
                enabled = false;
                return;
            }

            if (_timer % shootCD == 0)
            {
                ShootToPlayer();
            }
            _timer += 1;
        }
    }
}