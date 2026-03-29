using TestGame.Commons.StateMachines;
using UnityEngine;

namespace TestGame.Player.States
{
    public class PlayerState : HierarchicalState
    {
        protected PlayerPenguin _player;

        public override void Begin()
        {
            base.Begin();
            _player = FindAnyObjectByType<PlayerPenguin>();
        }
    }
}
