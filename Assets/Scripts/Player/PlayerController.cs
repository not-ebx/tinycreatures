using System;
using System.Collections.Generic;
using Modifiers;
using Player.PlayerStates;
using TinyUnits;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [Serializable]
    public class PlayerController: MonoBehaviour
    {
        [NonSerialized] public Rigidbody2D Rb;
        [NonSerialized] public BoxCollider2D Coll;
        [NonSerialized] public Animator Anim;
        [NonSerialized] public SpriteRenderer Sprite;
        [NonSerialized] public Camera MainCamera;
        
        private PlayerInput _playerInput;
        public PlayerInputActions PlayerInputActions;
        public StateMachine.StateMachine StateMachine;
        public PlayerStateContainer StateContainer;
        
        public List<TinyUnitInstance> units = new();
        public List<AttackDetails> AttacksQueue = new();
        public int currency = 0;
        public int clickDamage = 25;
        
        public void AddCurrency(int amount)
        {
            currency += amount;
        }
        
        public void RemoveCurrency(int amount)
        {
            currency -= amount;
        }

        public Vector3 GetCurrentEnemyPosition()
        {
            // Find current enemy by Tag "CurrentEnemy"
            var enemy = GameObject.FindGameObjectWithTag("CurrentEnemy");
            return enemy.transform.position;
        }

        public GameObject GetCurrentEnemyObject()
        {
            return GameObject.FindGameObjectWithTag("CurrentEnemy");
        }
        
        public void AddUnit(TinyUnit unit)
        {
            var prefab = Resources.Load<TinyUnitInstance>("Prefabs/TinyUnitPrefab");
            var instance = Instantiate(
                prefab,
                Vector3.zero,
                Quaternion.identity
            ).GetComponent<TinyUnitInstance>();
            instance.playerController = this;
            instance.Initialize(unit);
            instance.gameObject.SetActive(true);
            units.Add(instance);
        }
        
        private void RemoveUnit(TinyUnitInstance unit)
        {
            units.Remove(unit);
        }

        public void ClickAttack()
        {
            
        }

        // Mono Behaviour methods
        public void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            Coll = GetComponent<BoxCollider2D>();
            Anim = GetComponent<Animator>();
            Sprite = GetComponent<SpriteRenderer>();
            
            StateMachine = new StateMachine.StateMachine();
            StateContainer = new PlayerStateContainer(this);
            PlayerInputActions = new PlayerInputActions();
            MainCamera = Camera.main;
            
            StateMachine.ChangeState(StateContainer.PlayerFightingState);
        }
        
        private void OnEnable()
        {
            PlayerInputActions.Fighting.Enable();
        }

        private void OnDisable()
        {
            PlayerInputActions.Fighting.Disable();
        }
    }
}