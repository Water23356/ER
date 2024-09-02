namespace ER.StateMachine
{
    public class BaseStateMachine
    {
        private IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public BaseStateMachine(IState defaultState)
        {
            currentState = defaultState;
        }
    }
}