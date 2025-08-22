using System;

namespace SGGames.Scripts.Core
{
    public static partial class Global
    {
        public enum EnemyGrade
        {
            Normal,
            Elite,
        }

        [Flags]
        public enum EnemyProperties
        {
            Normal = 0,
            
            BurnResist = 1,
            PoisonResist = 2,
            FrozenResist = 4,
            ShockResist = 8,
            StunnedResist = 16,
            
            
        }
    }
}