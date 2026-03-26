using TestGame.Commons.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    public class GroundState : HierarchicalState
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
            _player.jump.action.started += jump;
        }
        public override void Exit()
        {
            base.Exit();
            _player.jump.action.started -= jump;
        }
        public void jump(InputAction.CallbackContext obj)
        {
            _parentStateMachine.TransitioToState("AerialState");
        }
    }
}
