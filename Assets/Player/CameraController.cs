using Unity.Cinemachine;
using UnityEngine;

namespace TestGame.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera _camera;
        [SerializeField]
        private float _mouseSensitivity = 1f;
        [SerializeField]
        private Transform _pitchAnchor;
        private float _pitch;
        private float _yaw;
        private PlayerPenguin _player;
        private void Awake()
        {
            _player = GetComponentInParent<PlayerPenguin>();
            if(_player == null)
            {
                Debug.LogError("CameraController could not find PlayerPenguin in parent hierarchy.");
            }
        }
        public void SetSensitivity(float newSens)
        {
            _mouseSensitivity = newSens;
        }
        public void MoveCamera(Vector2 mouseInput)
        {
            _pitch -= mouseInput.y * _mouseSensitivity;
            _yaw += mouseInput.x * _mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, -90f, 90f);
            _player.transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            _pitchAnchor.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

        }
    }
}
