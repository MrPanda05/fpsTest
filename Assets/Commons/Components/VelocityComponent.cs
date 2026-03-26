using TestGame.Commons.SO;
using UnityEngine;

namespace TestGame.Commons.Components
{
    /// <summary>
    /// Controls how an non physics object move
    /// </summary>
    public class VelocityComponent : MonoBehaviour
    {
        [SerializeField]
        private BaseStats _stats;

        [field: SerializeField]
        public float Gravity { get; private set; } = -40f;
        public Vector3 Velocity { get; private set; }

        public void SetMoveSpeed(float newSpeed)
        {
            _stats.MoveSpeed = newSpeed;
        }
        public void SetSprintSpeed(float newSpeed)
        {
            _stats.SprintSpeed = newSpeed;
        }
        public void SetSpeedChangeRate(float newRate)
        {
            _stats.SpeedChangeRate = newRate;
        }
        public void SetGravity(float newGravity)
        {
            Gravity = newGravity;
        }
    }
}
