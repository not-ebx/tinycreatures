using System;
using UnityEngine;

namespace Modifiers
{
    [Serializable]
    public class StatsModifier
    {
        // Stats that it increases
        // Values go from 0 to infinity, so 1 increase is 1.
        [SerializeField] public float incHealth;
        [SerializeField] public float incMana;
        [SerializeField] public float incDamageReduction;
        [SerializeField] public float incAttack;
        [SerializeField] public float incSpeed;
        [SerializeField] public float incAbilityPower;
        [SerializeField] public float incEvasion;
        [SerializeField] public float incCriticalChance;
        [SerializeField] public float incCriticalDamage;
        [SerializeField] public float incAccuracy;
        
        // Stat that it increases but by percent. incPercent...
        // Values go from 0 to 1, so 0.1 is 10%, for example.
        [SerializeField] public float incPercentHealth;
        [SerializeField] public float incPercentMana;
        [SerializeField] public float incPercentDamageReduction;
        [SerializeField] public float incPercentAttack;
        [SerializeField] public float incPercentSpeed;
        [SerializeField] public float incPercentAbilityPower;
        [SerializeField] public float incPercentEvasion;
        [SerializeField] public float incPercentCriticalChance;
        [SerializeField] public float incPercentCriticalDamage;
        [SerializeField] public float incPercentAccuracy;
        
        // Default constructor, setting everything to 0
        public StatsModifier()
        {
            incHealth = 0;
            incMana = 0;
            incDamageReduction = 0;
            incAttack = 0;
            incSpeed = 0;
            incAbilityPower = 0;
            incEvasion = 0;
            incCriticalChance = 0;
            incCriticalDamage = 0;
        }
    } 
}