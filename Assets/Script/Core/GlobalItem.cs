using System;

namespace SGGames.Script.Core
{
    public static partial class Global
    {
        public enum PickableType
        {
            AutoPick, //Will automatically fly to player/pick by player if they are in range
            ManualPick, //Require player press pick button/prompt to pick it up
        }

        public enum ItemRarity
        {
            Common,
            Uncommon,
            Rare,
            Legendary,
        }

        [Flags]
        public enum PlayerEvents
        {
            OnDashHitObstacle = 1 << 0,
            OnDashFinished = 1 << 1,
            OnTakingHit = 1 << 2, //Getting hit from enemy
            OnWeaponAttack = 1 << 3 , //Trigger when player attack
            OnWeaponHit = 1 << 4, //Trigger when attack projectile hit target either obstacle or target
        }

        public enum WorldEvents
        {
            OnEnterNewRoom,
            OnRoomCleared,
        }
        
        public enum ItemClass
        {
            /// <summary>
            /// Gián đoạn để giải phóng
            /// </summary>
            FractureTrigger,
            
            /// <summary>
            /// Tương tác chéo
            /// </summary>
            CrossConditionChain,
            
            /// <summary>
            /// Ghi dấu(đếm) hành vi
            /// </summary>
            MomentumStack,
            
            /// <summary>
            /// Hành vi đa tầng
            /// </summary>
            LayeredBehavior,
            
            /// <summary>
            /// Kích hoạt trễ
            /// </summary>
            DelayedTrigger,
            
            /// <summary>
            /// Sao chép hành vi
            /// </summary>
            MimicTrigger,
            
            /// <summary>
            /// Item cộng hưởng
            /// </summary>
            TraitResonance,
            
            /// <summary>
            /// Bất ổn có kiểm soát
            /// </summary>
            ControlledChaosTrigger,
        }

        public enum ItemID
        {
            None,
            Coin,
            Key,
            Bomb,
            
            //Fracture Trigger Item
            GreenMushroom = 10,
            RedMushroom,
            ReboundCowl,
        }
    }
}