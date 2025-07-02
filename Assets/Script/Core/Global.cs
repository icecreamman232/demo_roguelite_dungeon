
namespace SGGames.Script.Core
{
    public static class Global
    {
        public static readonly float HP_PER_SLOT = 50f;

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

        public enum GameEventType
        {
            SpawnPlayer,
            PlayerCreated,
            GameStarted,
        }
    }
}

