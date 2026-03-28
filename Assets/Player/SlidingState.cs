using TestGame.Commons.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    public class SlidingState : HierarchicalState
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
            _player.slide.action.started += StopSliding;
        }
        public override void Exit()
        {
            base.Exit();
            _player.slide.action.started -= StopSliding;

        }
        private void StopSliding(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitionTo("WalkingState");
        }
    }
}
