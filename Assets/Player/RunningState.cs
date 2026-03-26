using TestGame.Commons.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    public class RunningState : HierarchicalState
    {
        private PlayerPenguin _player;
        public override void Begin()
        {
            base.Begin();
            _player = FindAnyObjectByType<PlayerPenguin>();
        }
        public override void Enter()
        {
            base.Enter();
            _player.sprint.action.started += StopRunning;
        }
        public override void Exit()
        {
            base.Exit();
            _player.sprint.action.started -= StopRunning;

        }
        private void StopRunning(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitioToState("WalkingState");
        }
    }
}
