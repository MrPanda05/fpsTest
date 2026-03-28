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
            _player.sprint.action.started += Test;
        }
        public override void Exit()
        {
            _player.jump.action.started -= TestDeath;
            _player.sprint.action.started -= Test;
            base.Exit();
        }
        private void TestDeath(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitionTo("SimpleState");
        }
        private void Test(InputAction.CallbackContext obj)
        {
            _myStateMachine.TransitionTo("SubState");
        }
        public override void FixProcess()
        {
            _player.MovePlayer();
        }
    }
}
