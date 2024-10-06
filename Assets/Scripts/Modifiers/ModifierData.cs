using System;

namespace Modifiers
{
    [Serializable]
    public class ModifierData
    {
        public StatusModifier modifier;
        public float chance;
        public float duration;
        public int maxStack;

        public ModifierData(StatusModifier modifier, float chance, float duration, int maxStack = 5)
        {
            this.modifier = modifier;
            this.chance = chance;
            this.duration = duration;
            this.maxStack = maxStack;
        }

        public bool ShouldApply()
        {
            return UnityEngine.Random.Range(0f, 1f) < chance;
        }

        public TemporalStatus ToTemporalStatus()
        {
            return new TemporalStatus(this);
        }
    }
}