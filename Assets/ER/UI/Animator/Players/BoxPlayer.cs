using ER;
using ER.UI.Animator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ER.UI.Animator.Players
{
    /// <summary>
    /// 盒子类动画播放器
    /// 拓展属性:
    /// * type: 子类别
    /// 支持动画:
    /// - 无起点单向平移: no_start_move
    ///     * dir: 方向(Vector2)
    ///     * distance: 距离(float)
    ///     * speed: 移动速度(float)
    /// </summary>
    public class BoxMovePlayer : IUIPlayer
    {
        public string Type => "box";

        public bool Update(UIAnimationCD cd,float deltaTime)
        {
            NoStartMove(cd, deltaTime);
            return cd.Status != CDStatus.Playing;
        }

        private void NoStartMove(UIAnimationCD cd, float deltaTime)//无起点平移
        {
            float dis = deltaTime * ((Vector2)cd.Marks["distance"]).magnitude;
            float speed = (float)cd.Marks["speed"];
            float done = (float)cd.GetVar("distance_done");
            
            float delta_dis = Mathf.Min(speed * deltaTime,dis-done);//防止移动超过要求距离
            done += delta_dis;
            cd.SetVar("distance_done", done);

            Vector2 dir = ((Vector2)cd.Marks["dir"]).normalized;
            cd.Owner.position += (Vector3)dir * delta_dis;

            if(done>=dis)//表示动画完毕
            {
                cd.Done();
            }
        }
    }
}
