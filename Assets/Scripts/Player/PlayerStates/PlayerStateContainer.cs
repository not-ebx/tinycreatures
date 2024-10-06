namespace Player.PlayerStates
{
    public class PlayerStateContainer
    {
        public PlayerFightingState PlayerFightingState;
        public PlayerWalkingState PlayerWalkingState;
        
        public PlayerStateContainer(PlayerController pController)
        {
            PlayerFightingState = new PlayerFightingState(pController);
            PlayerWalkingState = new PlayerWalkingState(pController);
        }
    }
}