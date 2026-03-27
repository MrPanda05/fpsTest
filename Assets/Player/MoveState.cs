using TestGame.Commons.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    public class MoveState : HierarchicalState
    {
        PlayerPenguin _player;
        public override void Begin()
        {
            base.Begin();
            _player = FindAnyObjectByType<PlayerPenguin>();
        }
        public override void Enter()
        {
            base.Enter();
            _player.jump.action.started += TestDeath;
        }
        public override void Exit()
        {
            _player.jump.action.started -= TestDeath;
            base.Exit();
        }
        private void TestDeath(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitioToState("DeathState");
        }
        public override void FixProcess()
        {
            _player.MovePlayer();
        }
    }
}
