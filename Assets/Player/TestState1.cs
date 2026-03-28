using TestGame.Commons.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    public class TestState1 : HierarchicalState
    {
        PlayerPenguin _player;
        public string texto;
        public override void Begin()
        {
            base.Begin();
            _player = FindAnyObjectByType<PlayerPenguin>();
        }
        public override void Enter()
        {
            base.Enter();
            _player.slide.action.started += TestMove;
        }
        public override void Exit()
        {
            _player.slide.action.started -= TestMove;
            base.Exit();
        }
        private void TestMove(InputAction.CallbackContext obj)
        {
            print(texto);
        }
        public override void FixProcess()
        {
        }
    }
}
