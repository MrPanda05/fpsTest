using UnityEngine;

namespace TestGame.Player.States
{
    public class WalkingState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            print(_parentState.name);
        }
    }
}
