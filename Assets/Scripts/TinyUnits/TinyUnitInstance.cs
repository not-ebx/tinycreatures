using System;
using System.Collections.Generic;
using Modifiers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TinyUnits
{
    
    public class TinyUnitInstance : MonoBehaviour
    {
        private readonly float _chargeSpeedMultiplier = 0.5f;
        
        [NonSerialized] public Rigidbody2D Rb;
        [NonSerialized] public BoxCollider2D Coll;
        [NonSerialized] public Animator Anim;
        [NonSerialized] public SpriteRenderer Sprite;
        
        public TinyUnit Unit { get; set; }
        public int level;
        public float charge = 0.0f;
        public TemporalStatsManager temporalStats;
        public BaseStats currentStats;
        
        public int CalculateUpgradeCost()
        {
            return (int)(Unit.unitBaseCost * Mathf.Pow(Unit.unitCostMultiplier, level));
        }
        
        public int CalculateSellValue()
        {
            return (int)(Unit.unitBaseCost * Mathf.Pow(Unit.unitCostMultiplier, level - 1) * 0.5f);
        }
        
        private BaseStats GetStats()
        {
            return temporalStats.GetCalculatedStats(Unit.stats.Clone());
        }
        
        public void ResetUnit()
        {
            temporalStats.ClearAllStats();
        }
        
        public AttackDetails CalculateDamage()
        {
            var (
                selfModifiers,
                enemyModifiers,
                allyModifiers
            ) = Unit.stats.GetAndCalculateChanceForAllModifiers();

            var stats = currentStats;
            var damage = stats.attack;
            damage *= Random.Range(0.9f, 1.1f);

            if (Random.value < stats.criticalChance)
            {
                damage *= (stats.criticalDamage / 100);
            }
            
            return new AttackDetails(damage, selfModifiers, enemyModifiers, allyModifiers);
        }

        private void Attack()
        {
            var attack = CalculateDamage();
            // Get PiperPlayer instance
            var player = FindObjectOfType<Player.PlayerController>();
            player.AttacksQueue.Add(attack);
        }
        
        // MonoBehaviour methods
        private void Awake()
        {
            temporalStats = new TemporalStatsManager();
            Rb = GetComponent<Rigidbody2D>();
            Coll = GetComponent<BoxCollider2D>();
            Anim = GetComponent<Animator>();
            Sprite = GetComponent<SpriteRenderer>();
            // Set sprite as TinyUnit's sprite
            Sprite.sprite = Unit.unitSprite;
        }
        
        private void Update()
        {
            currentStats = GetStats();
            temporalStats.RemoveExpiredStats();
            
            // Calculate charging
            charge += Time.deltaTime * currentStats.speed * _chargeSpeedMultiplier;
            if (!(charge >= 1.0f)) return;
            Attack();
            charge = 0;
        }
    }
}