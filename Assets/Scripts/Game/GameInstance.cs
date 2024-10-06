using System;
using System.Collections.Generic;
using Enemies;
using Player;
using TinyUnits;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class GameInstance : MonoBehaviour
    {
        public EnemyInstance CurrentEnemy { get; set; }
        public PlayerController playerController;
        public GameObject enemyPrefab;
        
        public List<TinyUnit> tinyUnits;
        public List<Enemy> enemies;

        private void Awake()
        {
            tinyUnits = new List<TinyUnit>(Resources.LoadAll<TinyUnit>("GameObjects/TinyUnits"));
            enemies = new List<Enemy>(Resources.LoadAll<Enemy>("GameObjects/Enemies"));
            
            // Get a random enemy from the list of enemies
            var testEnemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];
            
            // Get a random tiny unit
            var testTinyUnit = tinyUnits[UnityEngine.Random.Range(0, tinyUnits.Count)];
            
            enemyPrefab = Resources.Load<GameObject>("Prefabs/EnemyPrefab");
            // Instantiate EnemyPrefab from Prefabs/Enemies/EnemyPrefab on scene root
            CurrentEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity).GetComponent<EnemyInstance>();
            CurrentEnemy.Initialize(testEnemy);
            CurrentEnemy.tag = "CurrentEnemy";

            
            playerController.AddUnit(testTinyUnit);
            playerController.AddUnit(tinyUnits[UnityEngine.Random.Range(0, tinyUnits.Count)]);
            playerController.AddUnit(tinyUnits[UnityEngine.Random.Range(0, tinyUnits.Count)]);
            playerController.AddUnit(tinyUnits[UnityEngine.Random.Range(0, tinyUnits.Count)]);
        }
    }
}