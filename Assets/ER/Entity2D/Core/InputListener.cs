namespace ER.Entity2D
{
    public abstract class InputListener : ControllerBase
    {
        public override void Init()
        {
            if (enabled)
            {
                UnregisterEvent();
                RegisterEvent();
            }
        }

        public override void OnDestroy()
        {
            UnregisterEvent();
        }

        public override void Start()
        {
            UnregisterEvent();
            RegisterEvent();
        }

        protected abstract void RegisterEvent();

        protected abstract void UnregisterEvent();
    }
}