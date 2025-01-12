using UnityEngine;
using ER.Entity2D.Agents;
using static ER.Entity2D.Agents.StateMahinceAgent;
using UnityEngine.AddressableAssets;

public class Player : StateMahcineInitializer
{
    protected override StateCell BuildStateMachine()
    {
        var idle = CreateState("idle");
        var move = CreateState("move");
        var dead = CreateState("dead");

        idle.onCheckTransition = (k) =>
        {
            return string.Empty;
        };
        return idle;

    }

}