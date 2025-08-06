namespace SGGames.Script.Core
{
    public static partial class Global
    {
        public static float HP_PER_SLOT = 50f;
        public static float S_FLIPPING_MODEL_SPEED = 3;
        
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

        public enum RoomRewardType
        {
            None,
            Coin,
            Key,
            Bomb,
            Item,
            Weapon,
            
            Random,
            HealingRoom,
            SacrificeHealthRoom,
            WeaponSmith,
            
            COUNT,
        }

        public enum WeaponState
        {
            Ready,
            CoolDown,
            AttackCombo,
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
            PlayBiomesTransition,
            LoadNextBiomes,
            GameOver,
            
            PauseGame = 30,
            UnpauseGame = 31,
        }
        
        
    }
}

