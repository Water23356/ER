using ER;
using System;
using UnityEngine;

namespace STG
{
    /// <summary>
    /// 角色行为
    /// </summary>
    public class EntityAction : MonoBehaviour
    {
        [SerializeField]
        private TickTimer timer;

        [SerializeField]
        private string actionName;

        private Entity owner;

        private Action action;

        public Entity Owner { get => owner; set => owner = value; }
        public TickTimer Timer { get => timer; private set => timer = value; }
        public string ActionName { get => actionName; set => actionName = value; }


        public EntityAction()
        {
            timer = new TickTimer()
            {
                destroyMode = TimerManager.DestroyMode.Disable,
                updateMode = TimerManager.UpdateMode.None,
                tag = "action",
            };
        }

        private void Awake()
        {
            TimerManager.Instance.RegisterTicker(Timer);
            enabled = false;
        }

        private void OnDisable()
        {
            timer.updateMode = TimerManager.UpdateMode.None;
            timer.ticks = 0;
        }

        private void OnDestroy()
        {
            TimerManager.Instance?.UnregisterTicker(Timer);
        }

        public void Execute()
        {
            //Debug.Log($"计时器状态: {Timer.updateMode} ticks: {Timer.counter}");
            if (Timer.updateMode == TimerManager.UpdateMode.None)
            {
                Timer.updateMode = TimerManager.UpdateMode.ScaledTime;
                action?.Invoke();
            }
        }


    }
}