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
        
        private PlayerInput _playerInput;
        public MouseInput MouseInput;
        
        public StateMachine.StateMachine StateMachine;
        public PlayerStateContainer StateContainer;
        
        public List<TinyUnitInstance> units;
        public List<AttackDetails> AttacksQueue;
        public int currency;
        
        public void AddCurrency(int amount)
        {
            currency += amount;
        }
        
        public void RemoveCurrency(int amount)
        {
            currency -= amount;
        }
        
        private void AddUnit(TinyUnitInstance unit)
        {
            units.Add(unit);
        }
        
        private void RemoveUnit(TinyUnitInstance unit)
        {
            units.Remove(unit);
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
            AttacksQueue = new List<AttackDetails>();
            units = new List<TinyUnitInstance>();
        }
    }
}