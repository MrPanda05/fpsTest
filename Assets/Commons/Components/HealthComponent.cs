using System;
using UnityEngine;

namespace TestGame
{
    public class HealthComponent : MonoBehaviour
    {
        [field: SerializeField]
        public float CurrentHealth { get; private set; } = 100f;
        [field: SerializeField]
        public float MaxHealth { get; private set; } = 100f;

        public event Action OnDeath;
        public void TakeDamage(float dmg)
        {
            CurrentHealth -= dmg;
            if(CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeath?.Invoke();
            }
        }
    }
}
