
namespace SGGames.Script.Core
{
    public static partial class Global
    {
        public const float k_DefaultLoadingTime = 0.5f;

        //For enemy AI
        public enum ActionState
        {
            NotStarted,
            InProgress,
            Completed,
        }
        
        public enum TurnBaseState
        {
            PlayerTakeTurn,
            PlayerFinishedTurn,
            EnemyTakeTurn,
            EnemyFinishedTurn,
        }
        
        public enum ComparisonType
        {
            LesserAndEqual,
            Lesser,
            Equal,
            Greater,
            GreaterAndEqual,
        }

        public static bool ComparisionBetweenFloat(float leftVal, float rightVal, ComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case ComparisonType.LesserAndEqual:
                    if (leftVal <= rightVal) return true;
                    break;
                case ComparisonType.Lesser:
                    if (leftVal < rightVal) return true;
                    break;
                case ComparisonType.Equal:
                    if (leftVal == rightVal) return true;
                    break;
                case ComparisonType.Greater:
                    if (leftVal > rightVal) return true;
                    break;
                case ComparisonType.GreaterAndEqual:
                    if (leftVal >= rightVal) return true;
                    break;
            }
            return false;
        }

        public enum PlayerDashState
        {
            Ready,
            ShowRange,
            Dashing,
            Cooldown,
        }
        
        public enum MovementDirectionType
        {
            FourDirections,
            EightDirections,
        }

        public enum MovementState
        {
            Ready,
            Moving,
            DelayAfterMoving,
            Finish
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
            InProgress,
            CoolDown,
            AttackCombo,
            Complete
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

        public enum LoadingScreenType
        {
            FadeInFromBlack,
            FadeOutToBlack,
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

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        }

        public enum EffectTileType
        {
            None,
            Indicator,
            Fire,
            Poison,
        }

        public enum HudButtonType
        {
            SpecialAbilityButton,
            ExecuteAbilityButton,
            CancelAbilityButton,
        }

        public enum AbilityState
        {
            Ready,
            ShowRange,
            Executing,
            Cooldown,
        }

        public enum AbilityID
        {
            Dash,
        }
    }
}

