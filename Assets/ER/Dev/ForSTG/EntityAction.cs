using UnityEngine;

namespace ER.STG
{
    /// <summary>
    /// 角色行为
    /// </summary>
    public abstract class EntityAction : MonoBehaviour
    {
        [SerializeField]
        private TickTimer timer;

        [SerializeField]
        private string actionName;

        private Transform owner;

        public Transform Owner { get => owner; set => owner = value; }
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

        public void Shoot(Vector2 speed,Color color)
        {
            ProjectileManager.Instance.Shoot(Owner.transform.position, speed, color);
        }
        public void Shoot(Vector2 speed,Vector2 offsetPos,Color color)
        {
            ProjectileManager.Instance.Shoot((Vector2)Owner.transform.position+offsetPos, speed, color);
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
                ActionEffect();
            }
        }

        public abstract void ActionEffect();
    }
}