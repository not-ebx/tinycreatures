using UnityEngine;
using Modifiers;

namespace Modifiers
{
    public class TemporalStatus : StatsModifier
    {
        // Holds the reference of the modifier that caused this status
        public ModifierData Source;
        public float DotDamage { get; set; }
        public StatusModifier DotType { get; set; }
        public float Duration;
        public float LastDotTime { get; set; }
        public float ReflectPercentage { get; set; }

        public bool IsExpired => Time.time >= LastDotTime + Duration;
        
        public TemporalStatus(ModifierData modifierData)
        {
            Source = modifierData;
            LastDotTime = Time.time;
            
            // switch with the StatusModifier enum
            switch (modifierData.modifier)
            {
                case StatusModifier.Slow:
                    // Temporal stat with incPercentSpeed -50%
                    incPercentSpeed = -0.2f;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Stun:
                    incSpeed = -10000;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Freeze:
                    incSpeed = -10000;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Paralyze:
                    incSpeed = -10000;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Sleep:
                    incSpeed = -10000;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Silence:
                    break;
                case StatusModifier.Blind:
                    incPercentAccuracy = -0.5f;
                    Duration = modifierData.duration;
                    break;
                // TODO: DoTs
                case StatusModifier.Poison:
                    DotDamage = 0.3f; 
                    DotType = StatusModifier.Poison;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Burn: 
                    DotDamage = 0.6f;
                    DotType = StatusModifier.Burn;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Bleed:
                    DotDamage = 0.4f;
                    DotType = StatusModifier.Bleed;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Taunt:
                    // Increases Attack a bit, but decreases its Defense
                    incAttack = 5;
                    incDamageReduction = -10;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Weakness:
                    // Decreases Attack by 50%
                    incPercentAttack = -0.5f;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Vulnerability:
                    // Decreases Defense by 50%
                    incPercentDamageReduction = -0.5f;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Fear:
                    incPercentAccuracy = -0.25f;
                    incPercentDamageReduction = -0.10f;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.ReflectDamage:
                    ReflectPercentage = 0.3f; // daño que reflejas 30%
                    Duration = modifierData.duration;
                    break;
                default:
                    break;
            }
        }
    }
}