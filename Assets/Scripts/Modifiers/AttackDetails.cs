using System.Collections.Generic;

namespace Modifiers
{
    public class AttackDetails
    {
        public float Damage;
        public List<TemporalStatus> SelfInflictingModifiers;
        public List<TemporalStatus> EnemyInflictingModifiers;
        public List<TemporalStatus> AllyInflictingModifiers;
        
        public AttackDetails(float damage, List<TemporalStatus> selfInflictingModifiers, List<TemporalStatus> enemyInflictingModifiers, List<TemporalStatus> allyInflictingModifiers)
        {
            Damage = damage;
            SelfInflictingModifiers = selfInflictingModifiers;
            EnemyInflictingModifiers = enemyInflictingModifiers;
            AllyInflictingModifiers = allyInflictingModifiers;
        }
    }
}