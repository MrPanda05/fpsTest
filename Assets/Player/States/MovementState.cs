using UnityEngine;

namespace TestGame.Player.States
{
    public class MovementState : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            _player.InputActions.FindActionMap("Player").Enable();
        }
        public override void Exit()
        {
            _player.InputActions.FindActionMap("Player").Disable();
            base.Exit();
        }
        public override void FixProcess()
        {
            _player.MovePlayer();
        }
    }
}
