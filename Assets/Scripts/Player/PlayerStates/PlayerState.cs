using Unity.VisualScripting;

namespace Player.PlayerStates
{
    public class PlayerState : StateMachine.IState
    {
        protected PlayerController PController;
        
        public PlayerState(PlayerController pController)
        {
            PController = pController;
        }

        public string GetName()
        {
            return GetType().Name;
        }

        protected bool IsCurrentAnimationFinished()
        {
            return true;
        }
        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void Update() {} 
    }
}