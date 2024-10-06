using Modifiers;
using UnityEditor.Animations;
using UnityEngine;

namespace TinyUnits
{
    [CreateAssetMenu(fileName = "New Tiny Unit", menuName = "Game Items/Tiny Unit")]
    public class TinyUnit : ScriptableObject
    {
        public string unitName;
        [TextArea(3, 10)] public string description;
        [Tooltip("Type of unit, used to determine the walking distance to check for enemies")]
        [SerializeField] public TinyUnitType unitType;
        [Tooltip("Base cost, will be used to calculate upgrades")]
        public int unitBaseCost;
        [Tooltip("Unit Cost multiplier, will be used to calculate upgrades using an EXPONENTIAL function")]
        public float unitCostMultiplier = 1.15f;
        [Tooltip("Max units of the same type that can be purchased")]
        public int maxUnits = 3;
        
        [SerializeField] public Sprite unitSprite;
        [SerializeField] public Sprite iconSprite;
        [SerializeField] public EntityDetails stats;
    }
}