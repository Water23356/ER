namespace ER.StateMachine
{
    public interface IState
    {
        public void Enter();//进入该状态的初始化

        public void Update();//处于该状态的持续更新逻辑, 包含跳转到其他

        public void Exit();//离开该状态需要处理的逻辑
    }
}