using System;
using UnityEngine;

namespace SGGames.Script.Core
{
    public static partial class Global
    {
        public static float HP_PER_SLOT = 50f;
        public static float S_FLIPPING_MODEL_SPEED = 3;
        public const float k_DefaultLoadingTime = 0.5f;
        
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
            Debug.Log("False Comparison");
            return false;
        }
        
        public enum MovementType
        {
            Normal,
            Stop,
        }

        public enum MovementState
        {
            Ready,
            Moving,
            DelayAfterMoving,
            Finish
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
    }
}

