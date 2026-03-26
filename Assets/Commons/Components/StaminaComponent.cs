using System;
using TestGame.Commons.SO;
using UnityEngine;

namespace TestGame.Commons.Components
{
    public class StaminaComponent : MonoBehaviour
    {
        [SerializeField]
        private VelocityComponent _velocityComponent;
        [SerializeField]
        private BaseStats _stats;
        public float CurrentStamina { get; private set; }

        public event Action OnStaminaDepleted;
        public event Action OnStaminaRecovered;

        private void Start()
        {
            CurrentStamina = _stats.MaxStamina;
        }

        public void SetMaxStamina(float newMaxStamina)
        {
            _stats.MaxStamina = newMaxStamina;
            if(CurrentStamina > _stats.MaxStamina)
            {
                CurrentStamina = _stats.MaxStamina;
            }
        }
        public void SetStaminaDrainRate(float newDrainRate)
        {
            _stats.StaminaDrainRate = newDrainRate;
        }
        public void SetStaminaRecoveryRate(float newRecoveryRate)
        {
            _stats.StaminaRecoveryRate = newRecoveryRate;
        }

        public void DrainStamina(float deltaTime)
        {
            CurrentStamina -= _stats.StaminaDrainRate * deltaTime;
            if(CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                OnStaminaDepleted?.Invoke();
            }
        }
        public void RecoverStamina(float deltaTime)
        {
            CurrentStamina += _stats.StaminaRecoveryRate * deltaTime;
            if(CurrentStamina >= _stats.MaxStamina)
            {
                CurrentStamina = _stats.MaxStamina;
                OnStaminaRecovered?.Invoke();
            }
        }

    }
}
