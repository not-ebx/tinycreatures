using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modifiers
{
    [Serializable]
    public class TemporalStatsManager
    {
        public List<TemporalStatus> TemporalStats { get; private set; }
        
        public TemporalStatsManager()
        {
            TemporalStats = new List<TemporalStatus>();
        }
        
        public void AddTemporalStat(TemporalStatus stat)
        {
            TemporalStats.Add(stat);
        }
        
        public void RemoveTemporalStatByType(StatusModifier type)
        {
            TemporalStats.RemoveAll(stat => stat.Source.modifier == type);
        }
        
        public void RemoveExpiredStats()
        {
            TemporalStats.RemoveAll(stat => stat.IsExpired );
        }
        
        public void ClearAllStats()
        {
            TemporalStats.Clear();
        }
        
        public BaseStats GetCalculatedStats(BaseStats baseStats)
        { 
            var modifiedStats = baseStats.Clone();
            foreach (var stat in TemporalStats)
            {
                // Add the stats from the temporal stat incStat, including the percentual add
                // Remember that the attributes are named incStat, incPercentStat, etc.
                modifiedStats.health += stat.incHealth + (modifiedStats.health * stat.incPercentHealth);
                modifiedStats.damageReduction += stat.incDamageReduction +
                                                 (modifiedStats.damageReduction * stat.incPercentDamageReduction);
                modifiedStats.attack += stat.incAttack + (modifiedStats.attack * stat.incPercentAttack);
                modifiedStats.speed += stat.incSpeed + (modifiedStats.speed * stat.incPercentSpeed);
                modifiedStats.abilityPower +=
                    stat.incAbilityPower + (modifiedStats.abilityPower * stat.incPercentAbilityPower);
                modifiedStats.evasion += stat.incEvasion + (modifiedStats.evasion * stat.incPercentEvasion);
                modifiedStats.criticalChance += stat.incCriticalChance +
                                                (modifiedStats.criticalChance * stat.incPercentCriticalChance);
                modifiedStats.criticalDamage += stat.incCriticalDamage +
                                                (modifiedStats.criticalDamage * stat.incPercentCriticalDamage);
                modifiedStats.accuracy += stat.incAccuracy + (modifiedStats.accuracy * stat.incPercentAccuracy);
            }

            // Next, we take all the modifiedStats, and use a Math.max(0, value) to avoid negative numbers
            modifiedStats.health = Mathf.Max(0, modifiedStats.health);
            modifiedStats.damageReduction = Mathf.Max(0, modifiedStats.damageReduction);
            modifiedStats.attack = Mathf.Max(0, modifiedStats.attack);
            modifiedStats.speed = Mathf.Max(0, modifiedStats.speed);
            modifiedStats.abilityPower = Mathf.Max(0, modifiedStats.abilityPower);
            modifiedStats.evasion = Mathf.Max(0, modifiedStats.evasion);
            modifiedStats.criticalChance = Mathf.Max(0, modifiedStats.criticalChance);
            modifiedStats.criticalDamage = Mathf.Max(0, modifiedStats.criticalDamage);
            modifiedStats.accuracy = Mathf.Max(0, modifiedStats.accuracy);

            return modifiedStats; 
        }
    }
}