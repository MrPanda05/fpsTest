using TestGame.Commons.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    public class DeathState : HierarchicalState
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
            _player.jump.action.started += TestMove;
        }
        public override void Exit()
        {
            _player.jump.action.started -= TestMove;
            base.Exit();
        }
        private void TestMove(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitioToState("MovementState");
        }
        public override void FixProcess()
        {
            _player.transform.position += new Vector3(0,_player.move.action.ReadValue<Vector2>().x,0);
        }
    }
}
