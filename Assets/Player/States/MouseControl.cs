using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player.States
{
    public class MouseControl : PlayerState
    {
        private Vector2 _mouseInput;
        private InputAction _lookAction;
        public override void Begin()
        {
            base.Begin();
            _lookAction = _player.InputActions.FindAction("Look");
        }
        public override void Enter()
        {
            base.Enter();
            Cursor.lockState = CursorLockMode.Locked;
        }
        public override void Exit()
        {
            Cursor.lockState = CursorLockMode.None;
            base.Exit();
        }
        public override void FixProcess()
        {
            _mouseInput = _lookAction.ReadValue<Vector2>();
            _player.CameraController.MoveCamera(_mouseInput);
        }
    }
}
