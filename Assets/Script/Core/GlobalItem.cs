namespace SGGames.Script.Core
{
    public static partial class Global
    {
        public enum PickableType
        {
            AutoPick, //Will automatically fly to player/pick by player if they are in range
            ManualPick, //Require player press pick button/prompt to pick it up
        }

        public enum ItemID
        {
            None,
            Coin,
            Key,
            
            GreenMushroom = 10,
            RedMushroom,
        }
        
        public enum ItemEffectID
        {
            IncreasePlayerSize,
            DecreasePlayerSize,
            IncreaseFlatDamage,
            IncreaseFlatMoveSpeed,
        }

        public enum ItemEffectTag
        {
            ModifySize,
            ModifyDamage,
            ModifySpeed,
        }
    }
}