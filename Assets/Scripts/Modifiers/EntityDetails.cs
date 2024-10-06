using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modifiers
{
    [Serializable]
    public class EntityDetails : BaseStats
    {
        [SerializeField] private List<ModifierData> selfInflictingModifiers = new List<ModifierData>();
        [SerializeField] private List<ModifierData> enemyInflictingModifiers = new List<ModifierData>();
        [SerializeField] private List<ModifierData> allyInflictingModifiers = new List<ModifierData>();
        
        public (List<TemporalStatus>, List<TemporalStatus>, List<TemporalStatus>) GetAndCalculateChanceForAllModifiers()
        {
            return (
                selfInflictingModifiers.Where(mod => mod.ShouldApply()).Select(mod => mod.ToTemporalStatus()).ToList(),
                enemyInflictingModifiers.Where(mod => mod.ShouldApply()).Select(mod => mod.ToTemporalStatus()).ToList(),
                allyInflictingModifiers.Where(mod => mod.ShouldApply()).Select(mod => mod.ToTemporalStatus()).ToList()
            );
        }
        
    }
}