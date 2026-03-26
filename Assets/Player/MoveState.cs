using TestGame.Commons.StateMachines;
using UnityEngine;

namespace TestGame.Player
{
    public class MoveState : HierarchicalState
    {
        PlayerPenguin _player;
        public override void Begin()
        {
            base.Begin();
            _player = FindAnyObjectByType<PlayerPenguin>();
        }
        public override void FixProcess()
        {
            _player.MovePlayer();
        }
    }
}
