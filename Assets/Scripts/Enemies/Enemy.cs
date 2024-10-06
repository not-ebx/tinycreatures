using Modifiers;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "New Ememy", menuName = "Game Items/Enemy")]
    public class Enemy : ScriptableObject
    {
        public string enemyName;
        [TextArea(3, 10)] public string description;
        public EnemyType enemyType;
        public int baseReward = 100;
        
        [SerializeField] public Sprite enemySprite;
        [SerializeField] public EntityDetails stats;
    }
}