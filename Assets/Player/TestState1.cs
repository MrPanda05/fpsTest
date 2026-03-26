using UnityEngine;
using TestGame.Commons.StateMachines;

namespace TestGame.Player
{
    public class TestState1 : SimpleState
    {
        public override void Begin()
        {
            print("I'm beginning");
        }
        public override void Enter()
        {
            print("Entering state" + name);
        }
        public override void Exit()
        {
            print("Exiting state" + name);
        }
    }
}
