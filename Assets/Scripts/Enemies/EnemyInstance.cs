using System;
using System.Collections.Generic;
using Modifiers;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class EnemyInstance : MonoBehaviour
    {
        public Enemy Enemy { get; set; }
        public float CurrentHealth { get; set; }
        public List<TemporalStatus> TemporalStats { get; set; }
        
        public void Initialize(Enemy enemy)
        {
            Enemy = enemy;
            CurrentHealth = enemy.stats.health;
            TemporalStats = new List<TemporalStatus>();
        }
        
        public int CalculateReward(int stageLevel)
        {
            // TODO need to add a better way to calculate this i guess.
            var rewardMultiplier = 1f;
            switch (Enemy.enemyType)
            {
                case EnemyType.Normal:
                    rewardMultiplier = 1.1f;
                    break;
                case EnemyType.Elite:
                    rewardMultiplier = 1.5f;
                    break;
                case EnemyType.Boss:
                    rewardMultiplier = 2f;
                    break;
            }
            
            return (int)(Enemy.baseReward * Mathf.Pow(rewardMultiplier, stageLevel));
        }
        
        
    }
}