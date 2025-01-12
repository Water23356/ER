
using ER.Entity2D.Agents;
using ER.Entity2D.Components;
using static ER.Entity2D.Agents.StateMahinceAgent;

namespace ER.Entity2D.Sample
{
    public class SMNormal1 : StateMahcineInitializer
    {
        protected override StateCell BuildStateMachine()
        {
            var act_move = actAgent.Get<AMovePlane>();


            var idle = new StateCell()
            {
                stateName = "idle",
                onCheckEntry = () => true,
                onEntry = last => { },
                onExit = next => { },
                onCheckTransition = trigger => 
                {
                    return string.Empty;
                },
                onChanged = step => { },
            };

            var moveLayer = new StateLayer()
            {
                stateName = "move",
                onDisable = () => 
                {
                    if(act_move.CanEntry())
                        act_move.Entry();
                },
                onEnable = () => 
                {
                    act_move.Exit();
                },
                checkEnable = () => 
                {
                    return stateMachine.GetParamState("move");
                },
                checkDisable = () =>
                {
                    return !stateMachine.GetParamState("move");
                }
            };

            return idle;
        }
    }
}
