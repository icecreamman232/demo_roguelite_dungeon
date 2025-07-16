
using SGGames.Script.Pickable;

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

        public enum BiomesName
        {
            Biome1,
            Biome2,
        }

        public enum RoomType
        {
            Normal,
            NPC_WeaponShop,
            NPC_ItemShop,
            MiniBoss,
            Boss = 30,
        }

        public enum RoomDifficulty
        {
            Easy,
            Hard,
            Challenge,
        }

        public enum WeaponState
        {
            Ready,
            CoolDown,
        }

        public enum InteractEventType
        {
            Interact,
            Cancel = 9,
            Finish = 10,
        }

        public enum InventoryEventType
        {
            Add,
            Remove,
        }

        public enum EaseType
        {
            Linear,
            EaseInSine, EaseOutSine, EaseInOutSine,
            EaseInQuad, EaseOutQuad, EaseInOutQuad,
            EaseInCubic, EaseOutCubic, EaseInOutCubic,
            EaseInQuart, EaseOutQuart, EaseInOutQuart,
            EaseInQuint, EaseOutQuint, EaseInOutQuint,
            EaseInExpo, EaseOutExpo, EaseInOutExpo,
            EaseInCirc, EaseOutCirc, EaseInOutCirc,
            EaseInBack, EaseOutBack, EaseInOutBack,
            EaseInElastic, EaseOutElastic, EaseInOutElastic,
            AnimationCurve
        }
        
        public enum GameEventType
        {
            SpawnPlayer,
            PlayerCreated,
            RoomCreated,
            GameStarted,
            RoomCleared,
            LoadNextRoomLeftRoom,
            LoadNextRoomRightRoom,
            LoadNextBiomes,
            
            PauseGame = 30,
            UnpauseGame = 31,
        }
    }
}

