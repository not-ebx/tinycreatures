using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Modifiers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies
{
    [Serializable]
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] public Enemy Enemy;
        public float currentHealth;
        public List<TemporalStatus> TemporalStats { get; set; }

        [NonSerialized] public GameObject DamageTextPrefab;
        public Transform canvasTransform;
        public Animator animator;
        public AnimatorOverrideController overrideController;
        
        private int damageTextOffset = 50;
        private int damageTextCount = 0;
        public Color hitColor = Color.red;
        public float hitColorDuration = 0.1f; 
        public float shakeDuration = 0.1f; 
        public float shakeMagnitude = 0.1f;

        public SpriteRenderer SpriteRenderer { get; set; }
        public BoxCollider2D Coll { get; set; }
        
        public void Initialize(Enemy enemy)
        {
            DamageTextPrefab = Resources.Load<GameObject>("Prefabs/DamageText"); 
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            gameObject.tag = "Enemy";
            // Find the game object with the name "MobSpawnPoint" and get its collider
            BoxCollider2D collider = GameObject.Find("MobSpawnPoint").GetComponent<BoxCollider2D>();
            // Get the position of the collider and set the position of the enemy to that position
            transform.position = collider.bounds.center;
            
            Enemy = enemy;
            // Set the collider as the size of the sprite, but with a little offset
            Coll = gameObject.AddComponent<BoxCollider2D>();
            // Set the sprite renderer to the sprit
            SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            SpriteRenderer.sprite = enemy.enemySprite;
            
            Coll.size = new Vector2(
                SpriteRenderer.bounds.size.x,
                SpriteRenderer.bounds.size.y
            );
            Coll.transform.position = transform.position;
            
            SetAnimations();
            
            currentHealth = enemy.stats.health;
            TemporalStats = new List<TemporalStatus>();
        }

        private void SetAnimations()
        {
            // Set animations
            animator = GetComponent<Animator>();

            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;
            
            overrideController["Idle"] = Enemy.animations.idle;
            overrideController["Die"] = Enemy.animations.die;

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
        
        public void TakeDamage(int damage)
        {
            currentHealth -= damage; // Reducir la salud del monstruo
            ShowDamageText(damage); // Mostrar el texto de daño
            AudioManager.Instance.PlaySfx(GetDamageSound());
            StartCoroutine(ChangeColorOnHit(hitColor, hitColorDuration));
            StartCoroutine(ShakeMonster());
    
            if (currentHealth <= 0)
            {
                AudioManager.Instance.PlaySfx(GetDeathSound());
                animator.SetTrigger("Die");
                Destroy(gameObject, 0.5f);
            }
        }

        public void TakeDamage(AttackDetails att)
        {
            TakeDamage((int)att.Damage);
            if (att.EnemyInflictingModifiers.Count <= 0) return;
            foreach (var modifier in att.EnemyInflictingModifiers)
            {
                TemporalStats.Add(modifier);
            }
        }
        
        public void ShowDamageText(int damage)
        {
            if (DamageTextPrefab != null && canvasTransform != null)
            {
                GameObject damageTextInstance = Instantiate(DamageTextPrefab, canvasTransform);
                TextMeshProUGUI tmp = damageTextInstance.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.text = "-" + damage.ToString();
                    tmp.fontSize = 50f;
                }
    
                Vector3 worldPosition = transform.position + new Vector3(0.5f, 2.3f, 0);
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                Vector3 offsetPosition = new Vector3(screenPosition.x, screenPosition.y + damageTextOffset * damageTextCount, screenPosition.z);
                damageTextInstance.transform.position = offsetPosition;
    
                damageTextCount++;
    
                Destroy(damageTextInstance, 1.0f);
    
                if (damageTextCount > 5)
                {
                    damageTextCount = 0;
                }
            }
        }
       
        public IEnumerator ChangeColorOnHit(Color color, float duration)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                Color originalColor = renderer.material.color;
                renderer.material.color = color;
                yield return new WaitForSeconds(duration);
                renderer.material.color = originalColor;
            }
        }

        private IEnumerator ShakeMonster()
        {
            Vector3 originalPosition = transform.position;

            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
                float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);

                transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = originalPosition; 
        } 
        private AudioClip GetDeathSound()
        {
            //return Enemy?.deathSound ?? Resources.Load<AudioClip>("Sounds/destroy");
            return Resources.Load<AudioClip>("Sounds/destroy");
        }
        
        private AudioClip GetDamageSound()
        {
            //return Enemy?.damageSound ??  Resources.Load<AudioClip>("Sounds/damage");
            return Resources.Load<AudioClip>("Sounds/damage");
        }
        
    }
}