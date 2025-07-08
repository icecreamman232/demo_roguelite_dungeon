
namespace SGGames.Script.Core
{
    public static partial class Global
    {
        public static readonly float HP_PER_SLOT = 50f;
        
        public enum MovementType
        {
            Normal,
            KnockBack,
            Stop,
        }

        public enum MovementBehaviorType
        {
            Normal,
            TowardPoint,
            FollowingTarget,
        }
        
        public enum HealthSlotType
        {
            Health,
            HealthDisable,
            Shield,
            ShieldDisable,
        }

        public enum RoomType
        {
            Normal,
            NPC_WeaponShop,
            NPC_ItemShop,
            MiniBoss,
            Boss = 30,
        }

        public enum WeaponState
        {
            Ready,
            CoolDown,
        }

        public enum InteractEventType
        {
            InteractWithItem,
            Cancel = 9,
            Finish = 10,
        }
        
        public enum GameEventType
        {
            SpawnPlayer,
            PlayerCreated,
            RoomCreated,
            GameStarted,
            RoomCleared,
            LoadNextRoom,
            
            PauseGame = 30,
            UnpauseGame = 31,
        }
    }
}

