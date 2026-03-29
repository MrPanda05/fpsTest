using UnityEngine;

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
    }
}
