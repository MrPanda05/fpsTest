using TestGame.Commons.StateMachines;
using TestGame.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame
{
    public class WalkingState : HierarchicalState
    {
        private PlayerPenguin _player;
        public override void Begin()
        {
            base.Begin();
            _player = FindAnyObjectByType<PlayerPenguin>();
        }
        public override void Enter()
        {
            _player.sprint.action.started += StartRunning;
            _player.slide.action.started += StartSliding;
            base.Enter();
        }
        public override void Exit()
        {
            _player.sprint.action.started -= StartRunning;
            _player.slide.action.started -= StartSliding;
            base.Exit();

        }
        private void StartSliding(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitioToState("SlidingState");
        }
        private void StartRunning(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitioToState("RunningState");
        }
      
    }
}
