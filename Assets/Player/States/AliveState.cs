using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player.States
{
    public class AliveState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            _player.HealthComponent.OnDeath += Die;
        }
        public override void Exit()
        {
            _player.HealthComponent.OnDeath -= Die;
            base.Exit();
        }
        private void Die()
        {
            _parentStateMachine.TransitionTo("DeathState");
        }
        public override void FixProcess()
        {
            //_player.InputActions.FindAction("Jump").performed += (InputAction.CallbackContext ojb) => print("jump");
        }
    }
}
