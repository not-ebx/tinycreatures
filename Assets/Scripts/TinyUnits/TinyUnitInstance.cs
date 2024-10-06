using System;
using Enemies;
using Modifiers;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TinyUnits
{
    
    public class TinyUnitInstance : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Attacking,
            Returning
        }
        
        private State currentState = State.Idle; // Estado actual del ratón
        
        public PlayerController playerController;
        private Vector3 _initialPosition;
        
        [NonSerialized] public BoxCollider2D Coll;
        [NonSerialized] public SpriteRenderer Sprite;
        [NonSerialized] public Rigidbody2D Rb;
        
        public TinyUnit Unit { get; set; }
        public int level = 1;
        public TemporalStatsManager temporalStats;
        public BaseStats currentStats;
        public GameObject target;

        public float GetMinimumDistance()
        {
            switch (Unit.unitType)
            {
                case TinyUnitType.Melee:
                    return 4f;
                case TinyUnitType.Range:
                    return 10f;
                default:
                    return 4f;
            }
        }
        
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

        /*private void Attack()
        {
            var attack = CalculateDamage();
            // Get PiperPlayer instance
            playerController.AttacksQueue.Add(attack);
        }*/

        public void Initialize(TinyUnit unit)
        {
            Unit = unit;
            // Set sprite as TinyUnit's sprite
            Sprite.sprite = Unit.unitSprite;
                        
            // Find game object named "RatSpawnPoint"
            var collider = GameObject.Find("RatSpawnPoint").GetComponent<BoxCollider2D>();
            // Get a random position inside of the collider, usind Random
            transform.position = collider.bounds.center + new Vector3(
                Random.Range(-collider.bounds.size.x / 2, collider.bounds.size.x / 2),
                Random.Range(-collider.bounds.size.y / 2, collider.bounds.size.y / 2),
                0
            );
            
            _initialPosition = new Vector2(
                transform.position.x,
                transform.position.y
            );
        }
        
        // MonoBehaviour methods
        private void Awake()
        {
            temporalStats = new TemporalStatsManager();
            Coll = GetComponent<BoxCollider2D>();
            //Anim = GetComponent<Animator>();
            Sprite = GetComponent<SpriteRenderer>();
            Rb = GetComponent<Rigidbody2D>();

        }
        
        void FindTarget()
        {
            target = playerController.GetCurrentEnemyObject();
            if (target != null)
            {
                currentState = State.Attacking;
            }
        }

        void MoveTowardsTarget()
        {
            if (target == null) // Si no hay objetivo, regresar
            {
                currentState = State.Returning; 
                return; // Salir de la función
            }

            // Mover el ratón hacia el monstruo, restringiendo el movimiento al eje X
            Vector2 direction = new Vector2(
                target.transform.position.x - transform.position.x,
                target.transform.position.y - transform.position.y
            ).normalized;
            Rb.velocity = new Vector2(direction.x * currentStats.speed, direction.y * currentStats.speed); // Usar Rigidbody2D para mover el ratón, manteniendo Y igual

            // Verificar si ha alcanzado al monstruo
            if (Vector3.Distance(transform.position, target.transform.position) < GetMinimumDistance())
            {
                Attack();
            }
        }

        void Attack()
        {
            // Aplicar daño al monstruo
            AttackDetails attack = CalculateDamage();
            var enemy = target.GetComponent<EnemyInstance>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)attack.Damage); // Método para aplicar daño
            }

            // Iniciar el regreso
            Rb.velocity = Vector2.zero; // Detener el ratón al atacar
            currentState = State.Returning; // Cambiar al estado de regreso
        }
        
        void ReturnToStart()
        {
            // Regresar a la posición inicial
            Vector2 direction = (_initialPosition - transform.position).normalized;
            Rb.velocity = direction * currentStats.speed; // Usar Rigidbody2D para regresar

            // Verificar si ha vuelto a la posición original
            if (Vector3.Distance(transform.position, _initialPosition) < 0.1f)
            {
                Rb.velocity = Vector2.zero; // Detener el ratón al regresar
                target = null; // Reiniciar el objetivo
                currentState = State.Idle; // Regresar al estado de espera
            }
        }
        
        private void Update()
        {
            temporalStats.RemoveExpiredStats();
            currentStats = GetStats();
            
            switch (currentState)
            {
                case State.Idle:
                    FindTarget();
                    break;
    
                case State.Attacking:
                    MoveTowardsTarget();
                    break;
    
                case State.Returning:
                    ReturnToStart();
                    break;
            }
        }
    }
}