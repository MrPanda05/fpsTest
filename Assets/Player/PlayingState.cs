using TestGame.Commons.StateMachines;
using UnityEngine;

namespace TestGame.Player
{
    public class PlayingState : HierarchicalState
    {
        private PlayerPenguin _player;
        public override void Begin()
        {
            base.Begin();
            _player = FindAnyObjectByType<PlayerPenguin>();
        }

        public override void Enter()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        public override void Exit()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
