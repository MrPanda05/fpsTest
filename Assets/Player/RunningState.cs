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
            _player.sprint.action.started += StopRunning;
            base.Enter();
        }
        public override void Exit()
        {
            _player.sprint.action.started -= StopRunning;
            base.Exit();

        }
        private void StopRunning(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitioToState("WalkingState");
        }
    }
}
