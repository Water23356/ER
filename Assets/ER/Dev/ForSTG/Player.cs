using ER.STG.Actions;
using UnityEngine;

namespace ER.STG
{
    /// <summary>
    /// 玩家角色
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Role
    {
        private void Start()
        {
            if (!Actions.ContainsKey("shoot"))
            {
                var action = gameObject.AddComponent<Shoot>();
                action.Owner = this;
                action.ActionName = "shoot";
                action.Timer.limitTick = 5;
                Actions["shoot"] = action;
            }
        }
    }
}