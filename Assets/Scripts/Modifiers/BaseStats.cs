using System;
using UnityEngine;

namespace Modifiers
{
    [Serializable]
    public class BaseStats
    {
        [Tooltip("Total clicks to kill it. Unused on Tiny Units")]
        [SerializeField] public float health;

        [Tooltip("")]
        [SerializeField] public float damageReduction;
        
        [Tooltip("Damage dealt, in clicks per attack. Unused by Enemies")]
        [SerializeField] public float attack;
        [SerializeField] public float speed;
        [SerializeField] public float abilityPower;
        [SerializeField] public float evasion;
        [SerializeField] public float criticalChance;
        [SerializeField] public float criticalDamage;
        [SerializeField] public float accuracy;

        public BaseStats Clone()
        {
            return new BaseStats
            {
                health = health,
                damageReduction = damageReduction,
                attack = attack,
                speed = speed,
                abilityPower = abilityPower,
                evasion = evasion,
                criticalChance = criticalChance,
                criticalDamage = criticalDamage,
                accuracy = accuracy
            };
        }
    }
}