using NUnit.Framework;
using UnityEngine;

namespace TestGame.Commons.SO
{
    [CreateAssetMenu(fileName = "Stats",menuName = "ScriptableOjbects/Stats", order = 1)]
    public class BaseStats : ScriptableObject
    {
        [field: SerializeField]
        public float MoveSpeed { get; set; } = 8f;
        [field: SerializeField]
        public float SprintSpeed { get; set; } = 12f;
        [field: SerializeField]
        public float SpeedChangeRate { get; set; } = 10f;
        [field: SerializeField]
        public float MaxStamina { get;  set; } = 100f;
        [field: SerializeField]
        public float StaminaDrainRate { get;  set; } = 15f;
        [field: SerializeField]
        public float StaminaRecoveryRate { get;  set; } = 10f;
    }
}
