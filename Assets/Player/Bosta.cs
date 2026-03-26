using TestGame.Commons.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestGame.Player
{
    public class Bosta : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _charController;
        [SerializeField]
        private float _speed = 5f;//change this
        [SerializeField]
        private InputActionReference move;
        [SerializeField]
        private BaseStats _stats;

        private Vector2 _input;

        private void Update()
        {
            _input = move.action.ReadValue<Vector2>();

        }

        private void FixedUpdate()
        {
            Vector3 move = new Vector3(_input.x, 0, _input.y);
            move = Vector3.RotateTowards(transform.forward, move, Mathf.PI, 0);
            _charController.Move(move * _speed * Time.fixedDeltaTime);
        }
    }
}
