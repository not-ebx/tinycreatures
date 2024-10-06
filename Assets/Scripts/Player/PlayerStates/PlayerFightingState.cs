using Enemies;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerStates
{
    public class PlayerFightingState : PlayerState
    {
        private InputAction _attackAction;
        private LayerMask _enemyLayer;
        
        public PlayerFightingState(PlayerController pController) : base(pController)
        {
            _enemyLayer = LayerMask.GetMask("Enemy");
        }

        public override void Enter()
        {
            _attackAction = PController.PlayerInputActions.Fighting.Attack;
            _attackAction.performed += OnMouseClick;
            
            Debug.Log("Entered fighting state.");
        }

        public override void Exit()
        {
            _attackAction.performed -= OnMouseClick;
            
            Debug.Log("Exited fighting state.");
        }

        private void OnMouseClick(InputAction.CallbackContext context)
        {
            // Perform a raycast to see if the player clicked on an enemy
            Debug.Log("OnMouseClick");
            //Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 mousePosition = PController.MainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );
            var ray = PController.MainCamera.ScreenPointToRay(mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f); // Draw the ray
            Debug.Log(mousePosition);
            
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, _enemyLayer);

            if (hit.collider == null) return;
            Debug.Log("Detected");
            var clickedObject = hit.collider.gameObject;

            if (!clickedObject.CompareTag("CurrentEnemy")) return;
            // Get the enemy instance and apply damage
            var enemyInstance = clickedObject.GetComponent<EnemyInstance>();
            enemyInstance.TakeDamage(PController.clickDamage);
        }
    }
}