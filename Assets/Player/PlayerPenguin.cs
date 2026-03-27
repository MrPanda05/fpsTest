using System;
using TestGame.Commons.Components;
using TestGame.Commons.SO;
using TestGame.Commons.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    [Serializable]
    public class PlayerContext
    {
        public bool isGrounded;
        public bool isRunning;
        public bool isSliding;
    }
    public class PlayerPenguin : MonoBehaviour
    {
        [field: SerializeField]
        public VelocityComponent VelocityComponent { get; private set; }
        [field: SerializeField]
        public StaminaComponent StaminaComponent { get; private set; }
        [field: SerializeField]
        public HStateMachine StateMachine { get; private set; }
        [SerializeField]
        private CharacterController _charController;
        [SerializeField]
        private BaseStats _stats;
        [SerializeField]
        public InputActionReference move;
        [SerializeField]
        public InputActionReference jump;
        [SerializeField]
        public InputActionReference sprint;
        [SerializeField]
        public InputActionReference slide;
        private Vector2 _input;

        private void Update()
        {
            _input = move.action.ReadValue<Vector2>();

        }

        public void MovePlayer()
        {
            Vector3 move = new Vector3(_input.x, 0, _input.y);
            _charController.Move(move * _stats.SprintSpeed * Time.fixedDeltaTime);
        }
    }
}
